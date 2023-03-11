using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class GiftMessage : AMessageData
    {
        public readonly Objects.Gift Gift;
        public readonly Objects.User Sender;

        public readonly string PurchaseId;
        public readonly string Receipt;

        public readonly uint Amount;
        public readonly bool StreakFinished;
        public readonly uint StreakIndex;

        internal GiftMessage(WebcastGiftMessage msg) : 
            base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Gift = new Objects.Gift(msg.GiftDetails);
            Sender = new Objects.User(msg.User);
            PurchaseId = msg.LogId;
            Receipt = msg.ReceiptJSON;
            Amount = msg.Amount;
            StreakFinished = msg.RepeatEnd;
            StreakIndex = msg.RepeatCount;
        }
    }
}
