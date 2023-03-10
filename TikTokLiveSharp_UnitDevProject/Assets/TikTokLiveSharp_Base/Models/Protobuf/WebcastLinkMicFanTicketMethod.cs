using ProtoBuf;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastLinkMicFanTicketMethod : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public FanTicketMethodDataContainer Data { get; set; }
    }

    [ProtoContract]
    public partial class FanTicketMethodDataContainer : AProtoBase
    {
        [ProtoMember(1)]
        public FanTicketMethodData Details { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }
    }

    [ProtoContract]
    public partial class FanTicketMethodData : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Id { get; set; }

        [ProtoMember(2)]
        public uint Data { get; set; }
    }
}