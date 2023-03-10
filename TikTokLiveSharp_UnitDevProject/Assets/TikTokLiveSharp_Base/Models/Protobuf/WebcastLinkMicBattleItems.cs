using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastLinkMicBattleItems : AProtoBase
    {
        [ProtoMember(2)]
        public WebcastLinkMicBattleGroup BattleGroup { get; set; }
    }
}
