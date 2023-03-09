using System;
using System.Threading;
using System.Threading.Tasks;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Unity.Utils;
using UnityEngine;

namespace TikTokLiveSharp.Unity
{
    public class TikTokLiveManager : MonoBehaviour
    {
        public ClientSettings settings;


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

        [Min(1)]
        [SerializeField]
        private uint texCacheSize = 2048;

        #region ConnectionSettings
        [SerializeField]
        private float connectionTimeout = 10f;
        [SerializeField]
        private float pollingInterval = .2f;
        [SerializeField]
        [Tooltip("Parse data-messages already in room when connecting")]
        private bool parseExistingRoomData = true;
        [SerializeField]
        private bool downloadGiftData = true;
        [SerializeField]
        private string clientLanguage = "en-US";

        [SerializeField]
        private bool retryOnConnectionFailure = true;
        #endregion

        private string currRoomHost;
        
        private TikTokLiveClient client;
        private TextureCache textureCache;
        private CancellationToken? cancelToken = null;

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
                client = new TikTokLiveClient(userID, TimeSpan.FromSeconds(connectionTimeout), TimeSpan.FromSeconds(pollingInterval), null, parseExistingRoomData, downloadGiftData, null, clientLanguage);
                currRoomHost = userID;
                // Set up Events for Client
                SetupEvents(client);
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
                onConnectException?.Invoke(e);
                // TODO: Fire internal exception-event
                return;
            }
            try
            {
                cancelToken = new CancellationToken();
                await client.Start(cancelToken, onConnectException, retryOnConnectionFailure);
                Debug.Log("Connected");
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
                onConnectException?.Invoke(e);
                // TODO: Fire internal exception-event
            }
        }

        private void SetupEvents(TikTokLiveClient client)
        {
           // throw new NotImplementedException();
        }

        public async Task DisconnectFromLivestream()
        {
            //throw new NotImplementedException();
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
            #endregion
        }

        protected virtual void OnDestroy()
        {
            if (Exists && ReferenceEquals(_instance, this))
            {
                #region DeInit

                #endregion
                _instance = null;
            }
        }
    }
}
