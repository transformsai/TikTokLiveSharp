using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class GiftMessage : AMessageData
    {
        public readonly Objects.Gift Gift;
        public readonly Objects.User Sender;

        public readonly string PurchaseId;
        public readonly string Receipt;

        internal GiftMessage(WebcastGiftMessage msg) : 
            base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Gift = new Objects.Gift(msg.GiftDetails);
            Sender = new Objects.User(msg.User);
            PurchaseId = msg.LogId;
            Receipt = msg.ReceiptJSON;
        }
    }
}
