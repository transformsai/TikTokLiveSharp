using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    /// <summary>
    /// Response from WebAPI
    /// <para>
    /// Data structure from im/fetch/ response
    /// </para>
    /// </summary>
    [ProtoContract]
    public partial class WebcastResponse : AProtoBase
    {
        [ProtoMember(1, Name = @"messages")]
        public List<Message> Messages { get; } = new List<Message>();

        [ProtoMember(2, Name = @"cursor")]
        [DefaultValue("")]
        public string Cursor { get; set; } = "";

        [ProtoMember(3)]
        public int FetchInterval { get; set; }

        [ProtoMember(4)]
        public ulong ServerTimestamp { get; set; }

        [ProtoMember(5)]
        [DefaultValue("")]
        public string AckIds { get; set; } = "";

        [ProtoMember(6)]
        public int FetchType { get; set; }

        [ProtoMember(7)]
        public List<WebsocketRouteParam> SocketParams { get; } = new List<WebsocketRouteParam>();

        [ProtoMember(8)]
        public int HeartBeatDuration { get; set; }

        [ProtoMember(9)]
        public bool NeedsAck { get; set; }

        [ProtoMember(10)]
        [DefaultValue("")]
        public string SocketUrl { get; set; } = "";
    }
}
