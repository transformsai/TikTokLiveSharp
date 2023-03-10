using System.Collections.Generic;
using System.Linq;
using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Objects
{
    public sealed class LinkMicArmy
    {
        public sealed class Army
        {
            public readonly IReadOnlyList<Objects.User> Users;
            public readonly int Points;
            public Army(List<Objects.User> users, int points)
            {
                Users = users;
                Points = points;
            }
        }

        public readonly ulong ArmyId;
        public readonly List<Army> Armies;

        public LinkMicArmy(ulong armyId, List<Army> armies)
        {
            ArmyId = armyId;
            Armies = armies;
        }

        internal LinkMicArmy(WebcastLinkMicArmiesItems army)
        {
            ArmyId = army.HostUserId;
            Armies = army.BattleGroups.Select(g => new Army(g.Users.Select(u => new Objects.User(u)).ToList(), g.Points)).ToList();
        }
    }
}