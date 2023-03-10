using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace TikTokLiveSharp.Events.MessageData.Objects
{
    public sealed class User
    {
        public readonly ulong UserId;

        public readonly string UniqueId;

        public readonly string NickName;

        public readonly string Description;

        public readonly Picture ProfilePicture;

        [CanBeNull]
        public readonly Picture Picture720;

        [CanBeNull]
        public readonly Picture Picture1080;

        [CanBeNull]
        public readonly List<Picture> AdditionalPictures;

        public readonly uint Following;
        public readonly uint Followers;
        public readonly int FollowsHost;

        public readonly List<Badge> Badges;

        public User(ulong userId, string uniqueId, string nickName, string description, Picture profilePicture, Picture picture720, Picture picture1080, List<Picture> additionalPictures, uint following, uint followers, int followsHost, List<Badge> badges)
        {
            UserId = userId;
            UniqueId = uniqueId;
            NickName = nickName;
            Description = description;
            ProfilePicture = profilePicture;
            Picture720 = picture720;
            Picture1080 = picture1080;
            AdditionalPictures = additionalPictures;
            Following = following;
            Followers = followers;
            FollowsHost = followsHost;
            Badges = badges;
        }

        internal User(Models.Protobuf.User user)
        {
            UserId = user.UserId;
            UniqueId = user.UniqueId;
            NickName = user.Nickname;
            Description = user.UserDescription;
            ProfilePicture = new Picture(user.ProfilePicture);
            Picture720 = user.Picture720 != null ? new Picture(user.Picture720) : null;
            Picture1080 = user.Picture1080 != null ? new Picture(user.Picture1080) : null;
            Following = user.FollowerData?.Following ?? 0;
            Followers = user.FollowerData?.Followers ?? 0;
            FollowsHost = user.FollowerData?.FollowsHost ?? 0;
            if (user.Badges != null && user.Badges.Count > 0)
                Badges = new List<Badge>(user.Badges.Select(b => new Badge(b)));
        }
    }
}