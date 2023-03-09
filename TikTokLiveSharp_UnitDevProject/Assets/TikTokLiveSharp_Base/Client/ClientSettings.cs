using System;
using TikTokLiveSharp.Client.Proxy;
using UnityEngine;

namespace TikTokLiveSharp.Client
{
    /// <summary>
    /// Settings for TikTok-Client
    /// </summary>
    [Serializable]
    public struct ClientSettings
    {
        /// <summary>
        /// Timeout for Connections
        /// </summary>
        [Header("Settings")]
        public TimeSpan Timeout;
        /// <summary>
        /// Polling-Interval for Socket-Connection
        /// </summary>
        public TimeSpan PollingInterval;
        /// <summary>
        /// Proxy for Connection
        /// </summary>
        public RotatingProxy Proxy;
        /// <summary>
        /// ISO-Language for Client
        /// </summary>
        public string ClientLanguage;
        /// <summary>
        /// Size for Buffer for Socket-Connection
        /// </summary>
        public uint SocketBufferSize;

        #region Connecting
        /// <summary>
        /// Whether to handle Messages received from Room when Connecting
        /// </summary>
        [Header("OnConnect")]
        public bool HandleExistingMessagesOnConnect;
        /// <summary>
        /// Whether to download List of Gifts for Room when Connecting
        /// </summary>
        public bool DownloadGiftInfo;
        #endregion

        #region Debug
        /// <summary>
        /// Whether to print Logs
        /// </summary>
        [Header("Debug")]
        public bool PrintToConsole;
        /// <summary>
        /// LoggingLevel for Logs
        /// </summary>
        public LogLevel LogLevel;
        #endregion
    }

    [Serializable]
    [Flags]
    public enum LogLevel
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Information = 4,
        Verbose = 8
    }
}