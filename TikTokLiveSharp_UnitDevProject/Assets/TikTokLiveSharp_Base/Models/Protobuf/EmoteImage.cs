using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class EmoteImage : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string ImageUrl { get; set; } = "";
    }
}
