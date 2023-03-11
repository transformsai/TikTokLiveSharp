using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class UserBadge : AProtoBase
    {
        [ProtoMember(2, Name = @"type")]
        [DefaultValue("")]
        public string Type { get; set; } = "";

        [ProtoMember(3, Name = @"name")]
        [DefaultValue("")]
        public string Name { get; set; } = "";
    }
}
