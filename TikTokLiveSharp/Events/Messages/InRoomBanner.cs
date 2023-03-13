using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class InRoomBanner : AMessageData
    {
        public readonly string JSON;

        internal InRoomBanner(WebcastInRoomBannerMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            JSON = msg.Json;
        }
    }
}
