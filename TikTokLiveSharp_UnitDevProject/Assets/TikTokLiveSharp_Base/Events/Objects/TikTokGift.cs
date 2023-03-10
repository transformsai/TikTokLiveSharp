using TikTokLiveSharp.Models.Protobuf;
using TikTokLiveSharp.Unity.Utils;

namespace TikTokLiveSharp.Events.MessageData.Objects
{
    public class TikTokGift
    {
        public delegate void TikTokGiftEventHandler<TEventArgs>(TikTokGift gift, TEventArgs args);

        public event TikTokGiftEventHandler<uint> OnAmountChanged;
        public event TikTokGiftEventHandler<uint> OnStreakFinished;

        public readonly Gift Gift;

        public readonly User Sender;

        public uint Amount { get; protected set; }
        public bool StreakFinished { get; protected set; }


        public TikTokGift(WebcastGiftMessage message)
        {
            Gift = new Gift(message.GiftDetails);
            Amount = message.Amount;
            StreakFinished = message.RepeatEnd;
        }

        internal virtual void FinishStreak()
        {
            StreakFinished = true;
#if UNITY
            Dispatcher.RunOnMainThread(() => {
#endif
                OnStreakFinished?.Invoke(this, Amount);
#if UNITY
            });
#endif
        }

        internal void UpdateGiftAmount(uint amount)
        {
            Amount = amount;
#if UNITY
            Dispatcher.RunOnMainThread(() => {
#endif
                OnAmountChanged?.Invoke(this, amount);
#if UNITY
            });
#endif
        }
    }
}