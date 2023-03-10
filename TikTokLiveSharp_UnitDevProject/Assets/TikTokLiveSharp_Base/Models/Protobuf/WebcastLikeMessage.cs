using ProtoBuf;
using TikTokLiveSharp.Models.Protobuf.Data;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastLikeMessage : AProtoBase
    {
        [ProtoMember(1)]
        public LikeMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public uint LikeCount { get; set; }

        [ProtoMember(3)]
        public ulong TotalLikes { get; set; }

        [ProtoMember(5, Name = @"user")]
        public User User { get; set; }

        // Same as in Header, but with additional Container & 1 value
        // NOT ALWAYS FILLED (The one in Header is!)
        [ProtoMember(8)]
        public LikeDataContainer LikeData { get; set; }
    }

    [ProtoContract]
    public partial class LikeDataContainer : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Data1 { get; set; }
        
        [ProtoMember(2)]
        public LikeData LikeData { get; set; }
    }
}
