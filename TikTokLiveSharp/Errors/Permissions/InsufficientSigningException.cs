using System;

namespace TikTokLiveSharp.Errors.Permissions
{
    public class InsufficientSigningException : Exception
    {
        public InsufficientSigningException()
        {
        }

        public InsufficientSigningException(string message) : base(message)
        {
        }

        public InsufficientSigningException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}