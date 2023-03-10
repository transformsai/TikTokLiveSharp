using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Objects
{
    public sealed class Gift
    {
        public readonly ulong Id;
        public readonly string Name;
        public readonly string Description;
        public readonly uint DiamondCost;

        public readonly int Type;

        public readonly Picture Picture;

        internal Gift(WebcastGiftMessageGiftDetails gift)
        {
            Id = gift.GiftId;
            Name = gift.GiftName;
            Description = gift.Describe;
            DiamondCost = gift.DiamondCount;
            Type = gift.GiftType;
            Picture = new Picture(gift.giftImage.PictureUrl);
        }
    }
}
