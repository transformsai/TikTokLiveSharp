using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    /// <summary>
    /// Follow/Share Events
    /// </summary>
    [ProtoContract]
    public partial class WebcastSocialMessage : AProtoBase
    {
        [ProtoMember(1)]
        public SocialMessageHeader Header { get; set; }

        [ProtoMember(2, Name = @"user")]
        [DefaultValue(null)]
        public User User { get; set; } = null;

        // Exists on 
        // - Share
        [ProtoMember(3)]
        public ulong Data0 { get; set; }

        [ProtoMember(4)]
        public ulong Data1 { get; set; }

        [ProtoMember(5)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";

        // Exists on 
        // - Follow
        [ProtoMember(6)]
        public ulong TotalFollowers { get; set; }

        // Exists on
        // - Share
        [ProtoMember(8)]
        public ulong Data4 { get; set; }
    }
}
