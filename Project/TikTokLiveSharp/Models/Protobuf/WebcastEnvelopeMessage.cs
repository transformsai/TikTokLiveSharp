using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastEnvelopeMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public EnvelopeUser User { get; set; }

        [ProtoMember(3)]
        public uint Data1 { get; set; }
    }

    [ProtoContract]
    public partial class EnvelopeUser : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Id { get; set; } = "";

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        [DefaultValue("")]
        public string Username { get; set; } = "";

        [ProtoMember(11)]
        [DefaultValue("")]
        public string Id2 { get; set; } = "";
    }
}
