using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TikTokLiveSharp.Client.Proxy;
using TikTokLiveSharp.Debugging;
using TikTokLiveSharp.Errors.Messaging;
using TikTokLiveSharp.Events;
using TikTokLiveSharp.Events.MessageData.Messages;
using TikTokLiveSharp.Models.Protobuf;
#if UNITY
using TikTokLiveSharp.Unity.Utils;
#endif
using TikTokGift = TikTokLiveSharp.Events.MessageData.Objects.TikTokGift;
using RoomMessage = TikTokLiveSharp.Events.MessageData.Messages.RoomMessage;
using LinkMicMethod = TikTokLiveSharp.Events.MessageData.Messages.LinkMicMethod;
using Newtonsoft.Json.Serialization;

namespace TikTokLiveSharp.Client
{
    public class TikTokLiveClient : TikTokBaseClient
    {
        private struct GiftId
        {
            public ulong Gift;
            public string UserName;

            public override int GetHashCode()
            {
                return Gift.GetHashCode() + UserName.GetHashCode();
            }
        }

        private struct SocialTypes
        {
            public const string LikeType = "pm_mt_msg_viewer";
            public const string FollowType = "pm_main_follow_message_viewer_2";
            public const string ShareType = "pm_mt_guidance_share";
            public const string JoinType = "pm_mt_join_message_other_viewer";
        }

        private readonly Dictionary<GiftId, TikTokGift> activeGifts = new Dictionary<GiftId, TikTokGift>();

        /// <summary>
        /// Creates a new instance of the TikTok Live client.
        /// Used for retrieving a stream of information from live streams.
        /// </summary>
        /// <param name="uniqueID">The unique ID of the user.</param>
        /// <param name="timeout">The timeout to be used with requests.</param>
        /// <param name="pollingInterval">The polling interval to use (The space between requests).</param>
        /// <param name="clientParams">The client parameters available.</param>
        /// <param name="processInitialData">Should the data be processed on connection.</param>
        /// <param name="fetchRoomInfoOnConnect">Should room information be retrieved on connection.</param>
        /// <param name="enableExtendedGiftInfo"></param>
        /// <param name="proxyHandler"></param>
        /// <param name="lang"></param>
        public TikTokLiveClient(string uniqueID,
            TimeSpan? timeout = null,
            TimeSpan? pollingInterval = null,
            Dictionary<string, object> clientParams = null,
            bool processInitialData = true,
            bool enableExtendedGiftInfo = true,
            RotatingProxy proxyHandler = null,
            string lang = "en-US") : base(uniqueID,
                timeout,
                pollingInterval,
                clientParams,
                processInitialData,
                enableExtendedGiftInfo,
                proxyHandler,
                lang)
        { }

        ~TikTokLiveClient()
        {
            socketClient?.Disconnect();
        }

        public event TikTokEventHandler<RoomMessage> OnRoomIntro;

        public event TikTokEventHandler<RoomMessage> OnRoomMessage;

        public event TikTokEventHandler<RoomMessage> OnSystemMessage;

        /// <summary>
        /// Event fired when comments are received.
        /// </summary>
        public event TikTokEventHandler<Comment> OnComment;


        /// <summary>
        /// Event fired when the view count updates.
        /// </summary>
        public event TikTokEventHandler<RoomViewerData> OnViewerData;

        /// <summary>
        /// Event fired when the live has ended.
        /// </summary>
        public event TikTokEventHandler OnLiveEnded;
        public event TikTokEventHandler OnLivePaused;


        /// <summary>
        /// Event fired when a gift-Message is received.
        /// <para>
        /// Fires for every GiftMessage
        /// </para>
        /// </summary>
        public event TikTokEventHandler<GiftMessage> OnGiftMessage;
        /// <summary>
        /// Event fired when a gift-Message is received.
        /// <para>
        /// Fires when a gift-streak starts. Gift-Events are fired for subsequent messages.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<TikTokGift> OnGift;
        /// <summary>
        /// Event fired when someone likes the stream.
        /// </summary>
        public event TikTokEventHandler<Like> OnLike;

