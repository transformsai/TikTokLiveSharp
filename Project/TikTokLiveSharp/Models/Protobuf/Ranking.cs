using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class Ranking : AProtoBase
    {
        [ProtoMember(1, Name = @"type")]
        [DefaultValue("")]
        public string Type { get; set; } = "";

        [ProtoMember(2, Name = @"label")]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        [ProtoMember(3, Name = @"rank")]
        public RankItem RankColor { get; set; }

        [ProtoMember(4)]
        public AdditionalRankData Data { get; set; }
    }

    [ProtoContract]
    public partial class AdditionalRankData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(11)]
        [DefaultValue("")]
        public string Rank { get; set; } = "";
    }
}