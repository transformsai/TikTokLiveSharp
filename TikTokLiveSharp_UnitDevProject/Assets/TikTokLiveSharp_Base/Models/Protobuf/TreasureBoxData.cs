using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class TreasureBoxData : AProtoBase
    {
        [ProtoMember(5, Name = @"coins")]
        public uint Coins { get; set; }

        [ProtoMember(6)]
        public uint CanOpen { get; set; }

        [ProtoMember(7, Name = @"timestamp")]
        public ulong Timestamp { get; set; }
    }
}
