using System.Collections.Generic;
using System.Linq;
using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class LinkMicArmies : AMessageData
    {
        /// <summary>
        /// UNCONFIRMED. Could be another type of Id
        /// </summary>
        public readonly ulong BattleId;

        public readonly int BattleStatus;

        public readonly Objects.Picture Picture;

        public readonly IReadOnlyList<Objects.LinkMicArmy> Armies;

        internal LinkMicArmies(WebcastLinkMicArmies msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            BattleId = msg.Id;
            Armies = msg.BattleItems.Select(a => new Objects.LinkMicArmy(a)).ToList();
            Picture = new Objects.Picture(msg.Picture);
            BattleStatus = msg.BattleStatus;
        }
    }
}