        /// <summary>
        /// Event fired when the stream is shared.
        /// </summary>
        public event TikTokEventHandler<Share> OnShare;

        /// <summary>
        /// Event fired when someone follows.
        /// </summary>
        public event TikTokEventHandler<Follow> OnFollow;

        /// <summary>
        /// Event fired when someone joins the stream.
        /// </summary>
        public event TikTokEventHandler<Join> OnJoin;

        /// <summary>
        /// Event fired when a user subscribes.
        /// </summary>
        public event TikTokEventHandler<Subscribe> OnSubscribe;

        /// <summary>
        /// Event fired when a LinkMicBattle starts
        /// </summary>
        public event TikTokEventHandler<LinkMicBattle> OnLinkMicBattle;

        public event TikTokEventHandler<LinkMicArmies> OnLinkMicArmies;

        /// <summary>
        /// Data for LinkMic-Connection(s)
        /// </summary>
        public event TikTokEventHandler<LinkMicMethod> OnLinkMicMethod;

        public event TikTokEventHandler<LinkMicFanTicket> OnLinkMicFanTicket;

        public event TikTokEventHandler<RankText> OnRankText;
        public event TikTokEventHandler<RankUpdate> OnRankUpdate;

        /// <summary>
        /// Closed Caption-Message for Video
        /// </summary>
        public event TikTokEventHandler<Caption> OnClosedCaption;

        public event TikTokEventHandler<PollMessage> OnPollMessage;

        public event TikTokEventHandler<InRoomBanner> OnInRoomBanner;

        /// <summary>
        /// Event Fired when Host pins a Comment to the Stream
        /// </summary>
        public event TikTokEventHandler<RoomPinMessage> OnRoomPinMessage;

        public event TikTokEventHandler<DetectMessage> OnDetectMessage;

        public event TikTokEventHandler<BarrageMessage> OnBarrageMessage;

        public event TikTokEventHandler<GoalUpdate> OnGoalUpdate;

        public event TikTokEventHandler<UnauthorizedMember> OnUnauthorizedMember;

        public event TikTokEventHandler<LinkMessage> OnLinkMessage;

        public event TikTokEventHandler<LinkLayerMessage> OnLinkLayerMessage;

        public event TikTokEventHandler<GiftBroadcast> OnGiftBroadcast;

        public event TikTokEventHandler<ShopMessage> OnShopMessage;

        public event TikTokEventHandler<IMDelete> OnIMDelete;

        public event TikTokEventHandler<Question> OnQuestion;

        public event TikTokEventHandler<Envelope> OnEnvelope;

        public event TikTokEventHandler<SubNotify> OnSubNotify;

        public event TikTokEventHandler<Emote> OnEmote;
        /// <summary>
        /// Event fired when an unhandled social event is received from the webcast.
        /// It's up to you how you can interpret this message.
        /// </summary>
        public event TikTokEventHandler<WebcastSocialMessage> UnhandledSocialEvent;

        /// <summary>
        /// Event fired when an unhandled member event is received from the webcast.
        /// It's up to you how you can interpret this message.
        /// </summary>
        public event TikTokEventHandler<WebcastMemberMessage> UnhandledMemberEvent;

        /// <summary>
        /// Event fired when an unhandled event is received from the webcast.
        /// It's up to you how you can interpret this message.
        /// </summary>
        public event TikTokEventHandler<Message> UnhandledEvent;
















        /// <summary>
        /// Event fired when the client connects.
        /// </summary>
        public event TikTokEventHandler<bool> OnConnected;

        /// <summary>
        /// Event fired when the client disconnects.
        /// </summary>
        public event TikTokEventHandler<bool> OnDisconnected;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="onConnectException">Callback for possible Exceptions during Connect</param>
        /// <returns></returns>
        protected override async Task<string> Connect(Action<Exception> onConnectException = null)
        {
            string roomID = await base.Connect(onConnectException);
            if (Settings.PrintToConsole)
                Debug.Log($"Connected to Room {roomID}");
            RunEvent(OnConnected, Connected);
            return roomID;
        }

