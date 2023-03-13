using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class RoomMessage : AMessageData
    {
        public readonly Objects.User Host;
        public readonly string HostLanguage;
        public readonly string Message;

        internal RoomMessage(WebcastRoomMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Message = msg.Data;
        }

        internal RoomMessage(SystemMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Host = null;
            HostLanguage = null;
            Message = msg.Message;
        }

        internal RoomMessage(Models.Protobuf.Messages.RoomMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Host = null;
            HostLanguage = null;
            Message = msg.Message;
        }

        internal RoomMessage(WebcastLiveIntroMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            if (msg.Host != null)
                Host = new Objects.User(msg.Host);
            Message = msg.Description;
            HostLanguage = msg.Language;
        }
    }
}