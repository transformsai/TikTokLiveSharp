using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class Like : AMessageData
    {
        public readonly Objects.User Sender;

        /// <summary>
        /// Maxes out at +/- 15 per Message
        /// </summary>
        public readonly uint Count;

        /// <summary>
        /// Can be 0 for SocialMessage
        /// </summary>
        public readonly ulong TotalLikes;

        internal Like(WebcastSocialMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            if (msg.Sender != null)
                Sender = new Objects.User(msg.Sender);
            Count = 1;
            TotalLikes = 0;
        }

        internal Like(WebcastLikeMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            if (msg.Sender != null)
                Sender = new Objects.User(msg.Sender);
            Count = msg.Count;
            TotalLikes = msg.TotalLikes;
        }
    }
}
