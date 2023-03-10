using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class GiftBroadcast : AMessageData
    {
        public readonly Objects.Picture Picture;

        public readonly string ShortURL;

        public readonly string NotifyEventType;
        public readonly string NotifyLabel;
        public readonly string NotifyType;

        internal GiftBroadcast(WebcastGiftBroadcastMessage msg) : 
            base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Picture = new Objects.Picture(msg.Picture);
            var data = msg.BroadcastData;
            ShortURL = data.ShortUrl;
            NotifyEventType = data.RoomNotifyMessage.NotifyData.EventType;
            NotifyLabel = data.RoomNotifyMessage.NotifyData.Label;
            NotifyType = data.NotifyType;
        }
    }
}
