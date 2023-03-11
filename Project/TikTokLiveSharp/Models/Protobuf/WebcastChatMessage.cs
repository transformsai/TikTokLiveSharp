using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastChatMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2, Name = @"user")]
        public User User { get; set; }

        [ProtoMember(3, Name = @"comment")]
        [DefaultValue("")]
        public string Comment { get; set; } = "";

        [ProtoMember(11)]
        public ulong Data1 { get; set; }

        [ProtoMember(12)]
        public List<User> MentionedUsers { get; set; } = new List<User>();

        [ProtoMember(13)]
        public List<ChatImage> Images { get; set; } = new List<ChatImage>();

        [ProtoMember(14)]
        [DefaultValue("")]
        public string Language { get; set; } = "";

        [ProtoMember(18)]
        public AdditionalChatData ExtraData { get; set; }

        [ProtoMember(19)]
        public ModerationData ModerationData { get; set; }
    }

    [ProtoContract]
    public partial class ModerationData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Type { get; set; } = "";

        [ProtoMember(2)]
        public ulong Value { get; set; }
    }

    [ProtoContract]
    public partial class AdditionalChatData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data10 { get; set; }

        [ProtoMember(3)]
        public uint Data2 { get; set; }

        [ProtoMember(4)]
        public uint Data3 { get; set; }

        [ProtoMember(5)]
        public uint Data4 { get; set; }
    }

    [ProtoContract]
    public partial class ChatImage : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Data1 { get; set; }

        [ProtoMember(2)]
        public Picture Picture { get; set; }
    }
}
