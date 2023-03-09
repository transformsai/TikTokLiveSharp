using System;
using TikTokLiveSharp.Client.Proxy;

namespace TikTokLiveSharp.Client
{
    public struct ClientSettings
    {
        public TimeSpan Timeout;
        public TimeSpan PollingInterval;

        public bool FetchRoomInfoOnConnect;
        public bool HandleExistingMessagesOnConnect;
        public bool DownloadGiftInfo;

        public RotatingProxy Proxy;

        public string ClientLanguage;
        public uint SocketBufferSize;
    }
}