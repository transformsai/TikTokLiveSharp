using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Client.Socket
{
    public struct TikTokWebSocketConnectionData
    {
        public readonly string RoomId;
        public readonly string WebSocketCookies;
        public readonly Response InitialWebcastResponse;

        public TikTokWebSocketConnectionData(string roomId, string cookies, Response initialResponse)
        {
            RoomId = roomId;
            WebSocketCookies = cookies;
            InitialWebcastResponse = initialResponse;
        }
    }
}
