using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastMsgDetectMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public MsgDetectData1 Data2 { get; set; }

        [ProtoMember(4)]
        public MsgDetectData2 Timestamps { get; set; }

        [ProtoMember(6)]
        [DefaultValue("")]
        public string Language { get; set; } = "";
    }

    [ProtoContract]
    public partial class MsgDetectData1 : AProtoBase
    { 
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(4)]
        public uint Data3 { get; set; }
    }


    [ProtoContract]
    public partial class MsgDetectData2 : AProtoBase
    {
        [ProtoMember(1)]
        public ulong TimeStamp1 { get; set; }

        [ProtoMember(2)]
        public ulong TimeStamp2 { get; set; }

        [ProtoMember(3)]
        public ulong TimeStamp3 { get; set; }
    }
}
