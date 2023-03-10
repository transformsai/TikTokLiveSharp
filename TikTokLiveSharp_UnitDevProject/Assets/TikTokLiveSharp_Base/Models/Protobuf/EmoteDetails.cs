using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class EmoteDetails : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string EmoteId { get; set; } = "";

        [ProtoMember(2, Name = @"image")]
        public EmoteImage Image { get; set; }
    }
}
