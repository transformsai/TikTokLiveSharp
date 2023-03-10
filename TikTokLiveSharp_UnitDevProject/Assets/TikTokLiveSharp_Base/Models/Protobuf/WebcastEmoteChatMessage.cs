using ProtoBuf;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastEmoteChatMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2, Name = @"user")]
        public User User { get; set; }

        [ProtoMember(3, Name = @"emote")]
        public EmoteDetails Emote { get; set; }
    }
}
