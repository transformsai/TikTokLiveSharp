using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public class RankText : AMessageData
    {
        public readonly string EventType;

        public readonly string Label;

        internal RankText(WebcastRankTextMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            EventType = msg.Details.Type;
            Label = msg.Details.Label;
        }
    }
}
