using System.Collections.Generic;
using System.Linq;

namespace TikTokLiveSharp.Events.MessageData.Objects
{
    public sealed class LinkMicBattleTeam
    {
        public readonly ulong TeamId;
        public readonly IReadOnlyList<Objects.User> Users;

        public LinkMicBattleTeam(ulong teamId, List<Objects.User> users)
        {
            TeamId = teamId;
            Users = users;
        }

        internal LinkMicBattleTeam(Models.Protobuf.LinkMicBattleTeam team)
        {
            TeamId = team.TeamId;
            Users = team.Users.Select(u => new Objects.User(u)).ToList();
        }
    }
}