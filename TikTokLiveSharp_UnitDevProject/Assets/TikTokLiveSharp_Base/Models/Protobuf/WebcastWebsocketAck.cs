using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastWebsocketAck : AProtoBase
    {
        [ProtoMember(2, Name = @"id")]
        public ulong Id { get; set; }

        [ProtoMember(7, Name = @"type")]
        [DefaultValue("")]
        public string Type { get; set; } = "";
    }
}
