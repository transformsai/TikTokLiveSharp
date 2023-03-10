using System.Collections.Generic;
using TikTokLiveSharp.Models.Protobuf;

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
            Data = new List<uint>{ msg.Data2.Data1, msg.Data2.Data2, msg.Data2.Data3 };
            Timings = new List<ulong> { msg.Timestamps.TimeStamp1, msg.Timestamps.TimeStamp2, msg.Timestamps.TimeStamp3 };
        }
    }
}
