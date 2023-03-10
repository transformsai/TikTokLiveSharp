using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Events;
using TikTokLiveSharp.Events.MessageData.Messages;
using TikTokLiveSharp.Events.MessageData.Objects;
using TikTokLiveSharp.Models.Protobuf;
using TikTokLiveSharp.Unity.Utils;
using UnityEngine;
using UnityEngine.Events;
using LinkMicMethod = TikTokLiveSharp.Events.MessageData.Messages.LinkMicMethod;
using Picture = TikTokLiveSharp.Events.MessageData.Objects.Picture;
using RoomMessage = TikTokLiveSharp.Events.MessageData.Messages.RoomMessage;

namespace TikTokLiveSharp.Unity
{
    public class TikTokLiveManager : MonoBehaviour
    {
        #region Singleton
        [SerializeField]
        private bool hasRootObject;

        public static bool Exists => _instance != null;

        public static TikTokLiveManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                GameObject obj = new GameObject("TikTokLiveManager");
                _instance = obj.AddComponent<TikTokLiveManager>();
                Debug.LogWarning("TikTokLiveManager-Instance does not exist. Created new Instance", obj);
                return _instance;
            }
            protected set { _instance = value; }
        }

        protected static TikTokLiveManager _instance;
        #endregion

        #region Public

        public IReadOnlyDictionary<int, TikTokGift> AvailableGifts => (IReadOnlyDictionary<int, TikTokGift>)client?.AvailableGifts;

        public bool Connected => client?.Connected ?? false;

        public string RoomID => currRoomHost;

        public JObject RoomInfo => client?.RoomInfo;

        public string UniqueID => client?.UniqueID;

        public uint? ViewerCount => client?.ViewerCount;
        #endregion


        [Min(1)]
        [SerializeField]
        private uint texCacheSize = 2048;

        [SerializeField]
        private bool autoConnect;

        [SerializeField]
        private string autoConnectHostId;

        #region ConnectionSettings

        [SerializeField]
        private ClientSettings settings;
        #endregion

        private string currRoomHost;

        private TikTokLiveClient client;
        private TextureCache textureCache;

        private CancellationToken? cancelToken = null;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        #region Events
        #region C#
        public event EventHandler<Exception> OnException;

        /// <summary>
        /// Event fired when the client connects.
        /// </summary>
        public event TikTokEventHandler<bool> OnConnected;

        /// <summary>
        /// Event fired when the client disconnects.
        /// </summary>
        public event TikTokEventHandler<bool> OnDisconnected;

        public event TikTokEventHandler<RoomMessage> OnRoomIntro;

        public event TikTokEventHandler<RoomMessage> OnRoomMessage;

        public event TikTokEventHandler<RoomMessage> OnSystemMessage;

        /// <summary>
        /// Event fired when comments are received.
        /// </summary>
        public event TikTokEventHandler<Comment> OnComment;


        /// <summary>
        /// Event fired when the view count updates.
        /// </summary>
        public event TikTokEventHandler<RoomViewerData> OnViewerData;

        /// <summary>
        /// Event fired when the live has ended.
        /// </summary>
        public event TikTokEventHandler OnLiveEnded;
        public event TikTokEventHandler OnLivePaused;


        /// <summary>
        /// Event fired when a gift-Message is received.
        /// <para>
        /// Fires for every GiftMessage
        /// </para>
        /// </summary>
        public event TikTokEventHandler<GiftMessage> OnGiftMessage;
        /// <summary>
        /// Event fired when a gift-Message is received.
        /// <para>
        /// Fires when a gift-streak starts. Gift-Events are fired for subsequent messages.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<TikTokGift> OnGift;
        /// <summary>
        /// Event fired when someone likes the stream.
        /// </summary>
        public event TikTokEventHandler<Like> OnLike;

        /// <summary>
        /// Event fired when the stream is shared.
        /// </summary>
        public event TikTokEventHandler<Share> OnShare;

        /// <summary>
        /// Event fired when someone follows.
        /// </summary>
        public event TikTokEventHandler<Follow> OnFollow;

        /// <summary>
        /// Event fired when someone joins the stream.
        /// </summary>
        public event TikTokEventHandler<Join> OnJoin;

        /// <summary>
        /// Event fired when a user subscribes.
        /// </summary>
        public event TikTokEventHandler<Subscribe> OnSubscribe;

        /// <summary>
        /// Event fired when a LinkMicBattle starts
        /// </summary>
        public event TikTokEventHandler<LinkMicBattle> OnLinkMicBattle;

        public event TikTokEventHandler<LinkMicArmies> OnLinkMicArmies;

        /// <summary>
        /// Data for LinkMic-Connection(s)
        /// </summary>
        public event TikTokEventHandler<LinkMicMethod> OnLinkMicMethod;

        public event TikTokEventHandler<LinkMicFanTicket> OnLinkMicFanTicket;

        public event TikTokEventHandler<RankText> OnRankText;
        public event TikTokEventHandler<RankUpdate> OnRankUpdate;

        /// <summary>
        /// Closed Caption-Message for Video
        /// </summary>
        public event TikTokEventHandler<Caption> OnClosedCaption;

        public event TikTokEventHandler<PollMessage> OnPollMessage;

        public event TikTokEventHandler<InRoomBanner> OnInRoomBanner;

        /// <summary>
        /// Event Fired when Host pins a Comment to the Stream
        /// </summary>
        public event TikTokEventHandler<RoomPinMessage> OnRoomPinMessage;

        public event TikTokEventHandler<DetectMessage> OnDetectMessage;

        public event TikTokEventHandler<BarrageMessage> OnBarrageMessage;

        public event TikTokEventHandler<GoalUpdate> OnGoalUpdate;

        public event TikTokEventHandler<UnauthorizedMember> OnUnauthorizedMember;

        public event TikTokEventHandler<LinkMessage> OnLinkMessage;

        public event TikTokEventHandler<LinkLayerMessage> OnLinkLayerMessage;

        public event TikTokEventHandler<GiftBroadcast> OnGiftBroadcast;

        public event TikTokEventHandler<ShopMessage> OnShopMessage;

        public event TikTokEventHandler<IMDelete> OnIMDelete;

        public event TikTokEventHandler<Question> OnQuestion;

        public event TikTokEventHandler<Envelope> OnEnvelope;

        public event TikTokEventHandler<SubNotify> OnSubNotify;

        public event TikTokEventHandler<Emote> OnEmote;
        /// <summary>
        /// Event fired when an unhandled social event is received from the webcast.
        /// It's up to you how you can interpret this message.
        /// </summary>
        public event TikTokEventHandler<WebcastSocialMessage> UnhandledSocialEvent;

        /// <summary>
        /// Event fired when an unhandled member event is received from the webcast.
        /// It's up to you how you can interpret this message.
        /// </summary>
        public event TikTokEventHandler<WebcastMemberMessage> UnhandledMemberEvent;

        /// <summary>
        /// Event fired when an unhandled event is received from the webcast.
        /// It's up to you how you can interpret this message.
        /// </summary>
        public event TikTokEventHandler<Message> UnhandledEvent;
        #endregion

        #region UnityEvents
        [SerializeField]
        private UnityEvent<Exception> onException;
        [SerializeField]
        private UnityEvent<bool> onConnected;
        [SerializeField]
        private UnityEvent<bool> onDisconnected;
        [SerializeField]
        private UnityEvent<RoomMessage> onRoomIntro;
        [SerializeField]
        private UnityEvent<RoomMessage> onRoomMessage;
        [SerializeField]
        private UnityEvent<RoomMessage> onSystemMessage;
        [SerializeField]
        private UnityEvent<Comment> onComment;
        [SerializeField]
        private UnityEvent<RoomViewerData> onViewerData;
        [SerializeField]
        private UnityEvent onLiveEnded;
        [SerializeField]
        private UnityEvent onLivePaused;
        [SerializeField]
        private UnityEvent<GiftMessage> onGiftMessage;
        [SerializeField]
        private UnityEvent<TikTokGift> onGift;
        [SerializeField]
        private UnityEvent<Like> onLike;
        [SerializeField]
        private UnityEvent<Share> onShare;
        [SerializeField]
        private UnityEvent<Follow> onFollow;
        [SerializeField]
        private UnityEvent<Join> onJoin;
        [SerializeField]
        private UnityEvent<Subscribe> onSubscribe;
        [SerializeField]
        private UnityEvent<LinkMicBattle> onLinkMicBattle;
        [SerializeField]
        private UnityEvent<LinkMicArmies> onLinkMicArmies;
        [SerializeField]
        private UnityEvent<LinkMicMethod> onLinkMicMethod;
        [SerializeField]
        private UnityEvent<LinkMicFanTicket> onLinkMicFanTicket;
        [SerializeField]
        private UnityEvent<RankText> onRankText;
        [SerializeField]
        private UnityEvent<RankUpdate> onRankUpdate;
        [SerializeField]
        private UnityEvent<Caption> onClosedCaption;
        [SerializeField]
        private UnityEvent<PollMessage> onPollMessage;
        [SerializeField]
        private UnityEvent<InRoomBanner> onInRoomBanner;
        [SerializeField]
        private UnityEvent<RoomPinMessage> onRoomPinMessage;
        [SerializeField]
        private UnityEvent<DetectMessage> onDetectMessage;
        [SerializeField]
        private UnityEvent<BarrageMessage> onBarrageMessage;
        [SerializeField]
        private UnityEvent<GoalUpdate> onGoalUpdate;
        [SerializeField]
        private UnityEvent<UnauthorizedMember> onUnauthorizedMember;
        [SerializeField]
        private UnityEvent<LinkMessage> onLinkMessage;
        [SerializeField]
        private UnityEvent<LinkLayerMessage> onLinkLayerMessage;
        [SerializeField]
        private UnityEvent<GiftBroadcast> onGiftBroadcast;
        [SerializeField]
        private UnityEvent<ShopMessage> onShopMessage;
        [SerializeField]
        private UnityEvent<IMDelete> onIMDelete;
        [SerializeField]
        private UnityEvent<Question> onQuestion;
        [SerializeField]
        private UnityEvent<Envelope> onEnvelope;
        [SerializeField]
        private UnityEvent<SubNotify> onSubNotify;
        [SerializeField]
        private UnityEvent<Emote> onEmote;
        [SerializeField]
        private UnityEvent<WebcastSocialMessage> onUnhandledSocialEvent;
        [SerializeField]
        private UnityEvent<WebcastMemberMessage> onUnhandledMemberEvent;
        [SerializeField]
        private UnityEvent<Message> onUnhandledEvent;
        #endregion
        #endregion



        public async void ConnectToStreamAsync(string userID, Action<Exception> onConnectException = null)
        {
            await ConnectToStream(userID, onConnectException);
        }

        public async Task ConnectToStream(string userID, Action<Exception> onConnectException = null)
        {
            if (string.IsNullOrWhiteSpace(userID))
                throw new ArgumentNullException(nameof(userID), "Did not provide a UserID to connect to");
            try
            {
                if (client != null)
                    await DisconnectFromLivestream();
                client = new TikTokLiveClient(userID, settings, null);
                currRoomHost = userID;
                // Set up Events for Client
                SetupEvents(client);
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
                onConnectException?.Invoke(e);
                return;
            }
            try
            {
                cancelToken = tokenSource.Token;
                await client.Start(cancelToken, onConnectException, settings.RetryOnConnectionFailure);
                Debug.Log("Connected");
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
                onConnectException?.Invoke(e);
            }
        }

        private void LinkUnityEvents()
        {
            OnException += (_, args) => onException?.Invoke(args);
            OnConnected += (_, args) => onConnected?.Invoke(args);
            OnDisconnected += (_, args) => onDisconnected?.Invoke(args);
            OnRoomIntro += (_, args) => onRoomIntro?.Invoke(args);
            OnRoomMessage += (_, args) => onRoomMessage?.Invoke(args);
            OnSystemMessage += (_, args) => onSystemMessage?.Invoke(args);
            OnComment += (_, args) => onComment?.Invoke(args);
            OnViewerData += (_, args) => onViewerData?.Invoke(args);
            OnLiveEnded += (_, _) => onLiveEnded?.Invoke();
            OnLivePaused += (_, _) => onLivePaused?.Invoke();
            OnGiftMessage += (_, args) => onGiftMessage?.Invoke(args);
            OnGift += (_, args) => onGift?.Invoke(args);
            OnLike += (_, args) => onLike?.Invoke(args);
            OnShare += (_, args) => onShare?.Invoke(args);
            OnFollow += (_, args) => onFollow?.Invoke(args);
            OnJoin += (_, args) => onJoin?.Invoke(args);
            OnSubscribe += (_, args) => onSubscribe?.Invoke(args);
            OnLinkMicBattle += (_, args) => onLinkMicBattle?.Invoke(args);
            OnLinkMicArmies += (_, args) => onLinkMicArmies?.Invoke(args);
            OnLinkMicMethod += (_, args) => onLinkMicMethod?.Invoke(args);
            OnLinkMicFanTicket += (_, args) => onLinkMicFanTicket?.Invoke(args);
            OnRankText += (_, args) => onRankText?.Invoke(args);
            OnRankUpdate += (_, args) => onRankUpdate?.Invoke(args);
            OnClosedCaption += (_, args) => onClosedCaption?.Invoke(args);
            OnPollMessage += (_, args) => onPollMessage?.Invoke(args);
            OnInRoomBanner += (_, args) => onInRoomBanner?.Invoke(args);
            OnRoomPinMessage += (_, args) => onRoomPinMessage?.Invoke(args);
            OnDetectMessage += (_, args) => onDetectMessage?.Invoke(args);
            OnBarrageMessage += (_, args) => onBarrageMessage?.Invoke(args);
            OnGoalUpdate += (_, args) => onGoalUpdate?.Invoke(args);
            OnUnauthorizedMember += (_, args) => onUnauthorizedMember?.Invoke(args);
            OnLinkMessage += (_, args) => onLinkMessage?.Invoke(args);
            OnLinkLayerMessage += (_, args) => onLinkLayerMessage?.Invoke(args);
            OnGiftBroadcast += (_, args) => onGiftBroadcast?.Invoke(args);
            OnShopMessage += (_, args) => onShopMessage?.Invoke(args);
            OnIMDelete += (_, args) => onIMDelete?.Invoke(args);
            OnQuestion += (_, args) => onQuestion?.Invoke(args);
            OnEnvelope += (_, args) => onEnvelope?.Invoke(args);
            OnSubNotify += (_, args) => onSubNotify?.Invoke(args);
            OnEmote += (_, args) => onEmote?.Invoke(args);
            UnhandledSocialEvent += (_, args) => onUnhandledSocialEvent?.Invoke(args);
            UnhandledMemberEvent += (_, args) => onUnhandledMemberEvent?.Invoke(args);
            UnhandledEvent += (_, args) => onUnhandledEvent?.Invoke(args);
        }

        private void SetupEvents(TikTokLiveClient client)
        {
            client.OnException += OnException.Invoke;
            client.OnConnected += OnConnected.Invoke;
            client.OnDisconnected += OnDisconnected.Invoke;
            client.OnRoomIntro += OnRoomIntro.Invoke;
            client.OnRoomMessage += OnRoomMessage.Invoke;
            client.OnSystemMessage += OnSystemMessage.Invoke;
            client.OnComment += OnComment.Invoke;
            client.OnViewerData += OnViewerData.Invoke;
            client.OnLiveEnded += OnLiveEnded.Invoke;
            client.OnLivePaused += OnLivePaused.Invoke;
            client.OnGiftMessage += OnGiftMessage.Invoke;
            client.OnGift += OnGift.Invoke;
            client.OnLike += OnLike.Invoke;
            client.OnShare += OnShare.Invoke;
            client.OnFollow += OnFollow.Invoke;
            client.OnJoin += OnJoin.Invoke;
            client.OnSubscribe += OnSubscribe.Invoke;
            client.OnLinkMicBattle += OnLinkMicBattle.Invoke;
            client.OnLinkMicArmies += OnLinkMicArmies.Invoke;
            client.OnLinkMicMethod += OnLinkMicMethod.Invoke;
            client.OnLinkMicFanTicket += OnLinkMicFanTicket.Invoke;
            client.OnRankText += OnRankText.Invoke;
            client.OnRankUpdate += OnRankUpdate.Invoke;
            client.OnClosedCaption += OnClosedCaption.Invoke;
            client.OnPollMessage += OnPollMessage.Invoke;
            client.OnInRoomBanner += OnInRoomBanner.Invoke;
            client.OnRoomPinMessage += OnRoomPinMessage.Invoke;
            client.OnDetectMessage += OnDetectMessage.Invoke;
            client.OnBarrageMessage += OnBarrageMessage.Invoke;
            client.OnGoalUpdate += OnGoalUpdate.Invoke;
            client.OnUnauthorizedMember += OnUnauthorizedMember.Invoke;
            client.OnLinkMessage += OnLinkMessage.Invoke;
            client.OnLinkLayerMessage += OnLinkLayerMessage.Invoke;
            client.OnGiftBroadcast += OnGiftBroadcast.Invoke;
            client.OnShopMessage += OnShopMessage.Invoke;
            client.OnIMDelete += OnIMDelete.Invoke;
            client.OnQuestion += OnQuestion.Invoke;
            client.OnEnvelope += OnEnvelope.Invoke;
            client.OnSubNotify += OnSubNotify.Invoke;
            client.OnEmote += OnEmote.Invoke;
            client.UnhandledSocialEvent += UnhandledSocialEvent.Invoke;
            client.UnhandledMemberEvent += UnhandledMemberEvent.Invoke;
            client.UnhandledEvent += UnhandledEvent.Invoke;
        }

        private void TearDownEvents(TikTokLiveClient client)
        {
            client.OnException -= OnException.Invoke;
            client.OnConnected -= OnConnected.Invoke;
            client.OnDisconnected -= OnDisconnected.Invoke;
            client.OnRoomIntro -= OnRoomIntro.Invoke;
            client.OnRoomMessage -= OnRoomMessage.Invoke;
            client.OnSystemMessage -= OnSystemMessage.Invoke;
            client.OnComment -= OnComment.Invoke;
            client.OnViewerData -= OnViewerData.Invoke;
            client.OnLiveEnded -= OnLiveEnded.Invoke;
            client.OnLivePaused -= OnLivePaused.Invoke;
            client.OnGiftMessage -= OnGiftMessage.Invoke;
            client.OnGift -= OnGift.Invoke;
            client.OnLike -= OnLike.Invoke;
            client.OnShare -= OnShare.Invoke;
            client.OnFollow -= OnFollow.Invoke;
            client.OnJoin -= OnJoin.Invoke;
            client.OnSubscribe -= OnSubscribe.Invoke;
            client.OnLinkMicBattle -= OnLinkMicBattle.Invoke;
            client.OnLinkMicArmies -= OnLinkMicArmies.Invoke;
            client.OnLinkMicMethod -= OnLinkMicMethod.Invoke;
            client.OnLinkMicFanTicket -= OnLinkMicFanTicket.Invoke;
            client.OnRankText -= OnRankText.Invoke;
            client.OnRankUpdate -= OnRankUpdate.Invoke;
            client.OnClosedCaption -= OnClosedCaption.Invoke;
            client.OnPollMessage -= OnPollMessage.Invoke;
            client.OnInRoomBanner -= OnInRoomBanner.Invoke;
            client.OnRoomPinMessage -= OnRoomPinMessage.Invoke;
            client.OnDetectMessage -= OnDetectMessage.Invoke;
            client.OnBarrageMessage -= OnBarrageMessage.Invoke;
            client.OnGoalUpdate -= OnGoalUpdate.Invoke;
            client.OnUnauthorizedMember -= OnUnauthorizedMember.Invoke;
            client.OnLinkMessage -= OnLinkMessage.Invoke;
            client.OnLinkLayerMessage -= OnLinkLayerMessage.Invoke;
            client.OnGiftBroadcast -= OnGiftBroadcast.Invoke;
            client.OnShopMessage -= OnShopMessage.Invoke;
            client.OnIMDelete -= OnIMDelete.Invoke;
            client.OnQuestion -= OnQuestion.Invoke;
            client.OnEnvelope -= OnEnvelope.Invoke;
            client.OnSubNotify -= OnSubNotify.Invoke;
            client.OnEmote -= OnEmote.Invoke;
            client.UnhandledSocialEvent -= UnhandledSocialEvent.Invoke;
            client.UnhandledMemberEvent -= UnhandledMemberEvent.Invoke;
            client.UnhandledEvent -= UnhandledEvent.Invoke;
        }

        public async Task DisconnectFromLivestream()
        {
            if (client != null)
            {
                await client.Stop();
                TearDownEvents(client);
                client = null;
            }
            tokenSource?.Cancel();
        }

        protected virtual void Awake()
        {
            // Destroy any duplicate Instance
            if (Exists && _instance != this)
            {
                Debug.LogErrorFormat(_instance.gameObject, "TikTokLiveManager already exists! Existing Object: {0}. New Object: {1}. Destroying new Object", _instance.gameObject.name, gameObject.name);
                DestroyImmediate(gameObject, false);
                return;
            }
            #region Singleton
            if (transform.parent != null)
            {
                if (!hasRootObject)
                {
                    Debug.LogWarning("TikTokLiveManager is not a Root-Object. Did you mean to set hasRootObject? Unparenting", gameObject);
                    transform.SetParent(null, true);
                    DontDestroyOnLoad(gameObject);
                }
                else DontDestroyOnLoad(transform.root.gameObject);
            }
            else DontDestroyOnLoad(gameObject);
            // Set Instance
            _instance = this;
            #endregion
            #region Init
            textureCache = new TextureCache(texCacheSize);
            LinkUnityEvents();
            #endregion
        }

        private IEnumerator Start()
        {
            if (autoConnect)
            {
                yield return null;
                ConnectToStreamAsync(autoConnectHostId, null);
            }
        }

        protected virtual void OnDestroy()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DisconnectFromLivestream();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            if (Exists && ReferenceEquals(_instance, this))
            {
                #region DeInit
                textureCache = null;
                Resources.UnloadUnusedAssets();
                #endregion
                _instance = null;
            }
        }

        public void RequestImage(Picture picture, Action<Texture2D> onComplete) => RequestImage(picture.URLs, onComplete);
        public void RequestImage(IEnumerable<string> urls, Action<Texture2D> onComplete) => textureCache.RequestImage(urls, onComplete);
        public void RequestImage(string url, Action<Texture2D> onComplete) => textureCache.RequestImage(url, onComplete);

        public void RequestSprite(Picture picture, Action<Sprite> onComplete) => RequestSprite(picture.URLs, onComplete);
        public void RequestSprite(IEnumerable<string> urls, Action<Sprite> onComplete) => textureCache.RequestSprite(urls, onComplete);
        public void RequestSprite(string url, Action<Sprite> onComplete) => textureCache.RequestSprite(url, onComplete);
    }
}
