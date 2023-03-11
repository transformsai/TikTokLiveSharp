using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class RankContainer : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data { get; set; }

        [ProtoMember(2)]
        public uint Data4 { get; set; }

        [ProtoMember(4, Name = @"rankings")]
        public Ranking Rankings { get; set; }

        [ProtoMember(6)]
        public uint Data2 { get; set; }

        [ProtoMember(7)]
        public uint Data3 { get; set; }
    }
}
