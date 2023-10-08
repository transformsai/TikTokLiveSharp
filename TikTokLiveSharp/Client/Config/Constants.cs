using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TikTokLiveSharp.Client.Config
{
    /// <summary>
    /// Constant Values for TikTokLiveSharp
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Web-URL for TikTok
        /// </summary>
        public const string TIKTOK_URL_WEB = "https://www.tiktok.com/";
        /// <summary>
        /// WebCast-BaseURL for TikTok
        /// </summary>
        public const string TIKTOK_URL_WEBCAST = "https://webcast.tiktok.com/webcast/";
        /// <summary>
        /// Signing API by Isaac Kogan
        /// </summary>
        public const string TIKTOK_SIGN_API = "https://tiktok.eulerstream.com/webcast/sign_url";

        /// <summary>
        /// Default TimeOut for Connections
        /// </summary>
        public const float DEFAULT_TIMEOUT = 20f;
        /// <summary>
        /// Default Polling-Time for Socket-Connection
        /// </summary>
        public const float DEFAULT_POLLTIME = .5f;

        /// <summary>
        /// Default Settings for Client
        /// </summary>
        public static readonly ClientSettings DEFAULT_SETTINGS = new ClientSettings()
        {
            Timeout = DEFAULT_TIMEOUT,
            PollingInterval = DEFAULT_POLLTIME,
            ClientLanguage = "en-US",
            SkipRoomInfo = false,
            HandleExistingMessagesOnConnect = true,
            DownloadGiftInfo = true,
            RetryOnConnectionFailure = true,
            Proxy = null,
            SocketBufferSize = 500_000,
            PrintToConsole = true,
            LogLevel = LogLevel.Error | LogLevel.Warning,
            CheckForUnparsedData = false,
            PrintMessageData = false
        };

        /// <summary>
        /// Default Parameters for HTTP-Request
        /// </summary>
        public static readonly IReadOnlyDictionary<string, object> DEFAULT_CLIENT_PARAMS = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>()
        {
            { "aid",  1988 },
            { "app_language",  "en-US" },
            { "app_name",  "tiktok_web" },
            { "browser_language",  "en-US" },
            { "browser_name",  "Mozilla" },
            { "browser_online",  true },
            { "browser_platform",  "Win32" },
            { "browser_version",  "5.0 (Windows NT 2010.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36" },
            { "cookie_enabled",  true },
            { "cursor",  "" },
            { "internal_ext",  "" },
            { "device_platform",  "web" },
            { "focus_state",  true },
            { "from_page",  "user" },
            { "history_len",  4 },
            { "is_fullscreen",  false },
            { "is_page_visible",  true },
            { "did_rule",  3 },
            { "fetch_rule",  1 },
            { "resp_content_type",  "protobuf" },
            { "screen_height",  1152 },
            { "screen_width",  2048 },
            { "tz_name",  "Europe/London" },
            { "referer",  "https, //www.tiktok.com/" },
            { "root_referer",  "https, //www.tiktok.com/" },
            { "msToken",  ""},
            { "version_code",  180800},
            { "webcast_sdk_version",  "1.3.2" },
            { "update_version_code",  "1.3.2" }
        });

        /// <summary>
        /// Default Headers for HTTP-Request
        /// </summary>
        public static readonly IReadOnlyDictionary<string, string> DEFAULT_REQUEST_HEADERS = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()
        {
            { "Connection", "keep-alive" },
            { "Cache-Control", "no-cache" },
            { "Accept", "text/html,application/json,application/protobuf" },
            { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36" },
            { "Referer", "https://www.tiktok.com/" },
            { "Origin", "https://www.tiktok.com" },
            { "Accept-Language", "en-US,en; q=0.8" },
            { "Accept-Encoding", "gzip, deflate, br" }
        });
    }
}
