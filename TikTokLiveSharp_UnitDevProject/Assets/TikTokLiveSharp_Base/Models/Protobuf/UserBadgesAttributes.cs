using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class UserBadgesAttributes : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(3)]
        public uint Data3 { get; set; }

        [ProtoMember(4)]
        public uint Data4 { get; set; }

        [ProtoMember(10)]
        [DefaultValue("")]
        public string ShortURL { get; set; } = "";

        [ProtoMember(11)]
        public uint Data5 { get; set; }

        [ProtoMember(12)]
        public UserBadgeAttributesData1 ExtraData1 { get; set; }

        [ProtoMember(20)]
        public List<UserImageBadge> ImageBadges { get; } = new List<UserImageBadge>();

        [ProtoMember(21, Name = @"badges")]
        public List<UserBadge> Badges { get; } = new List<UserBadge>();

        [ProtoMember(23)]
        public UserBadgeAttributesData2 ExtraData2 { get; set; }
    }

    [ProtoContract]
    public partial class UserBadgeAttributesData1 : AProtoBase
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
        public string Data3 { get; set; } = "";
    }


    [ProtoContract]
    public partial class UserBadgeAttributesData2 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public Picture Images { get; set; }

        [ProtoMember(4)]
        [DefaultValue("")]
        public string Data { get; set; } = "";

        [ProtoMember(5)]
        public UserBadgeAttributesData2Details1 Details1 { get; set; }

        [ProtoMember(6)]
        [DefaultValue("")]
        public string Data3 { get; set; } = "";

        [ProtoMember(11)]
        public UserBadgeAttributesData2Details2 Details2 { get; set; }

        [ProtoMember(12)]
        public UserBadgeAttributesData2Details2 Details3 { get; set; }

        [ProtoMember(15)]
        public uint Data8 { get; set; }

        [ProtoMember(16)]
        public uint Data9 { get; set; }
    }

    [ProtoContract]
    public partial class UserBadgeAttributesData2Details1 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(3)]
        public uint Data3 { get; set; }

        [ProtoMember(4)]
        public uint Data4 { get; set; }

        [ProtoMember(5)]
        public uint Data5 { get; set; }

        [ProtoMember(6)]
        public uint Data6 { get; set; }

        [ProtoMember(7)]
        public uint Data7 { get; set; }

        [ProtoMember(8)]
        public uint Data8 { get; set; }

        [ProtoMember(9)]
        public uint Data9 { get; set; }
    }

    [ProtoContract]
    public partial class UserBadgeAttributesData2Details2 : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string String1 { get; set; }

        [ProtoMember(2)]
        [DefaultValue("")]
        public string String2 { get; set; }
    }
}
