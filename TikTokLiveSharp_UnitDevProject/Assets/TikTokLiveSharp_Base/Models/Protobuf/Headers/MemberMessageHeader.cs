using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Data;

namespace TikTokLiveSharp.Models.Protobuf.Headers
{
    [ProtoContract]
    public partial class MemberMessageHeader : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string MessageType { get; set; } = "";

        [ProtoMember(2)]
        public ulong MessageId { get; set; }

        [ProtoMember(3)]
        public ulong RoomId { get; set; }

        /// <summary>
        /// Unix TimeStamp Server
        /// </summary>
        [ProtoMember(4)]
        public ulong ServerTime { get; set; }

        // Example:
        //# 6 - VarInt - 1
        [ProtoMember(6)]
        public ulong Data1 { get; set; }

        [ProtoMember(8)]
        public MemberMessageData EventData { get; set; }

        // Example:
        //# 9 - VarInt - 2
        [ProtoMember(9)]
        public ulong Data2 { get; set; }

        // Example:
        //# 10 - VarInt - 2
        [ProtoMember(10)]
        public ulong Data3 { get; set; }

        // Example:
        //# 11 - VarInt - 200000
        [ProtoMember(11)]
        public ulong Data4 { get; set; }

        // Example:
        //# 21 - VarInt - 20000
        [ProtoMember(21)]
        public ulong Data5 { get; set; }

        // Example:
        //# 22 - VarInt - 3
        [ProtoMember(22)]
        public ulong Data8 { get; set; }

        // Example:
        //# 11 - VarInt - 2
        [ProtoMember(23)]
        public ulong Data6 { get; set; }

        // Example:
        //# 11 - VarInt - 2
        [ProtoMember(24)]
        public ulong Data7 { get; set; }
    }
}
