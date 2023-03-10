using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class TreasureBoxUser4 : AProtoBase
    {
        [ProtoMember(1, Name = @"user")]
        public User User { get; set; }
    }
}
