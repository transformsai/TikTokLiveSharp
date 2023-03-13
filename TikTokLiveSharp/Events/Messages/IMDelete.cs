using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class IMDelete : AMessageData
    {
        public readonly string Data1;
        public readonly string Data2;

        internal IMDelete(WebcastImDeleteMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Data1 = msg.Data1;
            Data2 = msg.Data2;
        }
    }
}
