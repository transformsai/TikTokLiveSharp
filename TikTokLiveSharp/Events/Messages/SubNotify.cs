using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class SubNotify : AMessageData
    {
        public readonly Objects.User User;

        internal SubNotify(WebcastSubnotifyMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            if (msg.Sender != null)
                User = new Objects.User(msg.Sender);
        }
    }
}
