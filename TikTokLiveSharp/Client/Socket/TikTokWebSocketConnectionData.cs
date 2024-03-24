using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Client.Socket
{
    /// <summary>
    /// Data used to establish a WebSocket-Connection to the TikTok-Server.
    /// </summary>
    public readonly struct TikTokWebSocketConnectionData
    {
        /// <summary>
        /// RoomID for stream to connect to
        /// </summary>
        public readonly string RoomId;
        /// <summary>
        /// Required Cookies obtained from signing-server Headers
        /// </summary>
        public readonly string WebSocketCookies;
        /// <summary>
        /// TikTokServer-Response obtained from signing-server
        /// </summary>
        public readonly Response InitialWebcastResponse;

        /// <summary>
        /// Creates instance of TikTokWebSocketConnectionData
        /// </summary>
        /// <param name="roomId">RoomID for stream to connect to</param>
        /// <param name="cookies">Required Cookies obtained from signing-server Headers</param>
        /// <param name="initialResponse">TikTokServer-Response obtained from signing-server</param>
        public TikTokWebSocketConnectionData(string roomId, string cookies, Response initialResponse)
        {
            RoomId = roomId;
            WebSocketCookies = cookies;
            InitialWebcastResponse = initialResponse;
        }
    }
}
