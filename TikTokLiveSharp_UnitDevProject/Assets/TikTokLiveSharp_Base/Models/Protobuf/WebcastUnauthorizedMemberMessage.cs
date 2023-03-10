using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public class WebcastUnauthorizedMemberMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public UnauthorizedMemberData Detail1 { get; set; }

        [ProtoMember(4)]
        [DefaultValue("")]
        public string Data3 { get; set; } = "";

        [ProtoMember(5)]
        public UnauthorizedMemberData Detail2 { get; set; }
    }

    [ProtoContract]
    public partial class UnauthorizedMemberData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string EventType { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        [ProtoMember(3)]
        public RankItem Item { get; set; }
    }
}
