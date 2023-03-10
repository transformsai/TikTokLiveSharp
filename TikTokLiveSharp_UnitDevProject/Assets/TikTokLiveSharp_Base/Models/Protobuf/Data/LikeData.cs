using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf.Data
{
    [ProtoContract]
    public partial class LikeData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string DataType { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        [ProtoMember(3)]
        public RankItem RankItem { get; set; }

        [ProtoMember(4)]
        public LikeUserContainer UserContainer { get; set; }
    }

    [ProtoContract]
    public partial class LikeUserContainer : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Data1 { get; set; }

        [ProtoMember(21)]
        public LikeUser User { get; set; }
    }

    [ProtoContract]
    public partial class LikeUser : AProtoBase
    {
        [ProtoMember(1)]
        public User User { get; set; }
    }
}