        protected override async Task Disconnect()
        {
            await base.Disconnect();
            if (Settings.PrintToConsole)
                Debug.Log("Disconnected");
            RunEvent(OnDisconnected, Connected);
        }

        protected override void HandleWebcastMessages(WebcastResponse webcastResponse)
        {
            foreach (var message in webcastResponse.Messages)
            {
                try
                {
                    HandleMessage(message);
                }
                catch (Exception e)
                {
                    WebcastMessageException exc = new WebcastMessageException("Error whilst Handling Message. Stopping Client.", e);
                    CallOnException(exc);
                    // This is an irrecoverable error. Crash this Thread
                    throw exc;
                }
            }
        }


        #region HandleMessage

        private void HandleMessage(Message message)
        {
            Debug.Log("Handling Message: " + message.Type);
            if (Settings.PrintMessageData)
            {
                string msg = Convert.ToBase64String(message.Binary);
                Debug.Log($"Handling Message:{Environment.NewLine}{msg}");
            }

            using (var stream = new MemoryStream(message.Binary))
                switch (message.Type)
                {
                    #region ConnectionEvents
                    case nameof(WebcastControlMessage):
                        WebcastControlMessage controlMessage = Serializer.Deserialize<WebcastControlMessage>(stream);
                        if (controlMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(controlMessage);
                            switch (controlMessage.Action)
                            {                                
                                case ControlAction.StreamPaused:
                                    RunEvent(OnLivePaused);
                                    return;
                                case ControlAction.StreamEnded:
                                    RunEvent(OnLiveEnded);
                                    return;
                                default:
                                case ControlAction.Unknown:
                                    Debug.LogWarning("Unhandled Message");
                                    // Run Unhandled
                                    RunEvent(UnhandledEvent, message);
                                    return;
                            }
                        }
                        return;
                    case nameof(SystemMessage):
                        SystemMessage systemMessage = Serializer.Deserialize<SystemMessage>(stream);
                        if (systemMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(systemMessage);
                            RunEvent(OnSystemMessage, new RoomMessage(systemMessage));
                        }
                        return;
                    #endregion

                    #region RoomStatus
                    case nameof(WebcastLiveIntroMessage):
                        WebcastLiveIntroMessage introMessage = Serializer.Deserialize<WebcastLiveIntroMessage>(stream);
                        if (introMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(introMessage);
                            RunEvent(OnRoomIntro, new Events.MessageData.Messages.RoomMessage(introMessage));
                        }
                        return;
                    case nameof(WebcastRoomUserSeqMessage):
                        WebcastRoomUserSeqMessage userSeqMessage = Serializer.Deserialize<WebcastRoomUserSeqMessage>(stream);
                        if (userSeqMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(userSeqMessage);
                            viewerCount = userSeqMessage.ViewerCount;
                            RunEvent(OnViewerData, new RoomViewerData(userSeqMessage));
                        }
                        return;
                    case nameof(Models.Protobuf.RoomMessage):
                        Models.Protobuf.RoomMessage roomMessage = Serializer.Deserialize<Models.Protobuf.RoomMessage>(stream);
                        if (roomMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(roomMessage);
                            RunEvent(OnRoomMessage, new RoomMessage(roomMessage));
                        }
                        return;
                    case nameof(WebcastRoomMessage):
                        WebcastRoomMessage webcastRoomMessage = Serializer.Deserialize<WebcastRoomMessage>(stream);
                        if (webcastRoomMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(webcastRoomMessage);
                            RunEvent(OnRoomMessage, new RoomMessage(webcastRoomMessage));
                        }
                        return;
                    case nameof(WebcastCaptionMessage):
                        WebcastCaptionMessage captionMessage = Serializer.Deserialize<WebcastCaptionMessage>(stream);
                        if (captionMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(captionMessage);
                            RunEvent(OnClosedCaption, new Caption(captionMessage));
                        }
                        return;
                    #endregion

                    #region User-Interaction
                    case nameof(WebcastChatMessage):
                        WebcastChatMessage chatMessage = Serializer.Deserialize<WebcastChatMessage>(stream);
                        if (chatMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(chatMessage);
                            RunEvent(OnComment, new Comment(chatMessage));
                        }
                        return;
                    case nameof(WebcastLikeMessage):
                        WebcastLikeMessage likeMessage = Serializer.Deserialize<WebcastLikeMessage>(stream);
                        if (likeMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(likeMessage);
                            RunEvent(OnLike, new Like(likeMessage));
                        }
                        return;
                    case nameof(WebcastGiftMessage):
                        WebcastGiftMessage giftMessage = Serializer.Deserialize<WebcastGiftMessage>(stream);
                        if (giftMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(giftMessage);
                            HandleGiftMessage(giftMessage);
                        }
                        return;
                    case nameof(WebcastSocialMessage):
                        WebcastSocialMessage socialMessage = Serializer.Deserialize<WebcastSocialMessage>(stream);
                        if (socialMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(socialMessage);
                            HandleSocialMessage(socialMessage);
                        }
                        return;
                    case nameof(WebcastMemberMessage):
                        WebcastMemberMessage memberMessage = Serializer.Deserialize<WebcastMemberMessage>(stream);
                        if (memberMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(memberMessage);
                            HandleMemberMessage(memberMessage);
                        }
                        return;

                    #endregion

                    #region Host-Interaction
                    case nameof(WebcastPollMessage):
                        WebcastPollMessage pollMessage = Serializer.Deserialize<WebcastPollMessage>(stream);
                        if (pollMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(pollMessage);
                            RunEvent(OnPollMessage, new PollMessage(pollMessage));
                        }
                        return;
                    case nameof(WebcastRoomPinMessage):
                        WebcastRoomPinMessage pinMessage = Serializer.Deserialize<WebcastRoomPinMessage>(stream);
                        if (pinMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(pinMessage);
                            RunEvent(OnRoomPinMessage, new RoomPinMessage(pinMessage));
                        }
                        return;
                    case nameof(WebcastGoalUpdateMessage):
                        WebcastGoalUpdateMessage goalUpdateMessage = Serializer.Deserialize<WebcastGoalUpdateMessage>(stream);
                        if (goalUpdateMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(goalUpdateMessage);
                            RunEvent(OnGoalUpdate, new GoalUpdate(goalUpdateMessage));
                        }
                        return;
                    #endregion

                    #region LinkMic
                    case nameof(WebcastLinkMicBattle):
                        WebcastLinkMicBattle linkMicBattleMessage = Serializer.Deserialize<WebcastLinkMicBattle>(stream);
                        if (linkMicBattleMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(linkMicBattleMessage);
                            RunEvent(OnLinkMicBattle, new LinkMicBattle(linkMicBattleMessage));
                        }
                        return;
                    case nameof(WebcastLinkMicArmies):
                        WebcastLinkMicArmies linkMicArmiesMessage = Serializer.Deserialize<WebcastLinkMicArmies>(stream);
                        if (linkMicArmiesMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(linkMicArmiesMessage);
                            RunEvent(OnLinkMicArmies, new LinkMicArmies(linkMicArmiesMessage));
                        }
                        return;
                    case nameof(Models.Protobuf.LinkMicMethod):
                        Models.Protobuf.LinkMicMethod linkMicMethodMessage = Serializer.Deserialize<Models.Protobuf.LinkMicMethod>(stream);
                        if (linkMicMethodMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(linkMicMethodMessage);
                            RunEvent(OnLinkMicMethod, new LinkMicMethod(linkMicMethodMessage));
                        }
                        return;
                    case nameof(Models.Protobuf.WebcastLinkMicMethod):
                        Models.Protobuf.WebcastLinkMicMethod webcastLinkMicMethodMessage = Serializer.Deserialize<Models.Protobuf.WebcastLinkMicMethod>(stream);
                        if (webcastLinkMicMethodMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(webcastLinkMicMethodMessage);
                            RunEvent(OnLinkMicMethod, new LinkMicMethod(webcastLinkMicMethodMessage));
                        }
                        return;
                    case nameof(WebcastLinkMicFanTicketMethod):
                        WebcastLinkMicFanTicketMethod webcastFanTicketMessage = Serializer.Deserialize<WebcastLinkMicFanTicketMethod>(stream);
                        if (webcastFanTicketMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(webcastFanTicketMessage);
                            RunEvent(OnLinkMicFanTicket, new LinkMicFanTicket(webcastFanTicketMessage));
                        }
                        return;
                    #endregion

                    #region Rank
                    case nameof(WebcastRankTextMessage):
                        WebcastRankTextMessage webcastRankTextMessage = Serializer.Deserialize<WebcastRankTextMessage>(stream);
                        if (webcastRankTextMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(webcastRankTextMessage);
                            RunEvent(OnRankText, new RankText(webcastRankTextMessage));
                        }
                        return;
                    case nameof(WebcastRankUpdateMessage):
                        WebcastRankUpdateMessage webcastRankUpdateMessage = Serializer.Deserialize<WebcastRankUpdateMessage>(stream);
                        if (webcastRankUpdateMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(webcastRankUpdateMessage);
                            RunEvent(OnRankUpdate, new RankUpdate(webcastRankUpdateMessage));
                        }
                        return;
                    case nameof(WebcastHourlyRankMessage):
                        WebcastHourlyRankMessage webcastHourlyRankUpdateMessage = Serializer.Deserialize<WebcastHourlyRankMessage>(stream);
                        if (webcastHourlyRankUpdateMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(webcastHourlyRankUpdateMessage);
                            RunEvent(OnRankUpdate, new RankUpdate(webcastHourlyRankUpdateMessage));
                        }
                        return;
                    #endregion

                    #region Others
                    case nameof(WebcastInRoomBannerMessage):
                        WebcastInRoomBannerMessage webcastInRoomBannerMessage = Serializer.Deserialize<WebcastInRoomBannerMessage>(stream);
                        if (webcastInRoomBannerMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(webcastInRoomBannerMessage);
                            RunEvent(OnInRoomBanner, new InRoomBanner(webcastInRoomBannerMessage));
                        }
                        return;
                    case nameof(WebcastMsgDetectMessage):
                        WebcastMsgDetectMessage detectMessage = Serializer.Deserialize<WebcastMsgDetectMessage>(stream);
                        if (detectMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(detectMessage);
                            RunEvent(OnDetectMessage, new DetectMessage(detectMessage));
                        }
                        return;
                    case nameof(WebcastBarrageMessage):
                        WebcastBarrageMessage barrageMessage = Serializer.Deserialize<WebcastBarrageMessage>(stream);
                        if (barrageMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(barrageMessage);
                            RunEvent(OnBarrageMessage, new BarrageMessage(barrageMessage));
                        }
                        return;
                    case nameof(WebcastUnauthorizedMemberMessage):
                        WebcastUnauthorizedMemberMessage unauthorizedMemberMessage = Serializer.Deserialize<WebcastUnauthorizedMemberMessage>(stream);
                        if (unauthorizedMemberMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(unauthorizedMemberMessage);
                            RunEvent(OnUnauthorizedMember, new UnauthorizedMember(unauthorizedMemberMessage));
                        }
                        return;
                    case nameof(WebcastLinkMessage):
                        WebcastLinkMessage linkMessage = Serializer.Deserialize<WebcastLinkMessage>(stream);
                        if (linkMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(linkMessage);
                            RunEvent(OnLinkMessage, new LinkMessage(linkMessage));
                        }
                        return;
                    case nameof(WebcastLinkLayerMessage):
                        WebcastLinkLayerMessage linkLayerMessage = Serializer.Deserialize<WebcastLinkLayerMessage>(stream);
                        if (linkLayerMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(linkLayerMessage);
                            RunEvent(OnLinkLayerMessage, new LinkLayerMessage(linkLayerMessage));
                        }
                        return;
                    case nameof(WebcastGiftBroadcastMessage):
                        WebcastGiftBroadcastMessage webcastGiftBroadcast = Serializer.Deserialize<WebcastGiftBroadcastMessage>(stream);
                        if (webcastGiftBroadcast != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(webcastGiftBroadcast);
                            RunEvent(OnGiftBroadcast, new GiftBroadcast(webcastGiftBroadcast));
                        }
                        return;
                    case nameof(WebcastOecLiveShoppingMessage):
                        WebcastOecLiveShoppingMessage oecLiveShoppingMessage = Serializer.Deserialize<WebcastOecLiveShoppingMessage>(stream);
                        if (oecLiveShoppingMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(oecLiveShoppingMessage);
                            RunEvent(OnShopMessage, new ShopMessage(oecLiveShoppingMessage));
                        }
                        return;
                    case nameof(WebcastImDeleteMessage):
                        WebcastImDeleteMessage imDeleteMessage = Serializer.Deserialize<WebcastImDeleteMessage>(stream);
                        if (imDeleteMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(imDeleteMessage);
                            RunEvent(OnIMDelete, new IMDelete(imDeleteMessage));
                        }
                        return;
                    case nameof(WebcastQuestionNewMessage):
                        WebcastQuestionNewMessage newQuestionMessage = Serializer.Deserialize<WebcastQuestionNewMessage>(stream);
                        if (newQuestionMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(newQuestionMessage);
                            RunEvent(OnQuestion, new Question(newQuestionMessage));
                        }
                        return;
                    case nameof(WebcastEnvelopeMessage):
                        WebcastEnvelopeMessage envelopeMessage = Serializer.Deserialize<WebcastEnvelopeMessage>(stream);
                        if (envelopeMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(envelopeMessage);
                            RunEvent(OnEnvelope, new Envelope(envelopeMessage));
                        }
                        return;
                    case nameof(WebcastSubNotifyMessage):
                        WebcastSubNotifyMessage subNotifyMessage = Serializer.Deserialize<WebcastSubNotifyMessage>(stream);
                        if (subNotifyMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(subNotifyMessage);
                            RunEvent(OnSubNotify, new SubNotify(subNotifyMessage));
                        }
                        return;
                    case nameof(WebcastEmoteChatMessage):
                        WebcastEmoteChatMessage emoteMessage = Serializer.Deserialize<WebcastEmoteChatMessage>(stream);
                        if (emoteMessage != null)
                        {
                            if (Settings.CheckForUnparsedData)
                                CheckForUnparsedData(emoteMessage);
                            RunEvent(OnEmote, new Emote(emoteMessage));
                        }
                        return;
                    #endregion

                    default:
                        if (Settings.PrintToConsole && Settings.LogLevel.HasFlag(LogLevel.Warning))
                        {
                            Debug.LogWarning($"Unknown MessageType: {message.Type}");
                            Debug.LogWarning(Convert.ToBase64String(message.Binary));
                        }
                        // Run Unhandled
                        RunEvent(UnhandledEvent, message);
                        return;
                }
        }

