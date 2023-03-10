using System.Collections.Generic;
using System.Linq;
using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class BarrageMessage : AMessageData
    {
        public readonly Objects.Picture Picture;
        public readonly Objects.Picture Picture2;
        public readonly Objects.Picture Picture3;

        public readonly Objects.User User;

        public readonly Objects.BarrageData BarrageData;

        internal BarrageMessage(WebcastBarrageMessage msg) : 
            base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Picture = new Objects.Picture(msg.Picture);
            Picture2 = new Objects.Picture(msg.Picture2);
            Picture3 = new Objects.Picture(msg.Picture3);
            User = new Objects.User(msg.UserData.User);
            BarrageData = new Objects.BarrageData(msg.Data2.EventType, msg.Data2.Label, msg.Data2.Data1.Select(d => (new Objects.User(d.User.User), d.Data2)).ToList());
        }
    }
}
