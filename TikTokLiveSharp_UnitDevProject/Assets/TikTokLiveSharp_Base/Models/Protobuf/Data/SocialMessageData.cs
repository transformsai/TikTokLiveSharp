using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf.Data
{
    [ProtoContract]
    public partial class SocialMessageData : AProtoBase
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
        public List<SocialMessageDetails> Details { get; } = new List<SocialMessageDetails>();
    }

    [ProtoContract]
    public partial class SocialMessageDetails : AProtoBase
    {
        [ProtoMember(1)]
        public int Data1 { get; set; }

        [ProtoMember(2)]
        public RankItem RankItem { get; set; }

        [ProtoMember(11)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";

        // Is EITHER an empty string, or a (encapsulated) User
        [ProtoMember(21)]
        [DefaultValue("")]
        public string AdditionalData { get; set; } = "";
    }
}
