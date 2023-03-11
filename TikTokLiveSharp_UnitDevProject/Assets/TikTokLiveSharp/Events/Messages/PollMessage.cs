using System.Collections.Generic;
using System.Linq;
using TikTokLiveSharp.Models.Protobuf;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class PollMessage : AMessageData
    {
        public readonly ulong Id;

        public readonly Objects.PollOption Option1;

        public readonly Objects.PollOption Option2;

        public readonly List<Objects.PollOption.Option> Options;

        internal PollMessage(WebcastPollMessage msg)
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            Id = msg.Id;
            Option1 = new Objects.PollOption(new Objects.User(msg.Options1.User), msg.Options1.Options.Select(o => new Objects.PollOption.Option(o.Label, o.CurrentTotal)).ToList());
            Option2 = new Objects.PollOption(new Objects.User(msg.Options2.User), msg.Options2.Options.Select(o => new Objects.PollOption.Option(o.Label, o.CurrentTotal)).ToList());
            Options = msg.PollData.Options.Select(o => new Objects.PollOption.Option(o.Label, o.CurrentTotal)).ToList();
        }
    }
}
