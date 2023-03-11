using ProtoBuf;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastHourlyRankMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2, Name = @"data")]
        public RankContainer Data { get; set; }

        [ProtoMember(3)]
        public uint Data2 { get; set; }
    }
}
