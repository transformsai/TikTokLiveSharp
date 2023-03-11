using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastLinkMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public ulong Data2 { get; set; }

        [ProtoMember(4)]
        public uint Data3 { get; set; }

        [ProtoMember(20)]
        public LinkMessageUserContainer User { get; set; }

        [ProtoMember(18)]
        public LinkMessageDataContainer Data { get; set; }

        [ProtoMember(200)]
        [DefaultValue("")]
        public string Token { get; set; } = "";
    }

    [ProtoContract]
    public partial class LinkMessageUserContainer : AProtoBase
    {
        [ProtoMember(1)]
        public LinkMessageUser User1 { get; set; }

        [ProtoMember(2)]
        public List<LinkMessageUser> OtherUsers { get; } = new List<LinkMessageUser>();
    }

    [ProtoContract]
    public partial class LinkMessageUser : AProtoBase
    {
        [ProtoMember(1)]
        public User User { get; set; }

        [ProtoMember(2)]
        public ulong Timestamp1 { get; set; }

        [ProtoMember(4)]
        public uint Data1 { get; set; }

        [ProtoMember(5)]
        [DefaultValue("")]
        public string IdString { get; set; } = "";

        [ProtoMember(7)]
        public uint Data2 { get; set; }
    }

    [ProtoContract]
    public partial class LinkMessageDataContainer : AProtoBase
    {
        [ProtoMember(1)]
        public LinkMessageData Data { get; set; }
    }

    [ProtoContract]
    public partial class LinkMessageData : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Id { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public uint Data2 { get; set; }

        [ProtoMember(4)]
        public uint Data3 { get; set; }

        [ProtoMember(5)]
        public uint Data4 { get; set; }
    }
}