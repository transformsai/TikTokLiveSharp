namespace TikTokLiveSharp.Events
{
    public class ConnectionEventArgs
    {
        public ConnectionEventArgs(bool isConnected)
        {
            IsConnected = isConnected;
        }

        public readonly bool IsConnected;
    }
}