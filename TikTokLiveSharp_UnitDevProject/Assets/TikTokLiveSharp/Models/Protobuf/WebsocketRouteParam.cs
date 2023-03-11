using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebsocketRouteParam : AProtoBase
    {
        [ProtoMember(1, Name = @"name")]
        [DefaultValue("")]
        public string Name { get; set; } = "";

        [ProtoMember(2, Name = @"value")]
        [DefaultValue("")]
        public string Value { get; set; } = "";
    }
}
