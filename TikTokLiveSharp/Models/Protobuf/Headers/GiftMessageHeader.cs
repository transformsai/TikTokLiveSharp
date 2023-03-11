using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Data;

namespace TikTokLiveSharp.Models.Protobuf.Headers
{
    [ProtoContract]
    public partial class GiftMessageHeader : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string MessageType { get; set; } = "";

        [ProtoMember(2)]
        public ulong MessageId { get; set; }

        // Does NOT (always) match RoomID?
        [ProtoMember(3)]
        public ulong RoomId { get; set; }

        /// <summary>
        /// Unix TimeStamp Server
        /// </summary>
        [ProtoMember(4)]
        public ulong ServerTime { get; set; }

        // Example:
        //# 6 - VarInt - 1
        [ProtoMember(6)]
        public ulong Data1 { get; set; }

        // Example:
        //# 7 - String - "sf_frankvHoof: gifted the host 1 Rose"
        [ProtoMember(7)]
        [DefaultValue("")]
        public string Description { get; set; } = "";

        [ProtoMember(8)]
        public GiftHeaderData Data { get; set; }

        // Example:
        //# 9 - VarInt - 2
        [ProtoMember(9)]
        public ulong Data2 { get; set; }

        // Example:
        //# 10 - VarInt - 2
        [ProtoMember(10)]
        public ulong Data3 { get; set; }

        // Example:
        //# 11 - VarInt - 200000
        [ProtoMember(11)]
        public ulong Data4 { get; set; }

        // Example (in WebcastGiftBroadcastMessage)
        //# 17 - String - "gift_expensive"
        [ProtoMember(17)]
        [DefaultValue("")]
        public string Descr2 { get; set; } = "";

        // Example:
        //# 21 - VarInt - 20000
        [ProtoMember(21)]
        public ulong Data5 { get; set; }

        [ProtoMember(22)]
        public uint Data8 { get; set; }

        // Example:
        //# 11 - VarInt - 2
        [ProtoMember(23)]
        public ulong Data6 { get; set; }

        // Example:
        //# 11 - VarInt - 2
        [ProtoMember(24)]
        public ulong Data7 { get; set; }
    }

    [ProtoContract]
    public partial class GiftHeaderData : AProtoBase
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
        public List<GiftHeaderGiftData> GiftData { get; } = new List<GiftHeaderGiftData>();
    }

    [ProtoContract]
    public partial class GiftHeaderGiftData : AProtoBase
    {
        [ProtoMember(1)]
        public int Data1 { get; set; }

        [ProtoMember(2)]
        public RankItem RankItem { get; set; }

        [ProtoMember(11)]
        [DefaultValue("")]
        public string AdditionalGiftData { get; set; } = "";

        [ProtoMember(21)]
        public AdditionalGiftDataUserContainer UserContainer { get; set; }

        [ProtoMember(22)]
        public GiftHeaderGiftDataDetails1 Details1 { get; set; }
    }

    [ProtoContract]
    public partial class GiftHeaderGiftDataDetails1 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public AdditionalGiftData2Details GiftDetails { get; set; }

        [ProtoMember(4)]
        public int Data2 { get; set; }
    }
}
