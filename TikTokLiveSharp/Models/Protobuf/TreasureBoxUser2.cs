using ProtoBuf;
using System.Collections.Generic;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class TreasureBoxUser2 : AProtoBase
    {
        [ProtoMember(4, Name = @"user3")]
        public List<TreasureBoxUser3> User3 { get; } = new List<TreasureBoxUser3>();
    }
}
