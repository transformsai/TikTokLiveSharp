using System.Collections.Generic;
using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class DetectMessage : AMessageData
    {
        public readonly string Language;

        public readonly IReadOnlyList<uint> Data;
        public readonly IReadOnlyList<ulong> Timings;

        internal DetectMessage(WebcastMsgDetectMessage msg) : 
            base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Language = msg.Language;
            Data = new List<uint>{ (uint)msg.Data2.Data1, msg.Data2.Data2, msg.Data2.Data3 };
            Timings = new List<ulong> { msg.Timestamps.Timestamp1, msg.Timestamps.Timestamp2, msg.Timestamps.Timestamp3 };
        }
    }
}
