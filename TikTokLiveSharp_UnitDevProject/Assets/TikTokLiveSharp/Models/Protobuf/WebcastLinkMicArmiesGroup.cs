using ProtoBuf;
using System.Collections.Generic;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastLinkMicArmiesGroup : AProtoBase
    {
        [ProtoMember(1, Name = @"users")]
        public List<User> Users { get; } = new List<User>();

        [ProtoMember(2, Name = @"points")]
        public int Points { get; set; }
    }
}
