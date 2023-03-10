using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastGiftMessageGiftDetails : AProtoBase
    {
        [ProtoMember(1)]
        public WebcastGiftMessageGiftImage giftImage { get; set; }

        [ProtoMember(2, Name = @"describe")]
        [DefaultValue("")]
        public string Describe { get; set; } = "";

        [ProtoMember(4)]
        public int Data1 { get; set; }

        [ProtoMember(5)]
        public ulong GiftId { get; set; }

        [ProtoMember(7)]
        public int Data2 { get; set; }

        [ProtoMember(10)]
        public int Data3 { get; set; }

        [ProtoMember(11)]
        public int GiftType { get; set; }

        [ProtoMember(12)]
        public uint DiamondCount { get; set; }

        [ProtoMember(13)]
        public int Data4 { get; set; }

        [ProtoMember(14)]
        public uint Data6 { get; set; }

        [ProtoMember(15)]
        public Picture Picture { get; set; }

        [ProtoMember(16)]
        [DefaultValue("")]
        public string GiftName { get; set; } = "";

        // Field 5 (color?) is different here compared to giftImage
        // All other fields are equal
        [ProtoMember(21)]
        public WebcastGiftMessageGiftImage giftImage2 { get; set; }

        [ProtoMember(38)]
        public uint Data7 { get; set; }

        [ProtoMember(42)]
        public uint Data5 { get; set; }

        [ProtoMember(51)]
        public uint Data11 { get; set; }

        [ProtoMember(102)]
        public List<WebcastGiftMessageGiftDetailsAdditional> AdditionalDetails { get; } = new List<WebcastGiftMessageGiftDetailsAdditional>();

        [ProtoMember(104)]
        public WebcastGiftMessageDetailsMasks Masks { get; set; }

        [ProtoMember(105)]
        public uint Data10 { get; set; }

        [ProtoMember(106)]
        [DefaultValue("")]
        public string Data9 { get; set; } = "";
    }

    [ProtoContract]
    public partial class WebcastGiftMessageDetailsMasks : AProtoBase
    {
        [ProtoMember(3)]
        public WebcastGiftMessageDetailsMask Mask1 { get; set; }

        [ProtoMember(4)]
        public WebcastGiftMessageDetailsMask Mask2 { get; set; }
    }

        [ProtoContract]
    public partial class WebcastGiftMessageDetailsMask : AProtoBase
    {
        [ProtoMember(13)]
        public uint MaskValue { get; set; }
    }


    [ProtoContract]
    public partial class WebcastGiftMessageGiftDetailsAdditional : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Name { get; set; } = "";

        [ProtoMember(3)]
        public List<string> Colors { get; } = new List<string>();

        [ProtoMember(4)]
        public Picture Image { get; set; }

        [ProtoMember(5)]
        public Picture Icon { get; set; }

        [ProtoMember(6)]
        public uint Data2 { get; set; }

        [ProtoMember(7)]
        public uint Data3 { get; set; }
    }
}
