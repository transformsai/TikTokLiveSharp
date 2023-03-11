using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastGoalUpdateMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; }

        [ProtoMember(2)]
        public WebcastGoalUpdateData1 Data1 { get; set; }

        [ProtoMember(3)]
        public WebcastGoalUpdateDetails Details { get; set; }

        [ProtoMember(4)]
        public ulong Id1 { get; set; }

        [ProtoMember(5)]
        public Picture Picture { get; set; }

        [ProtoMember(6)]
        [DefaultValue("")]
        public string UserId1 { get; set; } = "";

        [ProtoMember(7)]
        public WebcastGoalUpdateData2 Data2 { get; set; }

        [ProtoMember(9)]
        public uint Data3 { get; set; }

        [ProtoMember(10)]
        public uint Data4 { get; set; }

        [ProtoMember(11)]
        public uint Data5 { get; set; }

        [ProtoMember(12)]
        [DefaultValue("")]
        public string Id1String { get; set; } = "";

        [ProtoMember(15)]
        public WebcastGoalUpdateTimeData TimeData { get; set; }
    }


    [ProtoContract]
    public partial class WebcastGoalUpdateDetails : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Id1 { get; set; }

        [ProtoMember(2)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public uint Data2 { get; set; }

        [ProtoMember(4)]
        public WebcastGoalUpdateData2 Data3 { get; set; }

        [ProtoMember(5)]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        [ProtoMember(6)]
        public int Data4 { get; set; }

        [ProtoMember(7)]
        public uint Data5 { get; set; }

        [ProtoMember(8)]
        public ulong Timestamp2 { get; set; }

        [ProtoMember(9)]
        public ulong TimeStamp1 { get; set; }

        [ProtoMember(11)]
        public List<WebcastGoalUser> Users { get; } = new List<WebcastGoalUser>();

        [ProtoMember(12)]
        public uint Data7 { get; set; }

        [ProtoMember(13)]
        [DefaultValue("")]
        public string Id1String { get; set; } = "";

        // Same as #5?
        [ProtoMember(14)]
        [DefaultValue("")]
        public string Label2 { get; set; } = "";
    }

    [ProtoContract]
    public partial class WebcastGoalUpdateData1 : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string EventType { get; set; } = "";

        [ProtoMember(2)]
        public uint Data { get; set; }
    }


    [ProtoContract]
    public partial class WebcastGoalUpdateData2 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data22 { get; set; }

        [ProtoMember(3)]
        public uint Data2 { get; set; }

        [ProtoMember(4)]
        public uint Data3 { get; set; }

        [ProtoMember(5)]
        public WebcastGoalUpdateData2GiftData GiftData { get; set; }

        [ProtoMember(6)]
        [DefaultValue("")]
        public string Data4 { get; set; } = "";
    }


    [ProtoContract]
    public partial class WebcastGoalUpdateData2GiftData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Name { get; set; } = "";
        [ProtoMember(2)]
        public Picture Picture { get; set; }
        [ProtoMember(3)]
        public uint Data1 { get; set; }
        [ProtoMember(4)]
        public uint Data2 { get; set; }
    }

    [ProtoContract]
    public partial class WebcastGoalUpdateTimeData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(3)]
        public ulong Timestamp { get; set; }
    }

    [ProtoContract]
    public partial class WebcastGoalUser : AProtoBase
    {
        /// <summary>
        /// Integer-ID for User
        /// </summary>
        [ProtoMember(1)]
        public ulong UserId { get; set; }

        [ProtoMember(2)]
        public Picture ProfilePicture { get; set; }

        [ProtoMember(3, Name = @"nickname")]
        [DefaultValue("")]
        public string Nickname { get; set; } = "";

        [ProtoMember(4)]
        public ulong Timestamp1 { get; set; }

        [ProtoMember(5)]
        [DefaultValue("")]
        public string Id1 { get; set; } = "";

        [ProtoMember(6)]
        public uint Data2 { get; set; }

        [ProtoMember(7)]
        public uint Data3 { get; set; }

        [ProtoMember(10)]
        public WebcastGoalUserAdditionalData Details { get; set; }
    }


    [ProtoContract]
    public partial class WebcastGoalUserAdditionalData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(3)]
        public uint Data3 { get; set; }

        [ProtoMember(10)]
        public WebcastGoalUserLinkContainer Links { get; set; }
    }

    [ProtoContract]
    public partial class WebcastGoalUserLinkContainer : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public WebcastGoalUserLinks Links { get; set; }
    }


    [ProtoContract]
    public partial class WebcastGoalUserLinks : AProtoBase
    {
        [ProtoMember(1)]
        public List<string> Links { get; } = new List<string>();

        [ProtoMember(2)]
        [DefaultValue("")]
        public string ShortLink { get; set; } = "";

        [ProtoMember(3)]
        public uint Data1 { get; set; }

        [ProtoMember(4)]
        public uint Data2 { get; set; }

        [ProtoMember(6)]
        public uint Data3 { get; set; }

        [ProtoMember(7)]
        [DefaultValue("")]
        public string LongLink { get; set; } = "";
    }
}
