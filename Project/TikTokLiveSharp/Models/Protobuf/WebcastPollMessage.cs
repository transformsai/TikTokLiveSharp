using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastPollMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public ulong Id { get; set; }

        [ProtoMember(4)]
        public PollMessageData PollData { get; set; }

        [ProtoMember(5)]
        public PollMessageOptionContainer Options1 { get; set; }

        [ProtoMember(6)]
        public PollMessageOptionContainer Options2 { get; set; }
    }

    [ProtoContract]
    public partial class PollMessageData : AProtoBase
    {
        [ProtoMember(1)]
        public ulong TimeStamp1 { get; set; }

        [ProtoMember(2)]
        public ulong TimeStamp2 { get; set; }

        [ProtoMember(3)]
        public List<PollMessageOption> Options { get; } = new List<PollMessageOption>();

        [ProtoMember(5)]
        public User User { get; set; }
    }

    [ProtoContract]
    public partial class PollMessageOptionContainer : AProtoBase
    {
        [ProtoMember(2)]
        public List<PollMessageOption> Options { get; } = new List<PollMessageOption>();

        [ProtoMember(3)]
        public User User { get; set; }
    }

    [ProtoContract]
    public partial class PollMessageOption : AProtoBase
    {
        [ProtoMember(1)]
        public uint CurrentTotal { get; set; }

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        [ProtoMember(3)]
        public uint Data2 { get; set; }
    }
}
