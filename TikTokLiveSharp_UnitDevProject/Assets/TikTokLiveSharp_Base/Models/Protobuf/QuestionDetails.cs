using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class QuestionDetails : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Id { get; set; }

        [ProtoMember(2)]
        [DefaultValue("")]
        public string QuestionText { get; set; } = "";

        [ProtoMember(4)]
        public ulong Timestamp { get; set; }

        [ProtoMember(5, Name = @"user")]
        public User User { get; set; }

        [ProtoMember(20)]
        public uint Data1 { get; set; }
    }
}
