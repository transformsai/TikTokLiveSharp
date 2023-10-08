namespace TikTokLiveSharp.Models.Protobuf.Enums
{
    [System.Serializable]
    public enum ControlAction : long
    {
        Unknown = 0,
        /// <summary>
        /// Stream Paused by Host
        /// </summary>
        Stream_Paused = 1,
        // Unknown type = 2
        /// <summary>
        /// Stream Ended by Host
        /// </summary>
        Stream_Ended = 3
    }
}
