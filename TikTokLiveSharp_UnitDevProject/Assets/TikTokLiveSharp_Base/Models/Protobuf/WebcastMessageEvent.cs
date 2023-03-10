using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastMessageEvent : AProtoBase
    {
        [ProtoMember(8)]
        public WebcastMessageEventDetails EventDetails { get; set; }
    }
}
