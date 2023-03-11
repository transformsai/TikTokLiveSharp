using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastLinkMicBattleGroup : AProtoBase
    {
        [ProtoMember(1, Name = @"user")]
        public LinkUser User { get; set; }
    }
}
