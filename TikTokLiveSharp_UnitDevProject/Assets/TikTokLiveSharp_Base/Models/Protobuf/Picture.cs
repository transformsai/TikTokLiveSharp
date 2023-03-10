using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class Picture : AProtoBase
    {
        [ProtoMember(1, Name = @"urls")]
        public List<string> Urls { get; } = new List<string>();

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Prefix { get; set; } = "";

        [ProtoMember(3)]
        public uint Data1 { get; set; }

        [ProtoMember(4)]
        public uint Data2 { get; set; }

        [ProtoMember(5)]
        [DefaultValue("")]
        public string Color { get; set; } = "";

        [ProtoMember(6)]
        public uint Data3 { get; set; }

        [ProtoMember(7)]
        [DefaultValue("")]
        public string AdditionalUrl { get; set; } = "";

        [ProtoMember(8)]
        [DefaultValue("")]
        public string Data4 { get; set; } = "";
    }
}
