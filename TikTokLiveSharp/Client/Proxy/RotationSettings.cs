﻿namespace TikTokLiveSharp.Client.Proxy
{
    /// <summary>
    /// Rotation settings for a proxy container.
    /// </summary>
    public enum RotationSettings
    {
        /// <summary>
        /// Rotate addresses consecutively, from proxy 0 -> 1 -> 2 -> ...etc.
        /// </summary>
        Consecutive = 1,

        /// <summary>
        /// Rotate addresses randomly, from proxy 0 -> 69 -> 420 -> 1 -> ...etc.
        /// </summary>
        Random = 2,

        /// <summary>
        /// Don't rotate addresses at all, pin to the indexed address.
        /// </summary>
        Pinned = 3
    }
}