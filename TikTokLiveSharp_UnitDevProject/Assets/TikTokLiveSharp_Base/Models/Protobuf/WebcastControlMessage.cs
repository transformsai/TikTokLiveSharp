using ProtoBuf;
using System.ComponentModel;
using TikTokLiveSharp.Models.Protobuf.Headers;

namespace TikTokLiveSharp.Models.Protobuf
{
    [ProtoContract]
    public enum ControlAction : uint
    {
        Unknown = 0,
        StreamPaused = 1,
        StreamEnded = 3
    }

    [ProtoContract]
    public partial class WebcastControlMessage : AProtoBase
    {
        [ProtoMember(1)]
        public WebcastControlHeader Header { get; set; }

        /// <summary>
        /// Type of Action/Event that occurred
        /// <para>
        /// 1 == App_Paused
        /// 3 == LiveStreamEnded
        /// </para>
        /// </summary>
        [ProtoMember(2, Name = @"action")]
        [DefaultValue(ControlAction.Unknown)]
        public ControlAction Action { get; set; }

        // Example (in Pause-Message):
        // 4 - Object: "B app_pause"
        //   8 - Object: "app_pause"
        //     12 - Fixed64: 7310315681510027376
        [ProtoMember(4)]
        public WebcastControlAdditionalData Data1 { get; set; }
    }

    [ProtoContract]
    public partial class WebcastControlAdditionalData : AProtoBase
    {
        [ProtoMember(8)]
        [DefaultValue("")]
        public string Type { get; set; } = "";
    }

    [ProtoContract]
    public partial class WebcastControlHeader : AProtoBase
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

        [ProtoMember(18)]
        public WebcastControlData ControlData { get; set; }
    }


    [ProtoContract]
    public partial class WebcastControlData : AProtoBase
    {
        // Example:
        // In WebcastControlMessage for Stream-End:
        // Field 1: RoomID as STRING
        // Field 2: "end_room"
        [ProtoMember(1)]
        public StringContainer Data { get; set; }

        [ProtoMember(2)]
        public ulong ServerTime { get; set; }
    }

}
