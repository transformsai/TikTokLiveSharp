using System.Linq;
using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class RoomViewerData : AMessageData
    {
        public readonly uint ViewerCount;
        public readonly Objects.TopViewer[] TopViewers;        

        internal RoomViewerData(WebcastRoomUserSeqMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            ViewerCount = msg.ViewerCount;
            TopViewers = msg.TopViewers.Select(t => new Objects.TopViewer(t)).OrderBy(t => t.Rank).ToArray();
        }
    }
}