using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class Subscribe : AMessageData
    {
        public readonly Objects.User NewSubscriber;

        internal Subscribe(WebcastMemberMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            NewSubscriber = new Objects.User(msg.User);
        }
    }
}
