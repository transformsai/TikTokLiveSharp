using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    // Battle Start?
    [ProtoContract]
    public partial class WebcastLinkMicBattle : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public ulong Id1 { get; set; }

        [ProtoMember(3)]
        public LinkMicBattleData1 Data1 { get; set; }

        [ProtoMember(4)]
        public uint Data2 { get; set; }

        [ProtoMember(5)]
        public List<LinkMicBattleData2> Data3 { get; } = new List<LinkMicBattleData2>();

        [ProtoMember(9)]
        public List<LinkMicBattleTeam> Teams { get; } = new List<LinkMicBattleTeam>();

        [ProtoMember(10)]
        public List<LinkMicBattleTeam> Teams2 { get; } = new List<LinkMicBattleTeam>();

        [ProtoMember(13)]
        public List<LinkMicBattleTeamDataContainer> AdditionalData { get; } = new List<LinkMicBattleTeamDataContainer>();
    }

    [ProtoContract]
    public partial class LinkMicBattleData1 : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Id1 { get; set; }

        [ProtoMember(2)]
        public ulong Timestamp1 { get; set; }

        [ProtoMember(3)]
        public uint Data1 { get; set; }

        [ProtoMember(4)]
        public ulong Id2 { get; set; }

        [ProtoMember(5)]
        public uint Data2 { get; set; }
    }

    [ProtoContract]
    public partial class LinkMicBattleData2 : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Id { get; set; }

        [ProtoMember(2)]
        public LinkMicBattleData2Details Details { get; set; }
    }

    [ProtoContract]
    public partial class LinkMicBattleData2Details : AProtoBase
    {
        // Same ID as container
        [ProtoMember(1)]
        public ulong Id { get; set; }
        [ProtoMember(2)]
        public uint Data1 { get; set; }
        [ProtoMember(3)]
        public uint Data2 { get; set; }
    }

    [ProtoContract]
    public partial class LinkMicBattleTeam : AProtoBase
    {
        [ProtoMember(1)]
        public ulong TeamId { get; set; }

        [ProtoMember(2)]
        public List<User> Users { get; } = new List<User>();
    }


    [ProtoContract]
    public partial class LinkMicBattleTeamDataContainer : AProtoBase
    {
        [ProtoMember(1)]
        public ulong TeamId { get; set; }

        [ProtoMember(2)]
        public LinkMicBattleTeamData Data { get; set; }
    }

    [ProtoContract]
    public partial class LinkMicBattleTeamData : AProtoBase
    {
        [ProtoMember(1)]
        public ulong TeamId { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public uint Data2 { get; set; }

        [ProtoMember(5)]
        public uint Data3 { get; set; }

        [ProtoMember(6)]
        [DefaultValue("")]
        public string Url { get; set; } = "";
    }
}