        private void HandleGiftMessage(WebcastGiftMessage message)
        {
            GiftId giftId = new GiftId
            {
                Gift = message.GiftId,
                UserName = message.User.UniqueId
            };
            if (activeGifts.ContainsKey(giftId))
                activeGifts[giftId].UpdateGiftAmount(message.Amount);
            else
            {
                TikTokGift newGift = new TikTokGift(message);
                lock (activeGifts)
                    activeGifts.Add(giftId, newGift);
                RunEvent(OnGift, newGift);
            }
            if (message.RepeatEnd)
            {
                lock (activeGifts)
                {
                    activeGifts[giftId].FinishStreak();
                    activeGifts.Remove(giftId);
                }
            }
            RunEvent(OnGiftMessage, new GiftMessage(message));
        }

        private void HandleSocialMessage(WebcastSocialMessage messageEvent)
        {
            Match match = Regex.Match(messageEvent.Header.Details.DataType, "pm_mt_guidance_viewer_([0-9]+)_share");
            if (match.Success)
            {
                Debug.Log("Handling Share");
                if (int.TryParse(match.Groups[0].Value, out int result))
                    RunEvent(OnShare, new Share(messageEvent, result));
                return;
            }
            switch (messageEvent.Header.Details.DataType)
            {
                case SocialTypes.LikeType:
                    Debug.Log("Handling Like");
                    RunEvent(OnLike, new Like(messageEvent));
                    return;
                case SocialTypes.FollowType:
                    Debug.Log("Handling Follow");
                    RunEvent(OnFollow, new Follow(messageEvent));
                    return;
                case SocialTypes.ShareType:
                    Debug.Log("Handling Share");
                    RunEvent(OnShare, new Share(messageEvent, 1));
                    return;
                case SocialTypes.JoinType:
                    Debug.Log("Handling Join");
                    RunEvent(OnJoin, new Join(messageEvent));
                    return;
                default:
                    Debug.LogWarning("Unhandled Message!");
                    // Run Unhandled
                    RunEvent(UnhandledSocialEvent, messageEvent);
                    return;
            }
        }

