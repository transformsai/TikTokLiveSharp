using JetBrains.Annotations;
using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class RoomMessage : AMessageData
    {
        [CanBeNull]
        public readonly Objects.User Host;
        [CanBeNull]
        public readonly string HostLanguage;
        public readonly string Message;

        internal RoomMessage(WebcastRoomMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Message = msg.Data1;
        }

        internal RoomMessage(SystemMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Host = null;
            HostLanguage = null;
            Message = msg.Message;
        }

        internal RoomMessage(Models.Protobuf.RoomMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Host = null;
            HostLanguage = null;
            Message = msg.Message;
        }

        internal RoomMessage(WebcastLiveIntroMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Host = new Objects.User(msg.RoomOwner);
            Message = msg.Description;
            HostLanguage = msg.Language;
        }
    }
}