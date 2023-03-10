using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastGiftMessageGiftReceipt : AProtoBase
    {
        [ProtoMember(1)]
        public ulong AnchorId { get; set; }

        //send_gift_profit_api_start_ms
        [ProtoMember(3)]
        public ulong ProfitApiStartTimestamp { get; set; }

        //send_gift_profit_core_start_ms
        [ProtoMember(4)]
        public ulong ProfitCoreStartTimestamp { get; set; }

        //send_gift_req_start_ms
        [ProtoMember(5)]
        public ulong SendGiftRequestTimestamp { get; set; }

        //send_gift_send_message_success_ms
        [ProtoMember(6)]
        public ulong Timestamp { get; set; }

        //send_profitapi_dur
        [ProtoMember(7)]
        public ulong ProfitApiDuration { get; set; }

        [ProtoMember(8)]
        public ulong ReceiverUserId { get; set; }

        //Earliest TimeStamp in this Object.
        //Does not exist elsewhere in the message.
        //Possibly a Timestamp for the START of the request?
        [ProtoMember(9)]
        public ulong UnknownTimestamp { get; set; }

        //Example:
        //# 10 - String - Android
        [ProtoMember(10)]
        public string OperatingSystem { get; set; }

        //Example:
        //# 11 - String - 270504
        [ProtoMember(11)]
        public string Data1 { get; set; }
    }
}
