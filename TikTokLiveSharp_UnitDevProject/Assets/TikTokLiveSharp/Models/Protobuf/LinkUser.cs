using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class LinkUser : AProtoBase
    {
        [ProtoMember(1)]
        public ulong UserId { get; set; }

        [ProtoMember(2, Name = @"nickname")]
        [DefaultValue("")]
        public string Nickname { get; set; } = "";

        [ProtoMember(3)]
        public Picture ProfilePicture { get; set; }

        [ProtoMember(4)]
        [DefaultValue("")]
        public string UniqueId { get; set; } = "";
    }
}
