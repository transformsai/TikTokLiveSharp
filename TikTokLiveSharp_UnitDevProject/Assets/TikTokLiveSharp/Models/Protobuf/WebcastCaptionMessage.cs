using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoContract]
    public partial class WebcastCaptionMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public ulong Timestamp { get; set; }

        [ProtoMember(3)]
        public uint Data0 { get; set; }

        [ProtoMember(4)]
        public CaptionData CaptionData { get; set; }
    }

    [ProtoContract]
    public partial class CaptionData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string ISOLanguage { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Text { get; set; } = "";
    }
}
