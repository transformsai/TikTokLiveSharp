using Newtonsoft.Json.Linq;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TikTokLiveSharp.Client.Proxy;
using TikTokLiveSharp.Client.Requests;
using TikTokLiveSharp.Client.Sockets;
using TikTokLiveSharp.Errors;
using TikTokLiveSharp.Errors.Connections;
using TikTokLiveSharp.Errors.FetchErrors;
using TikTokLiveSharp.Models;
using TikTokLiveSharp.Networking;
using TikTokLiveSharp.Utils;

namespace TikTokLiveSharp.Client
{
    public abstract class TikTokBaseClient
    {
        public Dictionary<int, TikTokGift> AvailableGifts => availableGifts;

        public bool Connected => socketClient?.IsConnected ?? false;

        public string RoomID => roomID;

        public JObject RoomInfo => roomInfo;

        public string UniqueID => hostName;

        public int? ViewerCount => viewerCount;



        protected Dictionary<string, object> clientParams;

        protected ClientSettings Settings;
        private string hostName;

        private CancellationToken token;
        private TikTokHTTPClient httpClient;
        protected TikTokWebSocket socketClient;
        protected bool isConnecting;
        protected bool isPolling;

        private Task runningTask, pollingTask; 
        
        private Dictionary<int, TikTokGift> availableGifts;
        private string roomID;
        private JObject roomInfo;
        private int? viewerCount;


       

        #region Constructors
        public TikTokBaseClient(string hostId, ClientSettings? settings = null, Dictionary<string, object> clientParams = null)
        {
            hostName = hostId;
            if (!settings.HasValue)
                settings = Constants.DEFAULT_SETTINGS;
            Settings = settings.Value;
            CheckSettings();
            roomInfo = null;
            availableGifts = new Dictionary<int, TikTokGift>();
            roomID = null;
            viewerCount = null;
            isConnecting = false;
            this.clientParams = new Dictionary<string, object>();
            foreach (var parameter in Constants.DEFAULT_CLIENT_PARAMS)
                this.clientParams.Add(parameter.Key, parameter.Value);
            if (clientParams != null)
                foreach (var param in clientParams)
                    this.clientParams[param.Key] = param.Value;
            this.clientParams["app_language"] = Settings.ClientLanguage;
            this.clientParams["webcast_language"] = Settings.ClientLanguage;

            httpClient = new TikTokHTTPClient(Settings.Timeout, Settings.Proxy);
        }

        public TikTokBaseClient(string uniqueID,
            TimeSpan? timeout,
            TimeSpan? pollingInterval,
            Dictionary<string, object> clientParams = null,
            bool processInitialData = true,
            bool fetchRoomInfoOnConnect = true,
            bool enableExtendedGiftInfo = true,
            RotatingProxy proxyHandler = null,
            string lang = "en-US", uint socketBufferSize = 10_000)
            : this(uniqueID,
                  new ClientSettings
                  {
                      Timeout = timeout ?? TimeSpan.FromSeconds(Constants.DEFAULT_TIMEOUT),
                      PollingInterval = pollingInterval ?? TimeSpan.FromSeconds(Constants.DEFAULT_POLLTIME),
                      HandleExistingMessagesOnConnect = processInitialData,
                      FetchRoomInfoOnConnect = fetchRoomInfoOnConnect,
                      DownloadGiftInfo = enableExtendedGiftInfo,
                      Proxy = proxyHandler,
                      ClientLanguage = lang,
                      SocketBufferSize = socketBufferSize
                  },
                  clientParams)
        { }

        private void CheckSettings()
        {
            ClientSettings s = Settings;
            if (Settings.Timeout == default)
                s.Timeout = Constants.DEFAULT_SETTINGS.Timeout;
            if (Settings.PollingInterval == default)
                s.PollingInterval = Constants.DEFAULT_SETTINGS.PollingInterval;
            if (string.IsNullOrEmpty(Settings.ClientLanguage))
                s.ClientLanguage = Constants.DEFAULT_SETTINGS.ClientLanguage;
            if (Settings.SocketBufferSize == 0)
                Settings.SocketBufferSize = Constants.DEFAULT_SETTINGS.SocketBufferSize;
            Settings = s;
        }
        #endregion

