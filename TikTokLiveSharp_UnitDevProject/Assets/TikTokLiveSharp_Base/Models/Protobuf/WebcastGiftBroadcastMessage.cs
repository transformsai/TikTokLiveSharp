using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public class WebcastGiftBroadcastMessage : AProtoBase
    {
        [ProtoMember(1)]
        public GiftMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public ulong Data1 { get; set; }

        [ProtoMember(3)]
        public Picture Picture { get; set; }

        [ProtoMember(4)]
        public GiftBroadcastData BroadcastData { get; set; }
    }

    [ProtoContract]
    public class GiftBroadcastData : AProtoBase
    {
        [ProtoMember(1)]
        public WebcastRoomNotifyMessage RoomNotifyMessage { get; set; }

        [ProtoMember(2)]
        [DefaultValue("")]
        public string ShortUrl { get; set; } = "";

        [ProtoMember(3)]
        public uint Data1 { get; set; }

        [ProtoMember(6)]
        public GiftBroadcastImageDataContainer ImageData { get; set; }

        // Example:
        //# 9 - String - "gift_broadcast"
        [ProtoMember(9)]
        [DefaultValue("")]
        public string NotifyType { get; set; } = "";

        [ProtoMember(10)]
        public ulong Data2 { get; set; }

        [ProtoMember(11)]
        public StringContainer AdditionalData { get; set; }
    }

    [ProtoContract]
    public partial class StringContainer : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Data { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";
    }


    [ProtoContract]
    public partial class GiftBroadcastImageDataContainer : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public GiftBroadcastImageData ImageData { get; set; }
    }

    [ProtoContract]
    public partial class GiftBroadcastImageData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(3)]
        public List<string> Urls { get; } = new List<string>();

        [ProtoMember(4)]
        [DefaultValue("")]
        public string ShortUrl { get; set; } = "";
    }
}
