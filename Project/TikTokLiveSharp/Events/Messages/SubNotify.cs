using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class SubNotify : AMessageData
    {
        public readonly Objects.User User;

        internal SubNotify(WebcastSubNotifyMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            User = new Objects.User(msg.User);
        }
    }
}
