using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class RoomPinMessage : AMessageData
    {
        public readonly ulong PinTimeStamp;

        public Comment Comment;

        internal RoomPinMessage(WebcastRoomPinMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            PinTimeStamp = msg.Timestamp1;
            Comment = new Comment(msg.Data);
        }
    }
}
