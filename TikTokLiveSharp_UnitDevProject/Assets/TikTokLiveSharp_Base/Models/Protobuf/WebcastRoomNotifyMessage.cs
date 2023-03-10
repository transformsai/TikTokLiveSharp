using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace TikTokLiveSharp.Models.Protobuf
{
    // Is part of WebcastGiftBroadcastMessage
    // Could be part of other BroadcastMessages?
    [ProtoContract]
    public partial class WebcastRoomNotifyMessage : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string Type { get; set; } = "";

        [ProtoMember(2)]
        public ulong Id1 { get; set; }

        [ProtoMember(3)]
        public ulong Id2 { get; set; }

        [ProtoMember(4)]
        public ulong TimeStamp1 { get; set; }

        [ProtoMember(6)]
        public uint Data1 { get; set; }

        [ProtoMember(8)]
        public NotifyData NotifyData { get; set; }
    }

    [ProtoContract]
    public partial class NotifyData : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string EventType { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Label { get; set; } = "";

        [ProtoMember(3)]
        public RankItem RankItem { get; set; }

        [ProtoMember(4)]
        public List<AdditionalNotifyData> Data { get; } = new List<AdditionalNotifyData>();
    }

    [ProtoContract]
    public partial class AdditionalNotifyData : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

 //       [ProtoMember(21)]
 //       public User User { get; set; }

        [ProtoMember(22)]
        public AdditionalNotifyDataDetailsContainer Details { get; set; }
    }


    [ProtoContract]
    public partial class AdditionalNotifyDataDetailsContainer : AProtoBase
    {
        [ProtoMember(1)]
        public uint Data1 { get; set; }

        [ProtoMember(2)]
        public AdditionalNotifyDataDetails Details { get; set; }

        [ProtoMember(3)]
        public uint Data2 { get; set; }
    }

    [ProtoContract]
    public partial class AdditionalNotifyDataDetails : AProtoBase
    {
        [ProtoMember(1)]
        [DefaultValue("")]
        public string DataType { get; set; } = "";

        [ProtoMember(2)]
        [DefaultValue("")]
        public string Label { get; set; } = "";
    }
}
