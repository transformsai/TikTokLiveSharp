using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastGiftMessage : AProtoBase
    {
        [ProtoMember(1)]
        public GiftMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public ulong GiftId { get; set; }

        [ProtoMember(3)]
        public int Data1 { get; set; }

        [ProtoMember(4)]
        public int Data2 { get; set; }

        [ProtoMember(5)]
        public int RepeatCount { get; set; }

        [ProtoMember(6)]
        public uint Amount { get; set; }

        // Sender
        [ProtoMember(7)]
        public User User { get; set; }

        // Receiver? (live_gift_send_message_to_anyuser) (Host sent a gift to a viewer?)
        [ProtoMember(8)]
        public User User2 { get; set; }

        /// <summary>
        /// Is this the final message? (Messages are sent until the User stops sending gifts. 
        /// Then the last message is sent once more, but with RepeatEnd set to true)
        /// </summary>
        [ProtoMember(9)]
        public bool RepeatEnd { get; set; }

        [ProtoMember(11)]
        public ulong ServerTime { get; set; }

        [ProtoMember(13)]
        public int Data4 { get; set; }

        // Example:
        //# 1 - String - "������������������2�"
        //# 2 - VarInt - 1
        //# 3 - VarInt - 3
        [ProtoMember(14)]
        public AdditionalGiftData1 Data5 { get; set; }

        [ProtoMember(15)]
        public WebcastGiftMessageGiftDetails GiftDetails { get; set; }

        // Example:
        //# "202301050654360101042172152C002035"
        // i.e.: yyyyMMDD
        [ProtoMember(16)]
        public string LogId { get; set; }

        [ProtoMember(17)]
        public int Data51 { get; set; }

        [ProtoMember(19)]
        public string Data6 { get; set; }

        // Example:
        //# 22 - String - "{"anchor_id":7174972564270924826,"from_idc":"useast2a","from_user_id":7172434342369657857,"gift_id":7160,"gift_type":1,"log_id":"202301050654360101042172152C002035","msg_id":7185056588544477979,"profitapi_message_dur":1053,"repeat_count":1,"repeat_end":0,"room_id":7184721971182406426,"send_gift_profit_api_start_ms":1672901676997,"send_gift_profit_core_start_ms":1672901677220,"send_gift_req_start_ms":1672901676847,"send_gift_send_message_success_ms":1672901678050,"send_profitapi_dur":150,"to_user_id":7174972564270924826}"
        [ProtoMember(22)]
        public string ReceiptJSON { get; set; }

        [ProtoMember(23)]
        public WebcastGiftMessageGiftReceipt Receipt { get; set; }

        [ProtoMember(24)]
        public int Data7 { get; set; }

        [ProtoMember(25)]
        public uint Data99 { get; set; }

        [ProtoMember(26)]
        public GiftDetails1 FirstGift1 { get; set; }

        [ProtoMember(27)]
        public GiftDetails1 FirstGift2 { get; set; }

        [ProtoMember(31)]
        public AdditionalGiftData3 Data9 { get; set; }

        [ProtoMember(32)]
        public AdditionalGiftData2 Data8 { get; set; }
    }


    [ProtoContract]
    public partial class AdditionalGiftData1 : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Data1 { get; set; } = "";

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(3)]
        public uint Data3 { get; set; }
    }

    [ProtoContract]
    public partial class AdditionalGiftData2 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(3)]
        public uint Data3 { get; set; }

        [ProtoMember(4)]
        public uint Data4 { get; set; }

        [ProtoMember(5)]
        public uint Data5 { get; set; }
    }



    [ProtoContract]
    public partial class AdditionalGiftData3 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public uint Data2 { get; set; }

        [ProtoMember(4)]
        public List<string> Data3 { get; } = new List<string>();
    }

    [ProtoContract]
    public partial class GiftDetails1 : AProtoBase
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
        public List<GiftDetails1Data> Data1 { get; } = new List<GiftDetails1Data>();
    }

    [ProtoContract]
    public partial class GiftDetails1Data : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public RankItem RankItem { get; set; }

        [ProtoMember(11)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";

        [ProtoMember(21)]
        public WebcastRankTextMessageUserContainer User { get; set; }

        [ProtoMember(22)]
        public GiftDetails1DataDetails1 GiftData { get; set; }
    }

    [ProtoContract]
    public partial class GiftDetails1DataDetails1 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public GiftDetailsDoubleStringContainer Strings { get; set; }
    }

    [ProtoContract]
    public partial class GiftDetailsDoubleStringContainer : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Data1 { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Data2 { get; set; } = "";
    }
}
