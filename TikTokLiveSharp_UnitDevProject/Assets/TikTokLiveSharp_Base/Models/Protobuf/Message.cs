using ProtoBuf;
using System.ComponentModel;

namespace TikTokLiveSharp.Models.Protobuf
{
    /// <summary>
    /// Event as part of a <see cref="WebcastResponse"/>
    /// </summary>
    [ProtoContract]
    public partial class Message : AProtoBase
    {
        /// <summary>
        /// Event-Type
        /// </summary>
        [ProtoMember(1, Name = @"type")]
        [DefaultValue("")]
        public string Type { get; set; } = "";

        /// <summary>
        /// Binary Data for Message
        /// </summary>
        [ProtoMember(2, Name = @"binary")]
        public byte[] Binary { get; set; }

        //public int 
    }
}
