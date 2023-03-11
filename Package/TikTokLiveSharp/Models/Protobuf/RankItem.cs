using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class RankItem : AProtoBase
    {
        [ProtoMember(1, Name = @"colour")]
        [DefaultValue("")]
        public string Colour { get; set; } = "";

        [ProtoMember(4, Name = @"id")]
        public ulong Id { get; set; }

        [ProtoMember(6)]
        public uint Data1 { get; set; }
    }
}
