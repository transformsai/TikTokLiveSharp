using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class LinkMicFanTicket : AMessageData
    {
        public readonly ulong Id;
        public readonly uint Data1;
        public readonly uint Data2;

        internal LinkMicFanTicket(WebcastLinkMicFanTicketMethod msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Id = msg.Data.Details.Id;
            Data1 = msg.Data.Data1;
            Data2 = msg.Data.Details.Data;
        }
    }
}
