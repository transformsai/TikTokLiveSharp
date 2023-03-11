using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastLinkLayerMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public ulong Id1 { get; set; }

        [ProtoMember(4)]
        public uint Data2 { get; set; }

        [ProtoMember(100)]
        public WebcastLinkLayerMessageDataDetails Details { get; set; }

        [ProtoMember(102)]
        public WebcastLinkLayerMessageDataContainer1 DataContainer1 { get; set; }

        [ProtoMember(110)]
        public WebcastLinkLayerMessageIDContainer IDContainer { get; set; }

        [ProtoMember(111)]
        public WebcastLinkLayerMessageIDContainer IDContainer2 { get; set; }
    }

    [ProtoContract]
    public partial class WebcastLinkLayerMessageDataContainer1 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public WebcastLinkLayerMessageDataDetails1 Details1 { get; set; }
    }

    [ProtoContract]
    public partial class WebcastLinkLayerMessageDataDetails1 : AProtoBase
    {
        [ProtoMember(2)]
        public WebcastLinkLayerMessageDataDetails Data1 { get; set; }

        [ProtoMember(3)]
        public List<WebcastLinkLayerMessageDataDetails> Data2 { get; } = new List<WebcastLinkLayerMessageDataDetails>();
    }

    [ProtoContract]
    public partial class WebcastLinkLayerMessageDataDetails : AProtoBase
    {
        [ProtoMember(1)]
        public WebcastLinkLayerMessageIDs Ids { get; set; }

        [ProtoMember(2)]
        [DefaultValue("")]
        public string IdString { get; set; } = "";

        [ProtoMember(3)]
        public LinkLayerStringContainer ExtraString { get; set; }

        [ProtoMember(4)]
        public ulong Id2 { get; set; }

        [ProtoMember(5)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";
    }

    [ProtoContract]
    public partial class WebcastLinkLayerMessageIDContainer : AProtoBase
    {
        [ProtoMember(1)]
        public WebcastLinkLayerMessageIDs Ids { get; set; }

        [ProtoMember(2)]
        public uint Data { get; set; }
    }


    [ProtoContract]
    public partial class WebcastLinkLayerMessageIDs : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Id1 { get; set; }

        [ProtoMember(2)]
        public ulong Id2 { get; set; }
    }

    [ProtoContract]
    public partial class WebcastLinkLayerMessageDataContainer : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public WebcastLinkLayerMessageData Data { get; set; }
    }

    [ProtoContract]
    public partial class WebcastLinkLayerMessageData : AProtoBase
    {
        [ProtoMember(2)]
        public WebcastLinkLayerMessageDataDetail Data { get; set; }

        [ProtoMember(3)]
        public List<WebcastLinkLayerMessageDataDetail> AdditionalData { get; } = new List<WebcastLinkLayerMessageDataDetail>();

    }

    [ProtoContract]
    public partial class WebcastLinkLayerMessageDataDetail : AProtoBase
    {
        [ProtoMember(1)]
        public WebcastLinkLayerMessageDataDetailIds Data1 { get; set; }

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";

        [ProtoMember(3)]
        public LinkLayerStringContainer Data3 { get; set; }

        [ProtoMember(4)]
        public ulong Id1 { get; set; }

        [ProtoMember(5)]
        [DefaultValue("")]
        public string Data4 { get; set; } = "";
    }

    [ProtoContract]
    public partial class WebcastLinkLayerMessageDataDetailIds : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Id1 { get; set; }

        [ProtoMember(2)]
        public ulong Id2 { get; set; }
    }

    [ProtoContract]
    public partial class LinkLayerStringContainer : AProtoBase
    {
        [ProtoMember(2)]
        [DefaultValue("")]
        public string Data { get; set; } = "";
    }
}
