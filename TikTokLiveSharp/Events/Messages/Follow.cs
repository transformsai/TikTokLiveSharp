using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class Follow : AMessageData
    {
        public readonly Objects.User NewFollower;
        public readonly ulong TotalFollowers;

        internal Follow(WebcastSocialMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            NewFollower = new Objects.User(msg.User);
            TotalFollowers = msg.TotalFollowers;
        }
    }
}
