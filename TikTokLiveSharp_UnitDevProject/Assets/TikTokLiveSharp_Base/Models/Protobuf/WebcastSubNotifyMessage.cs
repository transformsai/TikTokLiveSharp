using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastSubNotifyMessage : AProtoBase
    {
        [ProtoMember(1)]
        public SocialMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public User User { get; set; }

        [ProtoMember(4)]
        public uint Data1 { get; set; }

        [ProtoMember(5)]
        public uint Data2 { get; set; }

        [ProtoMember(6)]
        public uint Data3 { get; set; }

        [ProtoMember(8)]
        public uint Data4 { get; set; }
    }
}
