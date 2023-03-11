using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf.Data
{
    [ProtoContract]
    public partial class IntroMessageData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(21)]
        public IntroMessageDetails Details { get; set; }
    }

    [ProtoContract]
    public partial class IntroMessageDetails : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";

        [ProtoMember(3)]
        [DefaultValue("")]
        public string Data3 { get; set; } = "";
    }
}
