using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class TreasureBoxUser : AProtoBase
    {
        [ProtoMember(8, Name = @"user2")]
        public TreasureBoxUser2 User2 { get; set; }
    }
}
