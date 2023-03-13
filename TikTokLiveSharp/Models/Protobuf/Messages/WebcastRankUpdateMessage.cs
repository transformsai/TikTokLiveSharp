using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Messages.Headers;
using TikTokLiveSharp.Models.Protobuf.Objects;

namespace TikTokLiveSharp.Models.Protobuf.Messages
{
    [ProtoContract]
    public partial class WebcastRankUpdateMessage : AProtoBase
    {
        [ProtoMember(1)]
        public MessageHeader Header { get; set; }

        [ProtoMember(2)]
        public RankUpdateData Data { get; set; }

        [ProtoMember(3)]
        public uint Data1 { get; set; }

        [ProtoMember(5)]
        public uint Data2 { get; set; }
    }


    [ProtoContract]
    public partial class RankUpdateData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(3)]
        public Ranking RankData { get; set; }

        [ProtoMember(4)]
        public RankColor Color { get; set; }

        [ProtoMember(6)]
        public ulong Data3 { get; set; }

        [ProtoMember(7)]
        public RankColor2 Color2 { get; set; }

        [ProtoMember(8)]
        public uint Data4 { get; set; }

        [ProtoMember(9)]
        public uint Data5 { get; set; }
    }

    [ProtoContract]
    public partial class RankColor : AProtoBase
    {
        [ProtoMember(4)]
        [DefaultValue("")]
        public string Color { get; set; } = "";
    }


    [ProtoContract]
    public partial class RankColor2 : AProtoBase
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
