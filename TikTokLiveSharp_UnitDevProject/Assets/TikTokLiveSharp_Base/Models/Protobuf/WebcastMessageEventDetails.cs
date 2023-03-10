using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastMessageEventDetails : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string EventType { get; set; } = "";

        [ProtoMember(2, Name = @"label")]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        // Field 3 in WebcastLikeMessage
        [ProtoMember(3)]
        public RankItem Item { get; set; }

        // Field 4 in WebcastLikeMessage
        //# 1 - VarInt - 11
        //# 21 - User
        [ProtoMember(4)]
        public EventUser User { get; set; }

    }
}
