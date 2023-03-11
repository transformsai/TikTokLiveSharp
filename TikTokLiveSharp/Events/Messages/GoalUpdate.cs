using System.Collections.Generic;
using System.Linq;
using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class GoalUpdate : AMessageData
    {
        /// <summary>
        /// UNCONFIRMED
        /// </summary>
        public readonly ulong GoalId;

        public readonly Objects.Picture Picture;

        public readonly string EventType;
        public readonly string Label;

        public readonly IReadOnlyList<Objects.User> Users;

        internal GoalUpdate(WebcastGoalUpdateMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Picture = new Objects.Picture(msg.Picture);
            GoalId = msg.Id1;
            EventType = msg.Data1.EventType;
            Label = msg.Details.Label;
            Users = msg.Details.Users.Select(u => new Objects.User(u.UserId, null, u.Nickname, null, new Objects.Picture(u.ProfilePicture), null, null, null, 0, 0, 0, null)).ToList();
        }
    }
}
