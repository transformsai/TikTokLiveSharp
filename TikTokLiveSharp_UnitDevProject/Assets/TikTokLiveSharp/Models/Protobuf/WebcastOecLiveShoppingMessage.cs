using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastOecLiveShoppingMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(4)]
        public LiveShoppingData ShopData { get; set; }

        [ProtoMember(5)]
        public LiveShoppingTimings ShopTimings { get; set; }

        [ProtoMember(9)]
        public LiveShoppingAdditionalData AdditionalData { get; set; }
    }

    [ProtoContract]
    public partial class LiveShoppingData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Title { get; set; } = "";

        // Example:
        //# 2 - String - "$55.99"
        [ProtoMember(2)]
        [DefaultValue("")]
        public string PriceString { get; set; } = "";

        [ProtoMember(3)]
        [DefaultValue("")]
        public string ImageUrl { get; set; } = "";

        [ProtoMember(4)]
        [DefaultValue("")]
        public string ShopUrl { get; set; } = "";

        [ProtoMember(6)]
        public ulong Data1 { get; set; }

        // E.g. "Shopify"
        [ProtoMember(7)]
        [DefaultValue("")]
        public string ShopName { get; set; }

        [ProtoMember(8)]
        public ulong Data3 { get; set; }

        [ProtoMember(9)]
        [DefaultValue("")]
        public string ShopUrl2 { get; set; } = "";

        [ProtoMember(10)]
        public ulong Data4 { get; set; }

        [ProtoMember(11)]
        public ulong Data5 { get; set; }
    }

    [ProtoContract]
    public partial class LiveShoppingTimings : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Timing1 { get; set; }

        [ProtoMember(2)]
        public ulong Timing2 { get; set; }

        [ProtoMember(3)]
        public ulong Timing3 { get; set; }
    }

    [ProtoContract]
    public partial class LiveShoppingAdditionalData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Id1 { get; set; } = "";

        [ProtoMember(3)]
        [DefaultValue("")]
        public string Data1 { get; set; } = "";

        [ProtoMember(4)]
        public uint Data2 { get; set; }

        [ProtoMember(5)]
        public ulong Timing { get; set; }

        [ProtoMember(6)]
        public LiveShoppingAdditionalDataDetailsContainer Details { get; set; }
    }

    [ProtoContract]
    public partial class LiveShoppingAdditionalDataDetailsContainer : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        // Example: "ecom_purchase_soldout_1"
        [ProtoMember(2)]
        [DefaultValue("")]
        public string Label { get; set; } = "";
    }
}
