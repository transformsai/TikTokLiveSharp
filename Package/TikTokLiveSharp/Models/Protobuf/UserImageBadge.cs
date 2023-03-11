using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class UserImageBadge : AProtoBase
    {
        [ProtoMember(1)]
        public int DisplayType { get; set; }

        [ProtoMember(2, Name = @"image")]
        public Picture Image { get; set; }
    }
}