        private void HandleMemberMessage(WebcastMemberMessage msg)
        {
            switch (msg.ActionId)
            {
                case MemberMessageActionType.Joined:
                    Debug.Log("Handling Join");
                    RunEvent(OnJoin, new Join(msg));
                    return;
                case MemberMessageActionType.Subscribed:
                    Debug.Log("Handling Subscribe");
                    RunEvent(OnSubscribe, new Subscribe(msg));
                    return;
//                case 26: // As of yet undetermined
//                case 27: // ?? (Done by host? (User null, User2 == host))
//                case 50: // Share?
                default:
                case MemberMessageActionType.Unknown:
                    RunEvent(UnhandledMemberEvent, msg);
                    return;
            }
        }

        private void RunEvent(EventHandler evt, EventArgs e = null) => RunEvent(() => evt?.Invoke(this, e));
        private void RunEvent(TikTokEventHandler evt, EventArgs e = null) => RunEvent(() => evt?.Invoke(this, e));
        private void RunEvent<TEventArgs>(TikTokEventHandler<TEventArgs> evt, TEventArgs args) => RunEvent(() => evt?.Invoke(this, args));
        private void RunEvent(Action action)
        {
#if UNITY
            // Run Events on Unity's Main Thread instead of the Socket-Thread. This will solve about 75% of user-errors.
            Dispatcher.RunOnMainThread(() => {
#endif
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    UserCallbackException exc = new UserCallbackException("Error during custom User-Event", e);
                    CallOnException(exc);
                    // This is a recoverable error. Client remains connected. Do not Throw
                }
#if UNITY
            });
#endif
        }

