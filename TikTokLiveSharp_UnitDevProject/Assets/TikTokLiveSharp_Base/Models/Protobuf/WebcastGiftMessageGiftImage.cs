using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastGiftMessageGiftImage : AProtoBase
    {
        [ProtoMember(1)]
        public List<string> PictureUrl { get; set; } = new List<string>();

        [ProtoMember(2)]
        [DefaultValue("")]
        public string UrlShortHand { get; set; } = "";

        //Examples:
        //# 5 - String - #523749
        //# 5 - String - #270603
        [ProtoMember(5)]
        [DefaultValue("")]
        public string Color { get; set; } = "";
    }
}
