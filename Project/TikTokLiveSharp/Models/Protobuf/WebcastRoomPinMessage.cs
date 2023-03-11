using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public partial class WebcastRoomPinMessage : AProtoBase
    {
        [ProtoMember(1)]
        public BaseMessageHeader Header { get; set; }

        [ProtoMember(2)]
        public PinMessageData1 Data { get; set; }

        [ProtoMember(30)]
        [DefaultValue("")]
        public string OriginalMessageType { get; set; } = "";

        [ProtoMember(31)]
        public ulong Timestamp1 { get; set; }

        [ProtoMember(32)]
        public PinMessageData2 Data0 { get; set; }

        [ProtoMember(33)]
        public uint Data1 { get; set; }

        [ProtoMember(34)]
        public uint Data2 { get; set; }

        [ProtoMember(35)]
        public ulong Data3 { get; set; }
    }


    [ProtoContract]
    public partial class PinMessageData1 : AProtoBase
    {
        [ProtoMember(1)]
        public PinMessageData1Details Details { get; set; }

        [ProtoMember(2)]
        public User User { get; set; }

        [ProtoMember(3)]
        [DefaultValue("")]
        public string Message { get; set; } = "";

        [ProtoMember(12)]
        public User User2 { get; set; }

        [ProtoMember(14)]
        [DefaultValue("")]
        public string Language { get; set; } = "";

        [ProtoMember(18)]
        public PinMessageData1Details2 Details2 { get; set; }
    }


    [ProtoContract]
    public partial class PinMessageData1Details2 : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public uint Data2 { get; set; }

        [ProtoMember(3)]
        public uint Data3 { get; set; }

        [ProtoMember(4)]
        public uint Data4 { get; set; }

        [ProtoMember(5)]
        public uint Data5 { get; set; }
    }

    [ProtoContract]
    public partial class PinMessageData1Details : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Type { get; set; } = "";

        [ProtoMember(2)]
        public ulong RoomId { get; set; }

        [ProtoMember(3)]
        public ulong Data { get; set; }

        [ProtoMember(4)]
        public ulong TimeStamp1 { get; set; }

        [ProtoMember(6)]
        public uint Data2 { get; set; }

        [ProtoMember(8)]
        public PinMessageStringContainer String1 { get; set; }

        [ProtoMember(9)]
        public uint Data5 { get; set; }

        [ProtoMember(10)]
        public uint Data6 { get; set; }

        [ProtoMember(11)]
        public uint Data3 { get; set; }

        [ProtoMember(21)]
        public uint Data4 { get; set; }

        [ProtoMember(22)]
        public uint Data7 { get; set; }

        [ProtoMember(23)]
        public uint Data8 { get; set; }

        [ProtoMember(24)]
        public uint Data9 { get; set; }
    }

    [ProtoContract]
    public partial class PinMessageStringContainer : AProtoBase
    {
        [ProtoMember(3)]
        [DefaultValue("")]
        public string String1 { get; set; } = "";
    }

    [ProtoContract]
    public partial class PinMessageData2 : AProtoBase
    {
        [ProtoMember(1)]
        public ulong Id1 { get; set; }

        [ProtoMember(32)]
        public PinMessageData2Details Details { get; set; }
    }

    [ProtoContract]
    public partial class PinMessageData2Details : AProtoBase
    {
        [ProtoMember(2)]
        public uint Data { get; set; }
    }
}
