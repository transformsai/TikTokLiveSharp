using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class TopViewer : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue(0)]
        public uint CoinsGiven { get; set; }

        [ProtoMember(2)]
        public User User { get; set; }

        /// <summary>
        /// Index (in list)
        /// </summary>
        [ProtoMember(3)]
        public uint Rank { get; set; }
    }
}
