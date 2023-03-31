using ProtoBuf;
using TikTokLiveSharp.Models.Protobuf.Messages.Headers;
using TikTokLiveSharp.Models.Protobuf.Objects;

namespace TikTokLiveSharp.Models.Protobuf.Messages
{
    [ProtoContract]
    public partial class WebcastHourlyRankMessage : AProtoBase
    {
        [ProtoMember(1)]
        public MessageHeader Header { get; set; }

        [ProtoMember(2)]
        public RankContainer Data { get; set; }

        [ProtoMember(3)]
        public uint Data1 { get; set; }
    }

    [ProtoContract]
    public partial class RankContainer : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(4)]
        public Ranking Rankings { get; set; }

        [ProtoMember(6)]
        public uint Data3 { get; set; }

        [ProtoMember(7)]
        public uint Data4 { get; set; }
    }
}
