using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastRankTextMessage : AProtoBase
    {
        [ProtoMember(1)]
        public WebcastRankTextMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public uint Data2 { get; set; }

        [ProtoMember(4)]
        public uint Data3 { get; set; }

        [ProtoMember(5)]
        public WebcastRankTextMessageDetailsSmall Details1 { get; set; }

        [ProtoMember(6)]
        public WebcastRankTextMessageDetails Details { get; set; }

        [ProtoMember(7)]
        public ulong Id1 { get; set; }
    }

    [ProtoContract]
    public partial class WebcastRankTextMessageDetailsSmall : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string EventType { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        [ProtoMember(3)]
        public RankItem RankItem { get; set; }

        [ProtoMember(4)]
        public WebcastRankTextMessageDetailsSmallData Details { get; set; }
    }

    [ProtoContract]
    public partial class WebcastRankTextMessageDetailsSmallData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(11)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";
    }

    [ProtoContract]
    public partial class WebcastRankTextMessageDetails : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string EventType { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        [ProtoMember(3)]
        public RankItem RankItem { get; set; }

        [ProtoMember(4)]
        public List<WebcastRankTextMessageDetailsData2> AdditionalData { get; set; }
    }

    [ProtoContract]
    public partial class WebcastRankTextMessageDetailsData2 : AProtoBase
    { 
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(11)]
        [DefaultValue("")]
        public string Data11 { get; set; } = "";

        [ProtoMember(21)]
        public WebcastRankTextMessageUserContainer User { get; set; }
    }

    [ProtoContract]
    public partial class WebcastRankTextMessageDetailsData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(11)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";

        [ProtoMember(21)]
        public WebcastRankTextMessageUserContainer UserContainer { get; set; }
    }

    [ProtoContract]
    public partial class WebcastRankTextMessageUserContainer : AProtoBase
    {
        [ProtoMember(1)]
        public User User { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }
    }
}
