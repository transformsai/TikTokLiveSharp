using System.Collections.Generic;
using System.Linq;
using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class Comment : AMessageData
    {
        public readonly Objects.User User;

        public readonly string Text;

        public readonly string Language;

        public readonly IReadOnlyList<Objects.User> MentionedUsers;

        public readonly IReadOnlyList<Objects.Picture> Pictures;

        internal Comment(PinMessageData1 data) 
            : base(data.Details.RoomId, data.Details.Data, data.Details.TimeStamp1)
        {
            User = new Objects.User(data.User);
            Text = data.Message;
            Language = data.Language;
        }

        internal Comment(WebcastChatMessage msg) : 
            base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            User = new Objects.User(msg.User);
            Text = msg.Comment;
            Language = msg.Language;
            if (msg.MentionedUsers != null && msg.MentionedUsers.Count > 0)
                MentionedUsers = new List<Objects.User>(msg.MentionedUsers.Select(u => new Objects.User(u)));
            if (msg.Images != null && msg.Images.Count > 0)
                Pictures = new List<Objects.Picture>(msg.Images.Select(p => new Objects.Picture(p.Picture)));
        }
    }
}
