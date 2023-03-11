using ProtoBuf;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class EventUser : AProtoBase
    {
        // Ranking? 
        // Amount of Likes given by this user to this stream?
        // Example:
        //# 1 - VarInt - 11
        [ProtoMember(1)]
        public ulong Data1 { get; set; }

        [ProtoMember(2)]
        public RankItem Item { get; set; }


        // In a Like-Message, this is the User
        // # 1 - User
        // In a MemberMessage, this is:
        // # 1 - OBJECT
        // #   1 - USER
   //     [ProtoMember(21)]
   //     public User User { get; set; }
    }
}
