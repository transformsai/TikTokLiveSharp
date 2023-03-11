using System.Collections.Generic;
using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class Emote : AMessageData
    {
        public readonly Objects.User User;

        public readonly string EmoteId;

        public readonly Objects.Picture Picture;

        internal Emote(WebcastEmoteChatMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            User = new Objects.User(msg.User);
            EmoteId = msg.Emote.EmoteId;
            Picture = new Objects.Picture(new List<string> { msg.Emote.Image.ImageUrl });
        }
    }
}
