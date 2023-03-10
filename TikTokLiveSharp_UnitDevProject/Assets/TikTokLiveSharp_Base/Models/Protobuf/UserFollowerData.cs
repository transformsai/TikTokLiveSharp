using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class UserFollowerData : AProtoBase
    {
        /// <summary>
        /// Number of Accounts this User Follows
        /// </summary>
        [ProtoMember(1)]
        public uint Following { get; set; }
        /// <summary>
        /// Number of Accounts that Follow this User
        /// </summary>
        [ProtoMember(2)]
        public uint Followers { get; set; }
        /// <summary>
        /// Relative Role of this User to the Host
        /// TODO: Read some Messages & turn this into an Enum
        /// </summary>
        [ProtoMember(3)]
        public int FollowsHost { get; set; }
    }
}
