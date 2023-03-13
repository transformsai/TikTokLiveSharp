using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class Subscribe : AMessageData
    {
        public readonly Objects.User NewSubscriber;

        internal Subscribe(WebcastMemberMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            if (msg.Sender != null)
                NewSubscriber = new Objects.User(msg.Sender);
        }
    }
}
