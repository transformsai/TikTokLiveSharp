using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastWebsocketMessage : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2, Name = @"id")]
        public ulong Id { get; set; }

        [ProtoMember(3)]
        public uint Data2 { get; set; }

        [ProtoMember(4)]
        public uint Data3 { get; set; }

        // CompressionType, ServerTime, etc.
        [ProtoMember(5)]
        public List<WebcastWebsocketMessageData> ConnectionData { get; } = new List<WebcastWebsocketMessageData>();

        [ProtoMember(6)]
        public WebcastWebsocketMessageAdditionalData Data4 { get; set; }

        // "hb" for heartbeat. 
        // "msg" for messages
        [ProtoMember(7, Name = @"type")]
        [DefaultValue("")]
        public string Type { get; set; } = "";

        [ProtoMember(8, Name = @"binary")]
        public byte[] Binary { get; set; }
    }

    [ProtoContract]
    public partial class WebcastWebsocketMessageData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string DataType { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Amount { get; set; } = "";
    }

    [ProtoContract]
    public partial class WebcastWebsocketMessageAdditionalData : AProtoBase
    {
        [ProtoMember(14)]
        public uint Data1 { get; set; }
    }
}
