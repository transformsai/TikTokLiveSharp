using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public class Join : AMessageData
    {
        public readonly Objects.User User;

        /// <summary>
        /// Can be 0 for SocialMessage
        /// </summary>
        public readonly ulong TotalViewers;

        internal Join(WebcastSocialMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            User = new Objects.User(msg.User);
            TotalViewers = 0;
        }

        internal Join(WebcastMemberMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            User = new Objects.User(msg.User);
            TotalViewers = msg.TotalViewers;
        }
    }
}
