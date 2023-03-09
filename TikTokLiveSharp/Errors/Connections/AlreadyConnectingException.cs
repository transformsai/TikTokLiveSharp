using System;

namespace TikTokLiveSharp.Errors.Connections
{
    public class AlreadyConnectingException : Exception
    {
        public AlreadyConnectingException()
        {
        }

        public AlreadyConnectingException(string message) : base(message)
        {
        }

        public AlreadyConnectingException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}