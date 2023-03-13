using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    /// <summary>
    /// Data about LinkMicBattle
    /// Not really implemented right now
    /// </summary>
    public sealed class LinkMicMethod : AMessageData
    {
        public readonly string JSON;

        internal LinkMicMethod(WebcastLinkMicMethod msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        { }

        internal LinkMicMethod(Models.Protobuf.Messages.LinkMicMethod msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            JSON = msg.Json;
        }
    }
}
