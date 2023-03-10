using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class User : AProtoBase
    {
        /// <summary>
        /// Integer-ID for User
        /// </summary>
        [ProtoMember(1)]
        public ulong UserId { get; set; }

        // Unknown Data
        [ProtoMember(2)]
        public uint Data0 { get; set; }

        [ProtoMember(3, Name = @"nickname")]
        [DefaultValue("")]
        public string Nickname { get; set; } = "";

        [ProtoMember(5)]
        [DefaultValue("")]
        public string UserDescription { get; set; } = "";

        [ProtoMember(9)]
        public Picture ProfilePicture { get; set; }

        [ProtoMember(10)]
        public Picture Picture720 { get; set; }

        [ProtoMember(11)]
        public Picture Picture1080 { get; set; }

        // Unknown Data
        [ProtoMember(15)]
        public uint Data1 { get; set; }

        // Unknown Data
        [ProtoMember(16)]
        public ulong Data2 { get; set; }

        [ProtoMember(21)]
        public List<Picture> AdditionalPictures { get; set; }

        [ProtoMember(22)]
        public UserFollowerData FollowerData { get; set; }

        [ProtoMember(23)]
        [DefaultValue("")]
        public string UserString1 { get; set; } = "";

        [ProtoMember(25)]
        public UserRank UserRank1 { get; set; }

        [ProtoMember(32)]
        [DefaultValue("")]
        public string UserString2 { get; set; } = "";

        // Unknown Data
        [ProtoMember(37)]
        public ulong Data3 { get; set; }

        [ProtoMember(38)]
        [DefaultValue("")]
        public string UniqueId { get; set; } = "";

        // Unknown Data
        [ProtoMember(46)]
        [DefaultValue("")]
        public string Data4 { get; set; } = "";

        [ProtoMember(61)]
        public UserRank UserRank2 { get; set; }


        [ProtoMember(64, Name = @"badges")]
        public List<UserBadgesAttributes> Badges { get; } = new List<UserBadgesAttributes>();

        [ProtoMember(1028)]
        [DefaultValue("")]
        public string UserIdString { get; set; }

        public override string ToString()
        {
            return UniqueId;
        }
    }

    [ProtoContract]
    public partial class UserRank : AProtoBase
    {
        [ProtoMember(1)]
        public UserRankImageData Img1 { get; set; }

        [ProtoMember(3)]
        [DefaultValue("")]
        public string EventType { get; set; } = "";

        [ProtoMember(4)]
        public UserRankImageData Img2 { get; set; }

        [ProtoMember(5)]
        public UserRankIdData Id1 { get; set; }

        [ProtoMember(6)]
        public UserRankIdData Id2 { get; set; }
    }

    [ProtoContract]
    public partial class UserRankImageData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Url { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string ShortUrl { get; set; } = "";

        [ProtoMember(3)]
        public uint Data1 { get; set; }

        [ProtoMember(4)]
        public uint Data2 { get; set; }
    }

    [ProtoContract]
    public partial class UserRankIdData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Data1 { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Id1 { get; set; } = "";

        [ProtoMember(3)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";

        [ProtoMember(4)]
        [DefaultValue("")]
        public string Id2 { get; set; } = "";
    }
}