        private T Deserialize<T>(MemoryStream stream) where T : IExtensible
        {
            try
            {
                T msg = Serializer.Deserialize<T>(stream);
                return msg;
            }
            catch (Exception ex)
            {
                WebcastMessageException exc = new WebcastMessageException("Error Deserializing Message", ex);
                CallOnException(exc); // Inform user of Error
                // This is a recoverable error. Client remains connected. Do not Throw
                return default; // Return null to quietly quit out of this message
            }
        }
        #endregion

        #region Debug
        private void CheckForUnparsedData(AProtoBase msg)
        {
            if (msg._extension != null && msg._extension.GetLength() > 0)
            {
                Debug.LogWarning($"Found unparsed Data in Message {msg.GetType()}");
                FieldInfo f = msg._extension.GetType().GetField("buffer", BindingFlags.Instance | BindingFlags.NonPublic);
                byte[] foundMsg = (byte[])f.GetValue(msg._extension);
                Debug.Log($"Unparsed Data: {Environment.NewLine}{Convert.ToBase64String(foundMsg)}");
            }
            // Check Child-Objects
            RecursiveCheckForUnparsedData(msg);
        }

        private void RecursiveCheckForUnparsedData(AProtoBase msg)
        {
            PropertyInfo[] properties = msg.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                object obj = properties[i].GetValue(msg);
                if (obj != null && typeof(AProtoBase).IsAssignableFrom(obj.GetType()))
                    CheckForUnparsedData((AProtoBase)obj);
            }
        }
        #endregion
    }
}