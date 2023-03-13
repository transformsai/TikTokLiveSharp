using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class Caption : AMessageData
    {
        public readonly ulong CaptionTimeStamp;

        public readonly string ISOLanguage;

        public readonly string Text;

        internal Caption(WebcastCaptionMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            CaptionTimeStamp = msg.Timestamp;
            ISOLanguage = msg.CaptionData.ISOLanguage;
            Text = msg.CaptionData.Text;
        }
    }
}
