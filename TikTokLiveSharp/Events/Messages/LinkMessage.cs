using System.Collections.Generic;
using System.Linq;
using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class LinkMessage : AMessageData
    {
        public readonly string Token;

        public readonly Objects.User User;

        public readonly IReadOnlyList<Objects.User> OtherUsers;

        internal LinkMessage(WebcastLinkMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Token = msg.Token;
            if (msg.User?.User?.User != null)
                User = new Objects.User(msg.User?.User?.User);
            OtherUsers = msg.User?.OtherUsers.Select(u => u == null ? null : new Objects.User(u.User)).ToList();
        }
    }
}
