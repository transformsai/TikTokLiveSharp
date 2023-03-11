using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public class WebcastRankUpdateMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public WCRankUpdateData Rankings { get; set; }

        [ProtoMember(3)]
        public uint Data1 { get; set; }

        [ProtoMember(5)]
        public uint Data2 { get; set; }
    }

    [ProtoContract]
    public partial class WCRankUpdateData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(3)]
        public Ranking RankData { get; set; }

        [ProtoMember(4)]
        public WCRankColor RankItem { get; set; }
        
        [ProtoMember(6)]
        public ulong Data3 { get; set; }

        [ProtoMember(7)]
        public WCRankColor2 Data4 { get; set; }

        [ProtoMember(8)]
        public uint Data5 { get; set; }

        [ProtoMember(9)]
        public uint Data6 { get; set; }
    }

    [ProtoContract]
    public partial class WCRankColor : AProtoBase
    {
        [ProtoMember(4)]
        [DefaultValue("")]
        public string Color { get; set; } = "";
    }

    [ProtoContract]
    public partial class WCRankColor2 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Color { get; set; } = "";

        [ProtoMember(4)]
        public uint Data2 { get; set; }
    }
}