using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Messages.Headers;

namespace TikTokLiveSharp.Models.Protobuf.Messages
{
    /// <summary>
    /// Basic Text Message from Room
    /// </summary>
    [ProtoContract]
    public partial class WebcastRoomMessage : AProtoBase
    {
        [ProtoMember(1)]
        public MessageHeader Header { get; set; }

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Data { get; set; } = "";
    }
}
