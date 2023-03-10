using ProtoBuf;
using System.Collections.Generic;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastRoomUserSeqMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public List<TopViewer> TopViewers { get; } = new List<TopViewer>();

        [ProtoMember(3)]
        public uint ViewerCount { get; set; }

        [ProtoMember(7)]
        public long ExtraData { get; set; }

        [ProtoMember(8)]
        public uint ExtraData2 { get; set; }
    }
}
