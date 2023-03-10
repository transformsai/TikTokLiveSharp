using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class Share : AMessageData
    {
        public readonly Objects.User User;
        public readonly uint Amount;

        public Share(WebcastSocialMessage msg, int amount)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            User = new Objects.User(msg.User);
            Amount = (uint)amount;
        }

        internal Share(WebcastSocialMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            User = new Objects.User(msg.User);
            Amount = 1u;
        }
    }
}
