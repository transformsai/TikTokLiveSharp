using ProtoBuf;
using System.Collections.Generic;
using TikTokLiveSharp.Models.Protobuf.Objects;

namespace TikTokLiveSharp.Models.Protobuf.UnknownObjects.Data
{
    [ProtoContract]
    public partial class LinkMicArmy : AProtoBase
    {
        [ProtoMember(1)]
        public List<User> Users { get; } = new List<User>();

        [ProtoMember(2)]
        public uint Score { get; }
    }
}
