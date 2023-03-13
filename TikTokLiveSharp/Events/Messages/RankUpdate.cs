using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public class RankUpdate : AMessageData
    {
        public readonly string EventType;

        public readonly string Label;

        public readonly string Rank;

        public readonly string Color;

        internal RankUpdate(WebcastHourlyRankMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            var rankData = msg.Data.Rankings;
            EventType = rankData.Type;
            Label = rankData.Label;
            Rank = rankData.Details?.Label;
            Color = rankData.Color?.Color;
        }

        internal RankUpdate(WebcastRankUpdateMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            var rankData = msg.Data.RankData;
            EventType = rankData.Type;
            Label = rankData.Label;
            Rank = rankData.Details?.Label;
            Color = rankData.Color?.Color;
        }
    }
}
