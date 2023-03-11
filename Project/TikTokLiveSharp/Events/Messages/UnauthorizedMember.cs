using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class UnauthorizedMember : AMessageData
    {
        public readonly string Data;

        public readonly (string, string) Event;

        public readonly (string, string) Underlying;

        internal UnauthorizedMember(WebcastUnauthorizedMemberMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Data = msg.Data3;
            Event = (msg.Detail1.EventType, msg.Detail1.Label);
            Underlying = (msg.Detail2.EventType, msg.Detail2.Label);
        }
    }
}
