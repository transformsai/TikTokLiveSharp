using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Data;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastBarrageMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(3)]
        public uint Data1 { get; set; }

        [ProtoMember(4)]
        public Picture Picture { get; set; }

        [ProtoMember(5)]
        public BarrageMessageData1 Data2 { get; set; }

        [ProtoMember(6)]
        public uint Data3 { get; set; }

        [ProtoMember(7)]
        public Picture Picture2 { get; set; }

        [ProtoMember(8)]
        public Picture Picture3 { get; set; }

        [ProtoMember(100)]
        public BarrageMessageUserData UserData { get; set; }
    }

    [ProtoContract]
    public partial class BarrageMessageUserData : AProtoBase
    { 
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(3)]
        [DefaultValue("")]
        public string UserId { get; set; } = "";

        [ProtoMember(4)]
        public User User { get; set; }
    }

    [ProtoContract]
    public partial class BarrageMessageData1 : AProtoBase
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
        public List<AdditionalBarrageData> Data1 { get; } = new List<AdditionalBarrageData>();
    }

    [ProtoContract]
    public partial class AdditionalBarrageData : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Data1 { get; set; }

        [ProtoMember(11)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";

        [ProtoMember(21)]
        public AdditionalGiftDataUserContainer User { get; set; }
    }
}
