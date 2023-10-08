using System.Collections.Generic;
using System.Linq;
using TikTokLiveSharp.Events.Objects;

namespace TikTokLiveSharp.Events.Beta.Data
{
    public sealed class LinkMicArmy
    {
        public readonly IReadOnlyList<User> Users;
        public readonly uint Score;

        private LinkMicArmy(Models.Protobuf.UnknownObjects.Data.LinkMicArmy army)
        {
            Users = army?.Users is { Count: > 0 } ? army.Users.Select(u => (User)u).ToList().AsReadOnly() : new List<User>(0).AsReadOnly();
            Score = army?.Score ?? 0;
        }

        public static implicit operator LinkMicArmy(Models.Protobuf.UnknownObjects.Data.LinkMicArmy army) => new LinkMicArmy(army);
    }
}
