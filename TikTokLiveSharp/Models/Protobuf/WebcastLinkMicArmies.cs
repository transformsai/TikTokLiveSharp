using ProtoBuf;
using System.Collections.Generic;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastLinkMicArmies : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public ulong Id1 { get; set; }

        [ProtoMember(3)]
        public List<WebcastLinkMicArmiesItems> BattleItems { get; } = new List<WebcastLinkMicArmiesItems>();

        [ProtoMember(4)]
        public ulong Id2 { get; set; }

        [ProtoMember(5)]
        public ulong Timestamp1 { get; set; }

        [ProtoMember(6)]
        public ulong Timestamp2 { get; set; }

        [ProtoMember(7)]
        public int BattleStatus { get; set; }

        [ProtoMember(8)]
        public ulong Data1 { get; set; }

        [ProtoMember(9)]
        public ulong Data2 { get; set; }

        [ProtoMember(10)]
        public uint Data3 { get; set; }

        [ProtoMember(11)]
        public Picture Picture { get; set; }

        [ProtoMember(12)]
        public uint Data4 { get; set; }

        [ProtoMember(13)]
        public uint Data5 { get; set; }
    }
}
