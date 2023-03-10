using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf.Data
{
    [ProtoContract]
    public partial class GiftData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string DataType { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        [ProtoMember(3)]
        public RankItem RankItem { get; set; }

        [ProtoMember(4)]
        public List<AdditionalGiftData> AdditionalData { get; } = new List<AdditionalGiftData>();
    }

    [ProtoContract]
    public partial class AdditionalGiftData : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Data1 { get; set; }

        [ProtoMember(2)]
        public RankItem RankItem { get; set; }

        [ProtoMember(11)]
        public AdditionalGiftDataIntContainer Data2 { get; set; }

        [ProtoMember(21)]
        public AdditionalGiftDataUserContainer User { get; set; }

       [ProtoMember(22)]
        public AdditionalGiftData2 AdditionalGiftData2 { get; set; }
    }

    [ProtoContract]
    public partial class AdditionalGiftDataIntContainer : AProtoBase
    {
        [ProtoMember(15)]
        public long Data1 { get; set; }
    }

    [ProtoContract]
    public partial class AdditionalGiftData2 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public AdditionalGiftData2Details Details { get; set; }

        [ProtoMember(3)]
        public uint Data3 { get; set; }

        [ProtoMember(4)]
        public int Data2 { get; set; }

        [ProtoMember(5)]
        public int Data4 { get; set; }
    }

    [ProtoContract]
    public partial class AdditionalGiftData2Details : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string DataType { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string GiftName { get; set; } = "";
    }



    [ProtoContract]
    public partial class AdditionalGiftDataUserContainer : AProtoBase
    {
        [ProtoMember(1)]
        public User User { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }
    }
}
