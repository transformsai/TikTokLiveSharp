using ProtoBuf;
using System.Collections.Generic;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastLinkMicArmiesItems : AProtoBase
    {
        [ProtoMember(1)]
        public ulong HostUserId { get; set; }

        [ProtoMember(2)]
        public List<WebcastLinkMicArmiesGroup> BattleGroups { get; } = new List<WebcastLinkMicArmiesGroup>();
    }
}
