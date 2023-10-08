using Newtonsoft.Json.Linq;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TikTokLiveSharp.Client.Config;
using TikTokLiveSharp.Errors.Permissions;
using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Client.HTTP
{
    /// <summary>
    /// HTTP-Client for TikTok-WebPage & Signing-Page
    /// </summary>
    internal sealed class TikTokHttpClient
    {
        #region Properties
        /// <summary>
        /// TikTokLive-ClientName for Signing-Server
        /// </summary>
        private const string CLIENT_NAME = "ttlive-net";
        /// <summary>
        /// Number of Concurrent HTTP-Clients
        /// </summary>
        private static int concurrentClients = 0;
        /// <summary>
        /// Index for this Client
        /// </summary>
        private readonly int clientNum;
        /// <summary>
        /// Whether Compression is allowed for Responses
        /// </summary>
        private readonly bool compression;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for a HTTPClient
        /// </summary>
        /// <param name="timeout">Timeout for Connection</param>
        /// <param name="proxyHandler">Proxy for Connection</param>
        /// <param name="clientLanguage">Accepted Language for Client (ISO-format)</param>
        internal TikTokHttpClient(TimeSpan timeout, bool enableCompression = true, IWebProxy proxyHandler = null, string clientLanguage = null)
        {
            TikTokHttpRequest.Timeout = timeout;
            TikTokHttpRequest.WebProxy = proxyHandler;
            if (!string.IsNullOrEmpty(clientLanguage))
                TikTokHttpRequest.ClientLanguage = clientLanguage;
            compression = enableCompression;
            concurrentClients++;
            clientNum = concurrentClients;
        }

        /// <summary>
        /// Destructor for a HTTPClient
        /// </summary>
        ~TikTokHttpClient()
        {
            concurrentClients--;
        }
        #endregion

        #region Methods
        #region Internal
        /// <summary>
        /// Gets Response from WebCast-API
        /// </summary>
        /// <param name="path">Path to send Request to</param>
        /// <param name="parameters">Additional Parameters for Request</param>
        /// <param name="signUrl">Whether to sign URL using API</param>
        /// <returns>Task to Await. Result is Response from WebCast-API</returns>
        internal async Task<Response> GetDeserializedMessage(string path, IDictionary<string, object> parameters = null, bool signUrl = false)
        {
            HttpContent get = await GetRequest(Constants.TIKTOK_URL_WEBCAST + path, parameters, signUrl);
            return Serializer.Deserialize<Response>(await get.ReadAsStreamAsync());
        }

        /// <summary>
        /// Gets JSON-Object from WebCast-API
        /// </summary>
        /// <param name="path">Path to send Request to</param>
        /// <param name="parameters">Additional Parameters for Request</param>
        /// <param name="signUrl">Whether to sign URL using API</param>
        /// <returns>Task to Await. Result is JSON-Object</returns>
        internal async Task<JObject> GetJObjectFromWebcastApi(string path, IDictionary<string, object> parameters = null, bool signUrl = false)
        {
            HttpContent get = await GetRequest(Constants.TIKTOK_URL_WEBCAST + path, parameters, signUrl);
            return JObject.Parse(await get.ReadAsStringAsync());
        }

        /// <summary>
        /// Gets HTML for Live-Page
        /// </summary>
        /// <param name="uniqueId">ID for Host</param>
        /// <param name="signUrl">Whether to sign URL using API</param>
        /// <returns>Task to Await. Result is HTML for Live-Page</returns>
        internal async Task<string> GetLivestreamPage(string uniqueId, IDictionary<string, object> parameters = null, bool signUrl = false)
        {
            HttpContent get = await GetRequest($"{Constants.TIKTOK_URL_WEB}@{uniqueId}/live/", parameters, signUrl);
            return await get.ReadAsStringAsync();
        }

        /// <summary>
        /// Gets HTML for Live-Page
        /// </summary>
        /// <param name="uniqueId">ID for Host</param>
        /// <param name="signUrl">Whether to sign URL using API</param>
        /// <returns>Task to Await. Result is HTML for Profile-Page</returns>
        internal async Task<string> GetProfilePage(string uniqueId, IDictionary<string, object> parameters = null, bool signUrl = false)
        {
            HttpContent get = await GetRequest($"{Constants.TIKTOK_URL_WEB}@{uniqueId}", parameters, signUrl);
            return await get.ReadAsStringAsync();
        }

        /// <summary>
        /// Posts JSON-Object to WebCast-API
        /// </summary>
        /// <param name="path">Path to send to (sub-URL)</param>
        /// <param name="parameters">Additional Parameters for Request</param>
        /// <param name="json">Object to Post</param>
        /// <param name="signUrl">Whether to sign URL using API</param>
        /// <returns>Task to Await. Result is result of Post</returns>
        internal async Task<JObject> PostJObjectToWebcastApi(string path, IDictionary<string, object> parameters, JObject json, bool signUrl = false)
        {
            HttpContent post = await PostRequest(Constants.TIKTOK_URL_WEBCAST + path, json.ToString(Newtonsoft.Json.Formatting.None), parameters, signUrl);
            return JObject.Parse(await post.ReadAsStringAsync());
        }
        #endregion

        #region Private
        /// <summary>
        /// Creates HTTP-Request
        /// </summary>
        /// <param name="url">URL for Request</param>
        /// <param name="parameters">Additional Parameters for Request</param>
        /// <returns>HTTP-Request</returns>
        private static ITikTokHttpRequest BuildRequest(string url, bool enableCompression = true, IDictionary<string, object> parameters = null)
        {
            return new TikTokHttpRequest(url, enableCompression).SetQueries(parameters);
        }

        /// <summary>
        /// Sends HTTP-GetRequest to URL
        /// </summary>
        /// <param name="url">URL to send to</param>
        /// <param name="parameters">Additional Parameters for Request</param>
        /// <param name="signUrl">Whether to Sign URL using API</param>
        /// <returns>Task to await</returns>
        private async Task<HttpContent> GetRequest(string url, IDictionary<string, object> parameters = null, bool signUrl = false)
        {
            ITikTokHttpRequest request = BuildRequest(signUrl ? await GetSignedUrl(url, parameters) : url, compression, signUrl ? null : parameters);
            return await request.Get();
        }

        /// <summary>
        /// Sends HTTP-PostRequest to URL
        /// </summary>
        /// <param name="url">URL to send to</param>
        /// <param name="parameters">Additional Parameters for Request</param>
        /// <param name="signUrl">Whether to Sign URL using API</param>
        /// <returns>Task to await</returns>
        private async Task<HttpContent>PostRequest(string url, IDictionary<string, object> parameters = null, bool signUrl = false)
        {
            ITikTokHttpRequest request = BuildRequest(signUrl ? await GetSignedUrl(url, parameters) : url, compression, signUrl ? null : parameters);
            return await request.Post(null);
        }

        /// <summary>
        /// Sends HTTP-PostRequest to URL
        /// </summary>
        /// <param name="url">URL to send to</param>
        /// <param name="data">Data for Request</param>
        /// <param name="parameters">Additional Parameters for Request</param>
        /// <param name="signUrl">Whether to Sign URL using API</param>
        /// <returns>Task to await</returns>
        private async Task<HttpContent> PostRequest(string url, string data, IDictionary<string, object> parameters = null, bool signUrl = false)
        {
            ITikTokHttpRequest request = BuildRequest(signUrl ? await GetSignedUrl(url, parameters) : url, compression, signUrl ? null : parameters);
            return await request.Post(new StringContent(data, Encoding.UTF8));
        }

        /// <summary>
        /// Signs URL using WebAPI
        /// </summary>
        /// <param name="url">URL to sign</param>
        /// <param name="parameters">Additional Parameters for Request</param>
        /// <returns>Task to await. Result is Signed URL</returns>
        /// <exception cref="InsufficientPermissionException"></exception>
        private async Task<string> GetSignedUrl(string url, IDictionary<string, object> parameters = null)
        {
            string getParams = parameters != null ? $"?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}" : string.Empty;
            ITikTokHttpRequest request = new TikTokHttpRequest(Constants.TIKTOK_SIGN_API, false)
                .SetQueries(new Dictionary<string, object>()
                {
                    { "client", CLIENT_NAME },
                    { "uuc", clientNum },
                    { "url", url + getParams }
                });
            HttpContent content = await request.Get();
            try
            {
                JObject jObj = JObject.Parse(await content.ReadAsStringAsync());
                string signedUrl = jObj["signedUrl"]?.Value<string>();
                string userAgent = jObj["User-Agent"]?.Value<string>();
                TikTokHttpRequest.CurrentHeaders.Remove("User-Agent");
                TikTokHttpRequest.CurrentHeaders.Add("User-Agent", userAgent);
                return signedUrl;
            }
            catch (Exception e)
            {
                throw new InsufficientPermissionException("Insufficient values have been supplied for signing. Likely due to an update. Post an issue on GitHub or in the Discord.", e);
            }
        }
        #endregion
        #endregion
    }
}