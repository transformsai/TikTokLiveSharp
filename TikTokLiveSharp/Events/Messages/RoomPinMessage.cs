using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class RoomPinMessage : AMessageData
    {
        public readonly ulong PinTimeStamp;

        public Comment Comment;

        internal RoomPinMessage(WebcastRoomPinMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            PinTimeStamp = msg.Timestamp;
            Comment = new Comment(msg.PinData1);
        }
    }
}
