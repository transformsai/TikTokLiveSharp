using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TikTokLiveSharp.Events.MessageData.Objects
{
    public sealed class TopViewer
    {
        public readonly uint Rank;
        public readonly Objects.User User;
        public readonly uint CoinsGiven;

        internal TopViewer(Models.Protobuf.TopViewer viewer)
        {
            Rank = viewer.Rank;
            User = new Objects.User(viewer.User);
            CoinsGiven = viewer.CoinsGiven;
        }
    }
}