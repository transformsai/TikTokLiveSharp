using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf.Data
{
    [ProtoContract]
    public partial class MemberMessageData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string DataType { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        [ProtoMember(3)]
        public RankItem RankItem { get; set; }

        [ProtoMember(4)]
        public List<MemberMessageDetails> Details { get; } = new List<MemberMessageDetails>();
    }

    [ProtoContract]
    public partial class MemberMessageDetails : AProtoBase
    {
        [ProtoMember(1)]
        public int Data1 { get; set; }

        [ProtoMember(2)]
        public RankItem RankItem { get; set; }

        [ProtoMember(11)]
        [DefaultValue("")]
        public string Category { get; set; } = "";

        [ProtoMember(21)]
        public MemberMessageDetailUser UserContainer { get; set; }
    }

    [ProtoContract]
    public partial class MemberMessageDetailUser : AProtoBase
    {
        [ProtoMember(1)]
        public User User { get; set; }
    }
}