        #region Connect
        public void Run(CancellationToken? cancellationToken = null, bool retryConnection = false)
        {
            token = cancellationToken ?? new CancellationToken();
            token.ThrowIfCancellationRequested();
            var run = Task.Run(() => Start(token, retryConnection), token);
            run.Wait();
            runningTask.Wait();
            pollingTask.Wait();
        }

        /// <exception cref="AlreadyConnectedException"></exception>
        /// <exception cref="AlreadyConnectingException"></exception>
        public async Task<string> Start(CancellationToken? cancellationToken = null, bool retryConnection = false)
        {
            token = cancellationToken ?? new CancellationToken();
            token.ThrowIfCancellationRequested();
            try
            { 
                return await Connect();
            }
            catch (LiveNotFoundException)
            {
                throw; // LiveStream was not Found (or Host is not Online)
            }
            catch (FailedConnectionException)
            {
                // Failed to Connect, but Host was Online
                if (retryConnection)
                {
                    await Task.Delay(Settings.PollingInterval);
                    return await Start(cancellationToken, retryConnection);
                }
                return null;
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if (ex is OperationCanceledException) // Cancelled via token. Clean exit
                        return true;
                    if (ex is AlreadyConnectedException || ex is AlreadyConnectingException) // Already connecting, Skip duplicate Start
                        return true;
                    if (ex is FailedFetchRoomInfoException) // Failed to fetch RoomInfo for HostID
                        return false;
                    return false;
                });
                return null;
            }
        }

        protected virtual async Task<string> Connect()
        {
            if (isConnecting) throw new AlreadyConnectingException();
            if (Connected) throw new AlreadyConnectedException();
            isConnecting = true;
            await FetchRoomId();
            token.ThrowIfCancellationRequested();
            if (Settings.FetchRoomInfoOnConnect)
            {
                JObject info = await FetchRoomInfo();
                JToken status = info["data"]["status"];
                if (status == null || status.Value<int>() == 4)
                    throw new LiveNotFoundException("LiveStream for HostID could not be found. Is the Host online?");
            }
            token.ThrowIfCancellationRequested();
            if (Settings.DownloadGiftInfo)
                await FetchAvailableGifts();
            token.ThrowIfCancellationRequested();
            WebcastResponse response = await FetchRoomData();
            token.ThrowIfCancellationRequested();
            await CreateWebSocket(response);
            token.ThrowIfCancellationRequested();
            return roomID;
        }

        protected async Task CreateWebSocket(WebcastResponse webcastResponse)
        {
            if (webcastResponse.wsUrl != null && webcastResponse.wsParam != null)
            {
                try
                {
                    clientParams[webcastResponse.wsParam.Name] = webcastResponse.wsParam.Value;
                    string url = $"{webcastResponse.wsUrl}?{string.Join("&", clientParams.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value.ToString())}"))}";
                    socketClient = new TikTokWebSocket(TikTokHttpRequest.CookieJar, Settings.SocketBufferSize);
                    await socketClient.Connect(url);
                    runningTask = Task.Run(WebSocketLoop, token);
                    pollingTask = Task.Run(PingLoop, token);
                }
                catch (Exception e)
                {
                    throw new FailedConnectionException("Failed to connect to the websocket", e);
                }
                if (Settings.HandleExistingMessagesOnConnect)
                    HandleWebcastMessages(webcastResponse);
            }
            else throw new LiveNotFoundException("Could not find Room");
        }
        #endregion

        #region Disconnect
        public async Task Stop()
        {
            if (Connected)
                await Disconnect();
        }

        protected virtual async Task Disconnect()
        {
            isPolling = false;
            roomInfo = null;
            isConnecting = false;
            if (Connected)
                await socketClient.Disconnect();
            clientParams["cursor"] = string.Empty;
            await runningTask;
            await pollingTask;
        }
        #endregion

        #region FetchData
        protected async Task<string> FetchRoomId()
        {
            string html = null;
            try
            {
                html = await httpClient.GetLivestreamPage(hostName);
            }
            catch (Exception e)
            {
                new FailedFetchRoomInfoException("Failed to fetch room id from WebCast, see stacktrace for more info.", e);
            }
            var first = Regex.Match(html, "room_id=([0-9]*)");
            var second = Regex.Match(html, "\"roomId\":\"([0 - 9] *)\"");
            string id = first.Groups[1]?.Value ?? second.Groups[1]?.Value ?? string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                clientParams["room_id"] = id;
                roomID = id;
                return id;
            }
            else
                throw new FailedFetchRoomInfoException(html.Contains("\"og:url\"") ? "User might be offline" : "Your IP or country might be blocked by TikTok.");
        }

        protected async Task<Dictionary<int, TikTokGift>> FetchAvailableGifts()
        {
            try
            {
                JObject response = await httpClient.GetJObjectFromWebcastAPI("gift/list/", clientParams);
                var giftTokens = response.SelectTokens("..gifts")?.FirstOrDefault()?.Children() ?? null;
                if (giftTokens == null)
                    return new Dictionary<int, TikTokGift>();
                foreach (JToken giftToken in giftTokens)
                {
                    TikTokGift gift = giftToken.ToObject<TikTokGift>();
                    availableGifts[gift.id] = gift;
                }
                return availableGifts;
            }
            catch (Exception e)
            {
                throw new FailedFetchGiftsException("Failed to fetch giftTokens from WebCast, see stacktrace for more info.", e);
            }
        }

        protected async Task<WebcastResponse> FetchRoomData()
        {
            var webcastResponse = await httpClient.GetDeserializedMessage("im/fetch/", clientParams, true);
            clientParams["cursor"] = webcastResponse.Cursor;
            clientParams["internal_ext"] = webcastResponse.internalExt;
            return webcastResponse;
        }

        protected async Task<JObject> FetchRoomInfo()
        {
            try
            {
                JObject response = await httpClient.GetJObjectFromWebcastAPI("room/info/", clientParams);
                roomInfo = response;
                return response;
            }
            catch (Exception e)
            {
                throw new FailedFetchRoomInfoException("Failed to fetch room info from WebCast, see stacktrace for more info.", e);
            }
        }
        #endregion

        #region SocketLoop
        protected async Task WebSocketLoop()
        {
            while (true)
            {
                TikTokWebSocketResponse response = await socketClient.ReceiveMessage();
                if (response == null) 
                    continue;
                try
                {
                    using (var websocketMessageStream = new MemoryStream(response.Array, 0, response.Count))
                    {
                        string msg = Convert.ToBase64String(response.Array, 0, response.Count);
                        Console.WriteLine(1);
                        Console.WriteLine(msg);
                        WebcastWebsocketMessage websocketMessage = Serializer.Deserialize<WebcastWebsocketMessage>(websocketMessageStream);
                        Console.WriteLine(2);
                        if (websocketMessage.Binary != null)
                        {
                            using (var messageStream = new MemoryStream(websocketMessage.Binary))
                            {
                                WebcastResponse message = Serializer.Deserialize<WebcastResponse>(messageStream);
                                await SendAcknowledgement(websocketMessage.Id);
                                HandleWebcastMessages(message);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    throw new WebSocketException("Websocket has likely been closed.", e);
                }
            }
        }

        /// <summary>
        /// Pings the websocket.
        /// </summary>
        /// <returns>Task to await.</returns>
        protected async Task PingLoop()
        {
            while (true)
            {
                using (var messageStream = new MemoryStream())
                    await socketClient.WriteMessage(new ArraySegment<byte>(new byte[] { 58, 2, 104, 98 }));
                await Task.Delay(10);
            }
        }

        /// <summary>
        /// Send an acknowlegement to the websocket.
        /// </summary>
        /// <param name="id">The acknowledgment id.</param>
        /// <returns>Task to await.</returns>
        protected async Task SendAcknowledgement(ulong id)
        {
            using (var messageStream = new MemoryStream())
            {
                Serializer.Serialize(messageStream, new WebcastWebsocketAck
                {
                    Type = "ack",
                    Id = id
                });
                await socketClient.WriteMessage(new ArraySegment<byte>(messageStream.ToArray()));
            }
        }
        #endregion

        /// <summary>
        /// Handles the webcast messages
        /// </summary>
        /// <param name="webcastResponse">The current webcast response.</param>
        protected abstract void HandleWebcastMessages(WebcastResponse webcastResponse);
    }
}