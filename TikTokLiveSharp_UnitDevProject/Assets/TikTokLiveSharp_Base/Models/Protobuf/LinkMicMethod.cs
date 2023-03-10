using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public class LinkMicMethod : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(8)]
        public ulong RTC_ChannelId { get; set; }

        // Either Data3 or Data4 is probably RTC_Vendor
        [ProtoMember(9)]
        public uint Data3 { get; set; }

        [ProtoMember(10)]
        public uint Data4 { get; set; }

        [ProtoMember(11)]
        public uint Data5 { get; set; }

        [ProtoMember(13)]
        public ulong Data6 { get; set; }

        [ProtoMember(24)]
        public ulong Id2 { get; set; }

        [ProtoMember(28)]
        public ulong Data7 { get; set; }

        // Either Data9 or Data8 is probably RTC_MixType
        [ProtoMember(29)]
        public uint Data8 { get; set; }

        [ProtoMember(31)]
        [DefaultValue("")]
        public string DataString1 { get; set; } = "";

        [ProtoMember(32)]
        [DefaultValue("")]
        public string JSONDataRTC { get; set; } = "";

        [ProtoMember(37)]
        [DefaultValue("")]
        public string RTC_UserId { get; set; } = "";

        // Either Data9 or Data8 is probably RTC_MixType
        [ProtoMember(43)]
        public uint Data9 { get; set; }

        [ProtoMember(51)]
        public uint Data10 { get; set; }
    }
}
