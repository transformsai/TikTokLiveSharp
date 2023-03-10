using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class TreasureBoxUser3 : AProtoBase
    {
        [ProtoMember(21, Name = @"user4")]
        public TreasureBoxUser4 User4 { get; set; }
    }
}
