using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Data;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastLiveIntroMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2, Name = @"id")]
        public ulong RoomId { get; set; }

        //# 3 - VarInt - 1
        [ProtoMember(3)]
        public ulong Data1 { get; set; }

        [ProtoMember(4, Name = @"description")]
        [DefaultValue("")]
        public string Description { get; set; } = "";

        /// <summary>
        /// User who is hosting this Webcast
        /// </summary>
        [ProtoMember(5, Name = @"user")]
        public User RoomOwner { get; set; }

        [ProtoMember(6)]
        public uint Data2 { get; set; }

        //# Field 7:
        //#     1 - VarInt - 2
        //#    21 - OBJECT
        //#         1 - VarInt - 2
        //#         2 - String - pm_mt_hostlabel
        [ProtoMember(7)]
        public IntroMessageData MessageData { get; set; }

        /// <summary>
        /// ISO-Language of <see cref="RoomOwner"/>
        /// </summary>
        [ProtoMember(8)]
        [DefaultValue("")]
        public string Language { get; set; } = "";
    }
}
