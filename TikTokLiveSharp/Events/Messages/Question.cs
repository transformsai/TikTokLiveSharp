using TikTokLiveSharp.Models.Protobuf.Messages;

namespace TikTokLiveSharp.Events.MessageData.Messages
{
    public sealed class Question : AMessageData
    {
        public readonly ulong QuestionId;
        public readonly string Text;
        public readonly ulong Time;
        public readonly Objects.User User;

        internal Question(WebcastQuestionNewMessage msg) 
            : base(msg.Header.RoomId, msg.Header.MessageId, msg.Header.ServerTime)
        {
            var data = msg.Details;
            QuestionId = data.Id;
            Text = data.Text;
            Time = data.Timestamp;
            if (data?.User != null)
                User = new Objects.User(data.User);
        }
    }
}
