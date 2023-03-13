namespace TikTokLiveSharp.Events.MessageData.Objects
{
    public sealed class TopViewer
    {
        public readonly uint Rank;
        public readonly User User;
        public readonly uint CoinsGiven;

        internal TopViewer(Models.Protobuf.Objects.TopViewer viewer)
        {
            Rank = viewer.Rank; 
            if (viewer.User != null)
                User = new User(viewer.User);
            CoinsGiven = viewer.CoinsGiven;
        }
    }
}