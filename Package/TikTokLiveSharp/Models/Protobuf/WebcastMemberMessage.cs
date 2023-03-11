using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Data;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public enum MemberMessageActionType
    {
        Unknown = 0,
        Joined = 1,
        Subscribed = 7
    }

    [ProtoContract]
    public partial class WebcastMemberMessage : AProtoBase
    {
        [ProtoMember(1)]
        public MemberMessageHeader Header { get; set; }

        [ProtoMember(2, Name = @"user")]
        public User User { get; set; }

        // For OnJoin-Event: Total number of current viewers in stream
        [ProtoMember(3)]
        public ulong TotalViewers { get; set; }

        [ProtoMember(4)]
        public User User2 { get; set; }

        /// <summary>
        /// Type of Action performed by User
        /// <para>
        /// 1 == Joined
        /// 7 == Subscribed
        /// 26 == ??
        /// 50 == ?? (Share?)
        /// </para>
        /// </summary>
        [ProtoMember(10)]
        public MemberMessageActionType ActionId { get; set; }

        // IDENTICAL TO DETAILS IN HEADER!
        [ProtoMember(18)]
        public MemberMessageData EventDetails { get; set; }

        // Example:
        //# 19 - String - unknown
        //# 19 - String - homepage_hot-live_cell
        [ProtoMember(19)]
        [DefaultValue("")]
        public string Data1 { get; set; }

        // Example:
        //# 20 - String - click
        [ProtoMember(20)]
        [DefaultValue("")]
        public string Data2 { get; set; }

        // Example:
        //# 21 - String - rec
        [ProtoMember(21)]
        [DefaultValue("")]
        public string Data3 { get; set; }

        // Example:
        //# 23 - String - other
        [ProtoMember(23)]
        [DefaultValue("")]
        public string Data4 { get; set; }

        [ProtoMember(24)]
        public uint Data5 { get; set; }
    }
}
