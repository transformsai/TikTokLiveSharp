using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Client.Config;
using TikTokLiveSharp.Events;
using TikTokLiveSharp.Events.Beta;
using TikTokLiveSharp.Events.Objects;
using TikTokLiveSharp.Models.HTTP;
using TikTokLiveUnity.Utils;
using UnityEngine;
using UnityEngine.Events;
using Debug = TikTokLiveSharp.Debugging.Debug;
using Picture = TikTokLiveSharp.Events.Objects.Picture;

namespace TikTokLiveUnity
{
    /// <summary>
    /// Manager for TikTokLive-Client in Unity
    /// </summary>
    public class TikTokLiveManager : MonoBehaviour
    {
        #region Singleton
        /// <summary>
        /// Whether this GameObject is not Root
        /// </summary>
        [SerializeField]
        [Tooltip("Enable if this GameObject is not Root")]
        private bool hasRootObject;

        /// <summary>
        /// Whether an Instance Exists (!= NULL)
        /// </summary>
        public static bool Exists => instance != null;

        /// <summary>
        /// Singleton-Instance for TikTokLiveManager. Auto-Creates new TikTokLiveManager if none exists
        /// </summary>
        public static TikTokLiveManager Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                GameObject obj = new GameObject("TikTokLiveManager");
                instance = obj.AddComponent<TikTokLiveManager>();
                instance.Reset();
                if (instance.ShouldLog(LogLevel.Warning))
                    Debug.LogWarning("TikTokLiveManager-Instance does not exist. Created new Instance", obj);
                return instance;
            }
            protected set => instance = value;
        }

        /// <summary>
        /// Singleton-Instance for TikTokLiveManager
        /// </summary>
        protected static TikTokLiveManager instance;
        #endregion

        #region Properties
        #region Public
        /// <summary>
        /// Gifts Available (for Room)
        /// </summary>
        public IReadOnlyDictionary<long, TikTokGiftData> AvailableGifts => client?.AvailableGifts;
        /// <summary>
        /// Gifts Displayed (for Room)
        /// <para>
        /// Does not include gifts considered 'Exclusive' by TikTok
        /// </para>
        /// </summary>
        public IReadOnlyDictionary<long, TikTokGiftData> DisplayedGifts => client?.DisplayedGifts;
        /// <summary>
        /// Whether currently Connected to the TikTokLive-Servers
        /// </summary>
        public bool Connected => client?.Connected ?? false;
        /// <summary>
        /// Whether currently initializing a Connection to the TikTokLive-Servers
        /// </summary>
        public bool Connecting => client?.Connecting ?? false;
        /// <summary>
        /// Username for Host
        /// </summary>
        public string HostName => client?.HostName;
        /// <summary>
        /// RoomInfo
        /// </summary>
        public JObject RoomInfo => client?.RoomInfo;
        /// <summary>
        /// Id for Room
        /// </summary>
        public string RoomId => client?.RoomID;
        /// <summary>
        /// Number of Viewers in Room
        /// </summary>
        public long? ViewerCount => client?.ViewerCount;
        #endregion

        #region Unity
        /// <summary>
        /// Max amount of Textures Cached
        /// </summary>
        [Min(1)]
        [SerializeField]
        [Header("TextureCache")]
        [Tooltip("Max amount of Textures Cached")]
        private uint texCacheSize = 2048;

        /// <summary>
        /// Whether to Connect on Start (using set HostId)
        /// </summary>
        [SerializeField]
        [Header("Auto-Connect")]
        [Tooltip("Whether to Connect on Start (using set HostId)")]
        private bool autoConnect;
        /// <summary>
        /// HostName to Connect to on Start
        /// </summary>
        [SerializeField]
        [Tooltip("HostName to Connect to on Start")]
        private string autoConnectHostId;
        /// <summary>
        /// Settings for TikTokClient
        /// </summary>
        [SerializeField]
        [Tooltip("Settings for TikTokClient")]
        private ClientSettings settings;
        #endregion

        #region Private
        /// <summary>
        /// TikTokClient used for Connection
        /// </summary>
        protected TikTokLiveClient client;
        /// <summary>
        /// Tokensource for CancelToken used with Client-Threads
        /// </summary>
        protected CancellationTokenSource tokenSource = new CancellationTokenSource();
        /// <summary>
        /// Cached Access for Textures
        /// </summary>
        private TextureCache textureCache;
        #endregion

        #region Events
        #region C#
        /// <summary>
        /// Event thrown if an Exception occurred.
        /// </summary>
        public event EventHandler<Exception> OnException;
        private void InvokeOnException(object sender, Exception e) => OnException?.Invoke(sender, e);

        /// <summary>
        /// Event fired when the client connects.
        /// </summary>
        public event TikTokEventHandler<bool> OnConnected;
        private void InvokeOnConnected(TikTokLiveClient sender, bool val) => OnConnected?.Invoke(sender, val);

        /// <summary>
        /// Event fired when the client disconnects.
        /// </summary>
        public event TikTokEventHandler<bool> OnDisconnected;
        private void InvokeOnDisconnected(TikTokLiveClient sender, bool val) => OnDisconnected?.Invoke(sender, val);

        public event TikTokEventHandler<AccessControl> OnAccessControl;
        private void InvokeOnAccessControl(TikTokLiveClient sender, AccessControl val) => OnAccessControl?.Invoke(sender, val);

        public event TikTokEventHandler<AccessRecall> OnAccessRecall;
        private void InvokeOnAccessRecall(TikTokLiveClient sender, AccessRecall val) => OnAccessRecall?.Invoke(sender, val);

        public event TikTokEventHandler<AlertBoxAuditResult> OnAlertBoxAuditResult;
        private void InvokeOnAlertBoxAuditResult(TikTokLiveClient sender, AlertBoxAuditResult val) => OnAlertBoxAuditResult?.Invoke(sender, val);

        public event TikTokEventHandler<Barrage> OnBarrage;
        private void InvokeOnBarrage(TikTokLiveClient sender, Barrage val) => OnBarrage?.Invoke(sender, val);

        public event TikTokEventHandler<BindingGift> OnBindingGift;
        private void InvokeOnBindingGift(TikTokLiveClient sender, BindingGift val) => OnBindingGift?.Invoke(sender, val);

        public event TikTokEventHandler<BoostCardMessage> OnBoostCardMessage;
        private void InvokeOnBoostCardMessage(TikTokLiveClient sender, BoostCardMessage val) => OnBoostCardMessage?.Invoke(sender, val);

        public event TikTokEventHandler<BottomMessage> OnBottomMessage;
        private void InvokeOnBottomMessage(TikTokLiveClient sender, BottomMessage val) => OnBottomMessage?.Invoke(sender, val);

        /// <summary>
        /// Closed Caption-Message for Video.
        /// </summary>
        public event TikTokEventHandler<Caption> OnCaption;
        private void InvokeOnCaption(TikTokLiveClient sender, Caption val) => OnCaption?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a viewer Commented on the stream.
        /// </summary>
        public event TikTokEventHandler<Chat> OnChatMessage;
        private void InvokeOnChatMessage(TikTokLiveClient sender, Chat val) => OnChatMessage?.Invoke(sender, val);

        /// <summary>
        /// Event fired when the Host changed the stream's Status (e.g. Stream Ended)
        /// </summary>
        public event TikTokEventHandler<ControlMessage> OnControlMessage;
        private void InvokeOnControlMessage(TikTokLiveClient sender, ControlMessage val) => OnControlMessage?.Invoke(sender, val);
        
        /// <summary>
        /// Event fired when the Host pauses the stream.
        /// </summary>
        public event TikTokEventHandler<ControlMessage> OnLivePaused;
        private void InvokeOnLivePaused(TikTokLiveClient sender, ControlMessage val) => OnLivePaused?.Invoke(sender, val);

        /// <summary>
        /// Event fired when the Host ends the stream.
        /// </summary>
        public event TikTokEventHandler<ControlMessage> OnLiveEnded;
        private void InvokeOnLiveEnded(TikTokLiveClient sender, ControlMessage val) => OnLiveEnded?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a ControlMessage was received that could not be properly interpreted.
        /// <para>
        /// It's up to you how to interpret this message.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<TikTokLiveSharp.Models.Protobuf.Messages.ControlMessage> OnUnhandledControlMessage;
        private void InvokeOnUnhandledControlMessage(TikTokLiveClient sender, TikTokLiveSharp.Models.Protobuf.Messages.ControlMessage val) => OnUnhandledControlMessage?.Invoke(sender, val);
        
        public event TikTokEventHandler<EmoteChat> OnEmoteChat;
        private void InvokeOnEmoteChat(TikTokLiveClient sender, EmoteChat val) => OnEmoteChat?.Invoke(sender, val);

        public event TikTokEventHandler<Envelope> OnEnvelope;
        private void InvokeOnEnvelope(TikTokLiveClient sender, Envelope val) => OnEnvelope?.Invoke(sender, val);

        public event TikTokEventHandler<GameRankNotify> OnGameRankNotify;
        private void InvokeOnGameRankNotify(TikTokLiveClient sender, GameRankNotify val) => OnGameRankNotify?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a viewer sends a Gift to the Host.
        /// <para>
        /// Fires when a Gift-Streak starts. Gift-Events are fired for subsequent messages.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<TikTokGift> OnGift;
        private void InvokeOnGift(TikTokLiveClient sender, TikTokGift val) => OnGift?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a viewer sends a Gift to the Host.
        /// <para>
        /// Fires for every GiftMessage, including the StreakEnd-Message.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<GiftMessage> OnGiftMessage;
        private void InvokeOnGiftMessage(TikTokLiveClient sender, GiftMessage val) => OnGiftMessage?.Invoke(sender, val);

        public event TikTokEventHandler<GiftPrompt> OnGiftPrompt;
        private void InvokeOnGiftPrompt(TikTokLiveClient sender, GiftPrompt val) => OnGiftPrompt?.Invoke(sender, val);

        public event TikTokEventHandler<GoalUpdate> OnGoalUpdate;
        private void InvokeOnGoalUpdate(TikTokLiveClient sender, GoalUpdate val) => OnGoalUpdate?.Invoke(sender, val);

        public event TikTokEventHandler<ImDelete> OnImDelete;
        private void InvokeOnImDelete(TikTokLiveClient sender, ImDelete val) => OnImDelete?.Invoke(sender, val);

        public event TikTokEventHandler<InRoomBannerMessage> OnInRoomBannerMessage;
        private void InvokeOnInRoomBannerMessage(TikTokLiveClient sender, InRoomBannerMessage val) => OnInRoomBannerMessage?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a user Joins the stream (as a viewer).
        /// </summary>
        public event TikTokEventHandler<Like> OnLike;
        private void InvokeOnLike(TikTokLiveClient sender, Like val) => OnLike?.Invoke(sender, val);

        public event TikTokEventHandler<LinkLayerMessage> OnLinkLayerMessage;
        private void InvokeOnLinkLayerMessage(TikTokLiveClient sender, LinkLayerMessage val) => OnLinkLayerMessage?.Invoke(sender, val);

        public event TikTokEventHandler<LinkMessage> OnLinkMessage;
        private void InvokeOnLinkMessage(TikTokLiveClient sender, LinkMessage val) => OnLinkMessage?.Invoke(sender, val);

        public event TikTokEventHandler<LinkMicFanTicketMethod> OnLinkMicFanTicketMethod;
        private void InvokeOnLinkMicFanTicketMethod(TikTokLiveClient sender, LinkMicFanTicketMethod val) => OnLinkMicFanTicketMethod?.Invoke(sender, val);

        public event TikTokEventHandler<LinkMicMethod> OnLinkMicMethod;
        private void InvokeOnLinkMicMethod(TikTokLiveClient sender, LinkMicMethod val) => OnLinkMicMethod?.Invoke(sender, val);

        public event TikTokEventHandler<LinkState> OnLinkState;
        private void InvokeOnLinkState(TikTokLiveClient sender, LinkState val) => OnLinkState?.Invoke(sender, val);

        /// <summary>
        /// Event fired for the Host's Intro-Message for the stream.
        /// </summary>
        public event TikTokEventHandler<LiveIntro> OnLiveIntro;
        private void InvokeOnLiveIntro(TikTokLiveClient sender, LiveIntro val) => OnLiveIntro?.Invoke(sender, val);

        public event TikTokEventHandler<MarqueeAnnouncement> OnMarqueeAnnouncement;
        private void InvokeOnMarqueeAnnouncement(TikTokLiveClient sender, MarqueeAnnouncement val) => OnMarqueeAnnouncement?.Invoke(sender, val);

        /// <summary>
        /// Event fired for each MemberMessage-Event.
        /// <para>
        /// Includes Join- & Subscribe-Messages, among others. Known Sub-Types will fire their own Events as well.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<MemberMessage> OnMemberMessage;
        private void InvokeOnMemberMessage(TikTokLiveClient sender, MemberMessage val) => OnMemberMessage?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a MemberMessage was received that could not be properly interpreted.
        /// <para>
        /// It's up to you how to interpret this message.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<TikTokLiveSharp.Models.Protobuf.Messages.MemberMessage> OnUnhandledMemberMessage;
        private void InvokeOnUnhandledMemberMessage(TikTokLiveClient sender, TikTokLiveSharp.Models.Protobuf.Messages.MemberMessage val) => OnUnhandledMemberMessage?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a viewer joins the stream.
        /// </summary>
        public event TikTokEventHandler<Join> OnJoin;
        private void InvokeOnJoin(TikTokLiveClient sender, Join val) => OnJoin?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a viewer Subscribed to the Host.
        /// </summary>
        public event TikTokEventHandler<Subscribe> OnSubscribe;
        private void InvokeOnSubscribe(TikTokLiveClient sender, Subscribe val) => OnSubscribe?.Invoke(sender, val);

        public event TikTokEventHandler<MsgDetect> OnMsgDetect;
        private void InvokeOnMsgDetect(TikTokLiveClient sender, MsgDetect val) => OnMsgDetect?.Invoke(sender, val);

        public event TikTokEventHandler<Notice> OnNotice;
        private void InvokeOnNotice(TikTokLiveClient sender, Notice val) => OnNotice?.Invoke(sender, val);

        public event TikTokEventHandler<Notify> OnNotify;
        private void InvokeOnNotify(TikTokLiveClient sender, Notify val) => OnNotify?.Invoke(sender, val);

        public event TikTokEventHandler<PartnershipDropsUpdate> OnPartnershipDropsUpdate;
        private void InvokeOnPartnershipDropsUpdate(TikTokLiveClient sender, PartnershipDropsUpdate val) => OnPartnershipDropsUpdate?.Invoke(sender, val);

        public event TikTokEventHandler<PartnershipGameOffline> OnPartnershipGameOffline;
        private void InvokeOnPartnershipGameOffline(TikTokLiveClient sender, PartnershipGameOffline val) => OnPartnershipGameOffline?.Invoke(sender, val);

        public event TikTokEventHandler<PartnershipPunish> OnPartnershipPunish;
        private void InvokeOnPartnershipPunish(TikTokLiveClient sender, PartnershipPunish val) => OnPartnershipPunish?.Invoke(sender, val);

        public event TikTokEventHandler<Perception> OnPerception;
        private void InvokeOnPerception(TikTokLiveClient sender, Perception val) => OnPerception?.Invoke(sender, val);

        public event TikTokEventHandler<Poll> OnPoll;
        private void InvokeOnPoll(TikTokLiveClient sender, Poll val) => OnPoll?.Invoke(sender, val);

        public event TikTokEventHandler<RankText> OnRankText;
        private void InvokeOnRankText(TikTokLiveClient sender, RankText val) => OnRankText?.Invoke(sender, val);

        public event TikTokEventHandler<RankUpdate> OnRankUpdate;
        private void InvokeOnRankUpdate(TikTokLiveClient sender, RankUpdate val) => OnRankUpdate?.Invoke(sender, val);

        public event TikTokEventHandler<RoomMessage> OnRoomMessage;
        private void InvokeOnRoomMessage(TikTokLiveClient sender, RoomMessage val) => OnRoomMessage?.Invoke(sender, val);

        /// <summary>
        /// Event fired when the Room updates its State.
        /// <para>
        /// This event runs intermittently and contains stats such as ViewerCount.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<RoomUpdate> OnRoomUpdate;
        private void InvokeOnRoomUpdate(TikTokLiveClient sender, RoomUpdate val) => OnRoomUpdate?.Invoke(sender, val);

        public event TikTokEventHandler<RoomVerify> OnRoomVerify;
        private void InvokeOnRoomVerify(TikTokLiveClient sender, RoomVerify val) => OnRoomVerify?.Invoke(sender, val);

        /// <summary>
        /// Event fired for each SocialMessage-Event.
        /// <para>
        /// Includes Follow-, Like-, Share- & Join-Messages, among others. Known Sub-Types will fire their own Events as well.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<SocialMessage> OnSocialMessage;
        private void InvokeOnSocialMessage(TikTokLiveClient sender, SocialMessage val) => OnSocialMessage?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a viewer followed the Host.
        /// </summary>
        public event TikTokEventHandler<Follow> OnFollow;
        private void InvokeOnFollow(TikTokLiveClient sender, Follow val) => OnFollow?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a viewer shared the stream.
        /// </summary>
        public event TikTokEventHandler<Share> OnShare;
        private void InvokeOnShare(TikTokLiveClient sender, Share val) => OnShare?.Invoke(sender, val);

        /// <summary>
        /// Event fired when a SocialMessage was received that could not be properly interpreted.
        /// <para>
        /// It's up to you how to interpret this message.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<TikTokLiveSharp.Models.Protobuf.Messages.SocialMessage> OnUnhandledSocialMessage;
        private void InvokeOnUnhandledSocialMessage(TikTokLiveClient sender, TikTokLiveSharp.Models.Protobuf.Messages.SocialMessage val) => OnUnhandledSocialMessage?.Invoke(sender, val);

        public event TikTokEventHandler<Speaker> OnSpeaker;
        private void InvokeOnSpeaker(TikTokLiveClient sender, Speaker val) => OnSpeaker?.Invoke(sender, val);

        public event TikTokEventHandler<SubCapsule> OnSubCapsule;
        private void InvokeOnSubCapsule(TikTokLiveClient sender, SubCapsule val) => OnSubCapsule?.Invoke(sender, val);

        public event TikTokEventHandler<SubNotify> OnSubNotify;
        private void InvokeOnSubNotify(TikTokLiveClient sender, SubNotify val) => OnSubNotify?.Invoke(sender, val);

        public event TikTokEventHandler<SubPinEvent> OnSubPinEvent;
        private void InvokeOnSubPinEvent(TikTokLiveClient sender, SubPinEvent val) => OnSubPinEvent?.Invoke(sender, val);

        public event TikTokEventHandler<SubscriptionNotify> OnSubscriptionNotify;
        private void InvokeOnSubscriptionNotify(TikTokLiveClient sender, SubscriptionNotify val) => OnSubscriptionNotify?.Invoke(sender, val);

        public event TikTokEventHandler<Toast> OnToast;
        private void InvokeOnToast(TikTokLiveClient sender, Toast val) => OnToast?.Invoke(sender, val);

        public event TikTokEventHandler<UnauthorizedMember> OnUnauthorizedMember;
        private void InvokeOnUnauthorizedMember(TikTokLiveClient sender, UnauthorizedMember val) => OnUnauthorizedMember?.Invoke(sender, val);

        /// <summary>
        /// Event fired for a ServerMessage that could not be parsed into a known object.
        /// <para>
        /// It's up to you how to interpret this message.
        /// </para>
        /// </summary>
        public event TikTokEventHandler<TikTokLiveSharp.Models.Protobuf.Messages.Message> OnUnhandled;
        private void InvokeOnUnhandled(TikTokLiveClient sender, TikTokLiveSharp.Models.Protobuf.Messages.Message val) => OnUnhandled?.Invoke(sender, val);


        // The events below are less well understood.
        // They are missing a lot of variable-names & -Types (especially Enums).
        public event TikTokEventHandler<GiftBroadcast> OnGiftBroadcast;
        private void InvokeOnGiftBroadcast(TikTokLiveClient sender, GiftBroadcast val) => OnGiftBroadcast?.Invoke(sender, val);

        public event TikTokEventHandler<HourlyRank> OnHourlyRank;
        private void InvokeOnHourlyRank(TikTokLiveClient sender, HourlyRank val) => OnHourlyRank?.Invoke(sender, val);

        public event TikTokEventHandler<LinkMicArmies> OnLinkMicArmies;
        private void InvokeOnLinkMicArmies(TikTokLiveClient sender, LinkMicArmies val) => OnLinkMicArmies?.Invoke(sender, val);

        public event TikTokEventHandler<LinkMicBattle> OnLinkMicBattle;
        private void InvokeOnLinkMicBattle(TikTokLiveClient sender, LinkMicBattle val) => OnLinkMicBattle?.Invoke(sender, val);

        public event TikTokEventHandler<LinkMicBattlePunishFinish> OnLinkMicBattlePunishFinish;
        private void InvokeOnLinkMicBattlePunishFinish(TikTokLiveClient sender, LinkMicBattlePunishFinish val) => OnLinkMicBattlePunishFinish?.Invoke(sender, val);

        public event TikTokEventHandler<LinkmicBattleTask> OnLinkmicBattleTask;
        private void InvokeOnLinkmicBattleTask(TikTokLiveClient sender, LinkmicBattleTask val) => OnLinkmicBattleTask?.Invoke(sender, val);

        public event TikTokEventHandler<OecLiveShopping> OnOecLiveShopping;
        private void InvokeOnOecLiveShopping(TikTokLiveClient sender, OecLiveShopping val) => OnOecLiveShopping?.Invoke(sender, val);

        public event TikTokEventHandler<QuestionNew> OnQuestionNew;
        private void InvokeOnQuestionNew(TikTokLiveClient sender, QuestionNew val) => OnQuestionNew?.Invoke(sender, val);

        /// <summary>
        /// Event fired when the Host pins a message to the stream?
        /// </summary>
        public event TikTokEventHandler<RoomPinMessage> OnRoomPinMessage;
        private void InvokeOnRoomPinMessage(TikTokLiveClient sender, RoomPinMessage val) => OnRoomPinMessage?.Invoke(sender, val);

        public event TikTokEventHandler<SystemMessage> OnSystemMessage;
        private void InvokeOnSystemMessage(TikTokLiveClient sender, SystemMessage val) => OnSystemMessage?.Invoke(sender, val);
        #endregion

        #region UnityEvents
        /// <summary>
        /// Event fired if an Exception occurred.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired if an Exception occurred.")]
        private UnityEvent<Exception> onException;
        /// <summary>
        /// Event fired when the client connects.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when the client connects.")]
        private UnityEvent<bool> onConnected;
        /// <summary>
        /// Event fired when the client disconnects.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when the client disconnects.")]
        private UnityEvent<bool> onDisconnected;
        [SerializeField]
        private UnityEvent<AccessControl> onAccessControl;
        [SerializeField]
        private UnityEvent<AccessRecall> onAccessRecall;
        [SerializeField]
        private UnityEvent<AlertBoxAuditResult> onAlertBoxAuditResult;
        [SerializeField]
        private UnityEvent<Barrage> onBarrage;
        [SerializeField]
        private UnityEvent<BindingGift> onBindingGift;
        [SerializeField]
        private UnityEvent<BoostCardMessage> onBoostCardMessage;
        [SerializeField]
        private UnityEvent<BottomMessage> onBottomMessage;
        /// <summary>
        /// Closed Caption-Message for Video.
        /// </summary>
        [SerializeField]
        [Tooltip("Closed Caption-Message for Video.")]
        private UnityEvent<Caption> onCaption;
        /// <summary>
        /// Event fired when a viewer Commented on the stream.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a viewer Commented on the stream.")]
        private UnityEvent<Chat> onChatMessage;
        /// <summary>
        /// Event fired when the Host changed the stream's Status (e.g. Stream Ended).
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when the Host changed the stream's Status (e.g. Stream Ended).")]
        private UnityEvent<ControlMessage> onControlMessage;
        /// <summary>
        /// Event fired when the Host pauses the stream.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when the Host pauses the stream.")]
        private UnityEvent<ControlMessage> onLivePaused;
        /// <summary>
        /// Event fired when the Host ends the stream.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when the Host ends the stream.")]
        private UnityEvent<ControlMessage> onLiveEnded;
        /// <summary>
        /// Event fired when a ControlMessage was received that could not be properly interpreted.
        /// <para>
        /// It's up to you how to interpret this message.
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a ControlMessage was received that could not be properly interpreted.")]
        private UnityEvent<TikTokLiveSharp.Models.Protobuf.Messages.ControlMessage> onUnhandledControlMessage;
        [SerializeField]
        private UnityEvent<EmoteChat> onEmoteChat;
        [SerializeField]
        private UnityEvent<Envelope> onEnvelope;
        [SerializeField]
        private UnityEvent<GameRankNotify> onGameRankNotify;
        /// <summary>
        /// Event fired when a viewer sends a Gift to the Host.
        /// <para>
        /// Fires when a Gift-Streak starts. Gift-Events are fired for subsequent messages.
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a viewer sends a Gift to the Host. (First Message)")]
        private UnityEvent<TikTokGift> onGift;
        /// <summary>
        /// Event fired when a viewer sends a Gift to the Host.
        /// <para>
        /// Fires for every GiftMessage, including the StreakEnd-Message.
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a viewer sends a Gift to the Host. (Every Message)")]
        private UnityEvent<GiftMessage> onGiftMessage;
        [SerializeField]
        private UnityEvent<GiftPrompt> onGiftPrompt;
        [SerializeField]
        private UnityEvent<GoalUpdate> onGoalUpdate;
        [SerializeField]
        private UnityEvent<ImDelete> onImDelete;
        [SerializeField]
        private UnityEvent<InRoomBannerMessage> onInRoomBannerMessage;
        /// <summary>
        /// Event fired when a user Joins the stream (as a viewer).
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a user Joins the stream (as a viewer).")]
        private UnityEvent<Like> onLike;
        [SerializeField]
        private UnityEvent<LinkLayerMessage> onLinkLayerMessage;
        [SerializeField]
        private UnityEvent<LinkMessage> onLinkMessage;
        [SerializeField]
        private UnityEvent<LinkMicFanTicketMethod> onLinkMicFanTicketMethod;
        [SerializeField]
        private UnityEvent<LinkMicMethod> onLinkMicMethod;
        [SerializeField]
        private UnityEvent<LinkState> onLinkState;
        /// <summary>
        /// Event fired for the Host's Intro-Message for the stream.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired for the Host's Intro-Message for the stream.")]
        private UnityEvent<LiveIntro> onLiveIntro;
        [SerializeField]
        private UnityEvent<MarqueeAnnouncement> onMarqueeAnnouncement;
        /// <summary>
        /// Event fired for each MemberMessage-Event.
        /// <para>
        /// Includes Join- & Subscribe-Messages, among others. Known Sub-Types will fire their own Events as well.
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired for each MemberMessage-Event.")]
        private UnityEvent<MemberMessage> onMemberMessage;
        /// <summary>
        /// Event fired when a MemberMessage was received that could not be properly interpreted.
        /// <para>
        /// It's up to you how to interpret this message.
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a MemberMessage was received that could not be properly interpreted.")]
        private UnityEvent<TikTokLiveSharp.Models.Protobuf.Messages.MemberMessage> onUnhandledMemberMessage;
        /// <summary>
        /// Event fired when a user joins the stream as a viewer.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a user joins the stream as a viewer.")]
        private UnityEvent<Join> onJoin;
        /// <summary>
        /// Event fired when a viewer Subscribed to the Host.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a viewer Subscribed to the Host.")]
        private UnityEvent<Subscribe> onSubscribe;
        [SerializeField]
        private UnityEvent<MsgDetect> onMsgDetect;
        [SerializeField]
        private UnityEvent<Notice> onNotice;
        [SerializeField]
        private UnityEvent<Notify> onNotify;
        [SerializeField]
        private UnityEvent<PartnershipDropsUpdate> onPartnershipDropsUpdate;
        [SerializeField]
        private UnityEvent<PartnershipGameOffline> onPartnershipGameOffline;
        [SerializeField]
        private UnityEvent<PartnershipPunish> onPartnershipPunish;
        [SerializeField]
        private UnityEvent<Perception> onPerception;
        [SerializeField]
        private UnityEvent<Poll> onPoll;
        [SerializeField]
        private UnityEvent<RankText> onRankText;
        [SerializeField]
        private UnityEvent<RankUpdate> onRankUpdate;
        [SerializeField]
        private UnityEvent<RoomMessage> onRoomMessage;
        /// <summary>
        /// Event fired when the Room updates its State.
        /// <para>
        /// This event runs intermittently and contains stats such as ViewerCount.
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when the Room updates its State.")]
        private UnityEvent<RoomUpdate> onRoomUpdate;
        [SerializeField]
        private UnityEvent<RoomVerify> onRoomVerify;
        /// <summary>
        /// Event fired for each SocialMessage-Event.
        /// <para>
        /// Includes Follow-, Like-, Share- & Join-Messages, among others. Known Sub-Types will fire their own Events as well.
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired for each SocialMessage-Event.")]
        private UnityEvent<SocialMessage> onSocialMessage;
        /// <summary>
        /// Event fired when a viewer followed the Host.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a viewer followed the Host.")]
        private UnityEvent<Follow> onFollow;
        /// <summary>
        /// Event fired when a viewer shared the stream.
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a viewer shared the stream.")]
        private UnityEvent<Share> onShare;
        /// <summary>
        /// Event fired when a SocialMessage was received that could not be properly interpreted.
        /// <para>
        /// It's up to you how to interpret this message.
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when a SocialMessage was received that could not be properly interpreted.")]
        private UnityEvent<TikTokLiveSharp.Models.Protobuf.Messages.SocialMessage> onUnhandledSocialMessage;
        [SerializeField]
        private UnityEvent<Speaker> onSpeaker;
        [SerializeField]
        private UnityEvent<SubCapsule> onSubCapsule;
        [SerializeField]
        private UnityEvent<SubNotify> onSubNotify;
        [SerializeField]
        private UnityEvent<SubPinEvent> onSubPinEvent;
        [SerializeField]
        private UnityEvent<SubscriptionNotify> onSubscriptionNotify;
        [SerializeField]
        private UnityEvent<Toast> onToast;
        [SerializeField] 
        private UnityEvent<UnauthorizedMember> onUnauthorizedMember;
        /// <summary>
        /// Event fired for a ServerMessage that could not be parsed into a known object.
        /// <para>
        /// It's up to you how to interpret this message.
        /// </para>
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired for a ServerMessage that could not be parsed into a known object.")]
        private UnityEvent<TikTokLiveSharp.Models.Protobuf.Messages.Message> onUnhandled;

        // The events below are less well understood.
        // They are missing a lot of variable-names & -Types (especially Enums).
        [SerializeField]
        private UnityEvent<GiftBroadcast> onGiftBroadcast;
        [SerializeField]
        private UnityEvent<HourlyRank> onHourlyRank;
        [SerializeField]
        private UnityEvent<LinkMicArmies> onLinkMicArmies;
        [SerializeField]
        private UnityEvent<LinkMicBattle> onLinkMicBattle;
        [SerializeField]
        private UnityEvent<LinkMicBattlePunishFinish> onLinkMicBattlePunishFinish;
        [SerializeField]
        private UnityEvent<LinkmicBattleTask> onLinkmicBattleTask;
        [SerializeField]
        private UnityEvent<OecLiveShopping> onOecLiveShopping;
        [SerializeField]
        private UnityEvent<QuestionNew> onQuestionNew;
        /// <summary>
        /// Event fired when the Host pins a message to the stream?
        /// </summary>
        [SerializeField]
        [Tooltip("Event fired when the Host pins a message to the stream?")]
        private UnityEvent<RoomPinMessage> onRoomPinMessage;
        [SerializeField]
        private UnityEvent<SystemMessage> onSystemMessage;
        #endregion
        #endregion
        #endregion

        #region Methods
        #region Static
        /// <summary>
        /// Checks if a User exists on TikTok by attempting to get their Profile-Page
        /// </summary>
        /// <param name="userId">@-ID for User</param>
        /// <param name="timeOut">TimeOut for HTTP-Connection (set NULL for default)</param>
        /// <param name="proxy">Proxy to use with HTTP-Client</param>
        /// <returns>True if User has a Profile-Page on TikTok</returns>
        public static async Task<bool> GetUserExists(string userId, TimeSpan? timeOut = null, IWebProxy proxy = null) => await TikTokBaseClient.GetUserExists(userId, timeOut, proxy);

        /// <summary>
        /// Checks if a User is currently streaming by looking for a RoomId on their Live-Page
        /// </summary>
        /// <param name="userId">@-ID for User</param>
        /// <param name="timeOut">TimeOut for HTTP-Connection (set NULL for default)</param>
        /// <param name="proxy">Proxy to use with HTTP-Client</param>
        /// <returns>True if User is currently streaming on TikTok</returns>
        public static async Task<bool> GetUserStreaming(string userId, TimeSpan? timeOut = null, IWebProxy proxy = null) => await TikTokBaseClient.GetUserStreaming(userId, timeOut, proxy);
        #endregion

        #region Public
        #region Connection
        /// <summary>
        /// Connects to a LiveStream
        /// This can be called from the Unity Main Thread
        /// <para>
        ///     NOTE: Either <paramref name="userId"/> or <paramref name="roomId"/> must be set!
        /// </para>
        /// </summary>
        /// <param name="userId">UserName for Host of Room</param>
        /// <param name="roomId">Id for Room to connect to</param>
        /// <param name="onConnectException">Callback for Exception whilst Connecting</param>
        /// <param name="clientParams">Custom parameters for <see cref="TikTokLiveClient"/></param>
        /// <returns>Task to Await</returns>
        public async void ConnectAsync(string userId, string roomId, Action<Exception> onConnectException = null, Dictionary<string, object> clientParams = null)
        {
            await Connect(userId, roomId, onConnectException);
        }

        /// <summary>
        /// Connects to a LiveStream
        /// Returns Task to Await
        /// <para>
        ///     NOTE: Either <paramref name="userId"/> or <paramref name="roomId"/> must be set!
        /// </para>
        /// </summary>
        /// <param name="userId">UserName for Host of Room</param>
        /// <param name="roomId">Id for Room to connect to</param>
        /// <param name="onConnectException">Callback for Exception whilst Connecting</param>
        /// <param name="clientParams">Custom parameters for <see cref="TikTokLiveClient"/></param>
        /// <returns>Task to Await</returns>
        public async Task Connect(string userId, string roomId, Action<Exception> onConnectException = null, Dictionary<string, object> clientParams = null)
        {
            if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(roomId))
            {
                if (ShouldLog(LogLevel.Error))
                    Debug.LogError("No UserID Provided");
                throw new ArgumentNullException(nameof(userId), "Did not provide a UserId or RoomId to connect to");
            }
            try
            {
                if (client != null)
                {
                    if (ShouldLog(LogLevel.Information))
                        Debug.Log("Disconnecting Existing Client", gameObject);
                    await DisconnectFromLivestream();
                }
                if (ShouldLog(LogLevel.Verbose))
                    Debug.Log("Creating new TikTokLiveClient");
                client = new TikTokLiveClient(userId, roomId, settings, null);
                if (ShouldLog(LogLevel.Information))
                    Debug.Log(string.IsNullOrWhiteSpace(userId)
                        ? $"Created new Client with RoomId {roomId}"
                        : $"Created new Client with UserId {userId}");
                // Set up Events for Client
                if (ShouldLog(LogLevel.Verbose))
                    Debug.Log("Connecting Events to Client");
                SetupEvents(client);
            }
            catch (Exception e)
            {
                if (ShouldLog(LogLevel.Error))
                    Debug.LogException(e, gameObject);
                onConnectException?.Invoke(e);
                return;
            }
            try
            {
                await client.Start(tokenSource.Token, onConnectException, settings.RetryOnConnectionFailure);
                if (ShouldLog(LogLevel.Verbose))
                    Debug.Log("Connected");
            }
            catch (Exception e)
            {
                if (ShouldLog(LogLevel.Error))
                    Debug.LogException(e, gameObject);
                onConnectException?.Invoke(e);
            }
        }

        /// <summary>
        /// Connects to a LiveStream
        /// This can be called from the Unity Main Thread
        /// </summary>
        /// <param name="userId">UserName for Host of Room</param>
        /// <param name="onConnectException">Callback for Exception whilst Connecting</param>
        /// <param name="clientParams">Custom parameters for <see cref="TikTokLiveClient"/></param>
        public async void ConnectToStreamAsync(string userId, Action<Exception> onConnectException = null, Dictionary<string, object> clientParams = null) => await ConnectToStream(userId, onConnectException, clientParams);

        /// <summary>
        /// Connects to a LiveStream
        /// Returns Task to Await
        /// </summary>
        /// <param name="userId">UserName for Host of Room</param>
        /// <param name="onConnectException">Callback for Exception whilst Connecting</param>
        /// <param name="clientParams">Custom parameters for <see cref="TikTokLiveClient"/></param>
        /// <returns>Task to Await</returns>
        public async Task ConnectToStream(string userId, Action<Exception> onConnectException = null, Dictionary<string, object> clientParams = null) => await Connect(userId, null, onConnectException, clientParams);

        /// <summary>
        /// Connects to a LiveStream
        /// This can be called from the Unity Main Thread
        /// </summary>
        /// <param name="roomId">Id for Room to connect to</param>
        /// <param name="onConnectException">Callback for Exception whilst Connecting</param>
        /// <param name="clientParams">Custom parameters for <see cref="TikTokLiveClient"/></param>
        /// <returns>Task to Await</returns>
        public async void ConnectToRoomAsync(string roomId, Action<Exception> onConnectException = null, Dictionary<string, object> clientParams = null) => await ConnectToRoom(roomId, onConnectException, clientParams);

        /// <summary>
        /// Connects to a LiveStream
        /// Returns Task to Await
        /// </summary>
        /// <param name="roomId">Id for Room to connect to</param>
        /// <param name="onConnectException">Callback for Exception whilst Connecting</param>
        /// <param name="clientParams">Custom parameters for <see cref="TikTokLiveClient"/></param>
        /// <returns>Task to Await</returns>
        public async Task ConnectToRoom(string roomId, Action<Exception> onConnectException = null, Dictionary<string, object> clientParams = null) => await Connect(null, roomId, onConnectException, clientParams);

        /// <summary>
        /// Disconnects from current LiveStream
        /// This can be called from the Unity Main Thread
        /// </summary>
        public async void DisconnectFromLivestreamAsync() => await DisconnectFromLivestream();

        /// <summary>
        /// Disconnects from current LiveStream
        /// Returns Task to Await
        /// </summary>
        /// <returns>Task to Await</returns>
        public async Task DisconnectFromLivestream()
        {
            if (client != null)
            {
                if (ShouldLog(LogLevel.Information))
                    Debug.Log("Disconnecting Client");
                await client.Stop();
                if (ShouldLog(LogLevel.Verbose))
                    Debug.Log("Removing EventListeners from Client");
                TearDownEvents(client);
                client = null;
            }
            await Task.Delay(200);
            if (ShouldLog(LogLevel.Verbose))
                Debug.Log("Stopping Threads");
            tokenSource?.Cancel();
            tokenSource = new CancellationTokenSource();
        }
        #endregion

        #region Images
        /// <summary>
        /// Gets Texture from TikTok
        /// </summary>
        /// <param name="picture">Picture-Object for Texture</param>
        /// <param name="onComplete">Callback for Texture-Retrieval</param>
        public void RequestImage(Picture picture, Action<Texture2D> onComplete) => RequestImage(picture.Urls, onComplete);
        /// <summary>
        /// Gets Texture from TikTok
        /// </summary>
        /// <param name="urls">URLs for Texture. First one with valid Extension is used</param>
        /// <param name="onComplete">Callback for Texture-Retrieval</param>
        public void RequestImage(IEnumerable<string> urls, Action<Texture2D> onComplete) => textureCache.RequestImage(urls, onComplete);
        /// <summary>
        /// Gets Texture from TikTok
        /// </summary>
        /// <param name="url">URL for Texture</param>
        /// <param name="onComplete">Callback for Texture-Retrieval</param>
        public void RequestImage(string url, Action<Texture2D> onComplete) => textureCache.RequestImage(url, onComplete);

        /// <summary>
        /// Gets Sprite from TikTok
        /// </summary>
        /// <param name="picture">Picture-Object for Sprite</param>
        /// <param name="onComplete">Callback for Sprite-Retrieval</param>
        public void RequestSprite(Picture picture, Action<Sprite> onComplete) => RequestSprite(picture.Urls, onComplete);
        /// <summary>
        /// Gets Sprite from TikTok
        /// </summary>
        /// <param name="urls">URLs for Sprite. First one with valid Extension is used</param>
        /// <param name="onComplete">Callback for Sprite-Retrieval</param>
        public void RequestSprite(IEnumerable<string> urls, Action<Sprite> onComplete) => textureCache.RequestSprite(urls, onComplete);
        /// <summary>
        /// Gets Sprite from TikTok
        /// </summary>
        /// <param name="url">URL for Sprite</param>
        /// <param name="onComplete">Callback for Sprite-Retrieval</param>
        public void RequestSprite(string url, Action<Sprite> onComplete) => textureCache.RequestSprite(url, onComplete);
        #endregion
        #endregion

        #region Unity
        /// <summary>
        /// Initializes Manager
        /// </summary>
        protected virtual void Awake()
        {
            // Destroy any duplicate Instance
            if (Exists && instance != this)
            {
                if ((instance.settings.PrintToConsole && instance.settings.LogLevel.HasFlag(LogLevel.Error))
                    || settings.PrintToConsole && settings.LogLevel.HasFlag(LogLevel.Error))
                    Debug.LogError($"TikTokLiveManager already exists! Existing Object: {instance.gameObject.name}. New Object: {gameObject.name}. Destroying new Object", instance.gameObject);
                DestroyImmediate(gameObject, false);
                return;
            }
            #region Singleton
            if (transform.parent != null)
            {
                if (!hasRootObject)
                {
                    if (ShouldLog(LogLevel.Warning))
                        Debug.LogWarning("TikTokLiveManager is not a Root-Object. Did you mean to set hasRootObject? Unparenting", gameObject);
                    transform.SetParent(null, true);
                    DontDestroyOnLoad(gameObject);
                }
                else DontDestroyOnLoad(transform.root.gameObject);
            }
            else DontDestroyOnLoad(gameObject);
            // Set Instance
            if (ShouldLog(LogLevel.Verbose))
                Debug.Log("Setting Singleton-Instance");
            instance = this;
            #endregion
            #region Init
            if (ShouldLog(LogLevel.Verbose))
                Debug.Log("Creating TextureCache");
            textureCache = new TextureCache(texCacheSize);
            if (ShouldLog(LogLevel.Verbose))
                Debug.Log("Linking UnityEvents to C#-Events");
            LinkUnityEvents();
            #endregion
        }
        /// <summary>
        /// Performs Auto-Connect
        /// </summary>
        protected virtual IEnumerator Start()
        {
            if (!autoConnect) 
                yield break;
            yield return null;
            if (ShouldLog(LogLevel.Information))
                Debug.Log($"Auto-Connecting to {autoConnectHostId}");
            ConnectToStreamAsync(autoConnectHostId, null);
        }
        /// <summary>
        /// Sets default Settings
        /// </summary>
        protected virtual void Reset()
        {
            hasRootObject = transform.parent != null;
            texCacheSize = 2048;
            autoConnect = false;
            autoConnectHostId = string.Empty;
            settings = Constants.DEFAULT_SETTINGS;
        }
        /// <summary>
        /// DeInitializes Manager. Closes any Active Connection
        /// </summary>
        protected virtual void OnDestroy()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DisconnectFromLivestream();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            if (Exists && ReferenceEquals(instance, this))
            {
                #region DeInit
                if (ShouldLog(LogLevel.Verbose))
                    Debug.Log("Clearing TextureCache");
                textureCache = null;
                Resources.UnloadUnusedAssets();
                #endregion
                if (ShouldLog(LogLevel.Verbose))
                    Debug.Log("Removing Singleton-Instance");
                instance = null;
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// Links UnityEvents to C#-Events
        /// </summary>
        private void LinkUnityEvents()
        {
            OnException += (_, args) => onException?.Invoke(args);
            OnConnected += (_, args) => onConnected?.Invoke(args);
            OnDisconnected += (_, args) => onDisconnected?.Invoke(args);
            OnAccessControl += (_, args) => onAccessControl?.Invoke(args);
            OnAccessRecall += (_, args) => onAccessRecall?.Invoke(args);
            OnAlertBoxAuditResult += (_, args) => onAlertBoxAuditResult?.Invoke(args);
            OnBarrage += (_, args) => onBarrage?.Invoke(args);
            OnBindingGift += (_, args) => onBindingGift?.Invoke(args);
            OnBoostCardMessage += (_, args) => onBoostCardMessage?.Invoke(args);
            OnBottomMessage += (_, args) => onBottomMessage?.Invoke(args);
            OnCaption += (_, args) => onCaption?.Invoke(args);
            OnChatMessage += (_, args) => onChatMessage?.Invoke(args);
            OnControlMessage += (_, args) => onControlMessage?.Invoke(args);
            OnLivePaused += (_, args) => onLivePaused?.Invoke(args);
            OnLiveEnded += (_, args) => onLiveEnded?.Invoke(args);
            OnUnhandledControlMessage += (_, args) => onUnhandledControlMessage?.Invoke(args);
            OnEmoteChat += (_, args) => onEmoteChat?.Invoke(args);
            OnEnvelope += (_, args) => onEnvelope?.Invoke(args);
            OnGameRankNotify += (_, args) => onGameRankNotify?.Invoke(args);
            OnGift += (_, args) => onGift?.Invoke(args);
            OnGiftMessage += (_, args) => onGiftMessage?.Invoke(args);
            OnGiftPrompt += (_, args) => onGiftPrompt?.Invoke(args);
            OnGoalUpdate += (_, args) => onGoalUpdate?.Invoke(args);
            OnImDelete += (_, args) => onImDelete?.Invoke(args);
            OnInRoomBannerMessage += (_, args) => onInRoomBannerMessage?.Invoke(args);
            OnLike += (_, args) => onLike?.Invoke(args);
            OnLinkLayerMessage += (_, args) => onLinkLayerMessage?.Invoke(args);
            OnLinkMessage += (_, args) => onLinkMessage?.Invoke(args);
            OnLinkMicFanTicketMethod += (_, args) => onLinkMicFanTicketMethod?.Invoke(args);
            OnLinkMicMethod += (_, args) => onLinkMicMethod?.Invoke(args);
            OnLinkState += (_, args) => onLinkState?.Invoke(args);
            OnLiveIntro += (_, args) => onLiveIntro?.Invoke(args);
            OnMarqueeAnnouncement += (_, args) => onMarqueeAnnouncement?.Invoke(args);
            OnMemberMessage += (_, args) => onMemberMessage?.Invoke(args);
            OnUnhandledMemberMessage += (_, args) => onUnhandledMemberMessage?.Invoke(args);
            OnJoin += (_, args) => onJoin?.Invoke(args);
            OnSubscribe += (_, args) => onSubscribe?.Invoke(args);
            OnMsgDetect += (_, args) => onMsgDetect?.Invoke(args);
            OnNotice += (_, args) => onNotice?.Invoke(args);
            OnNotify += (_, args) => onNotify?.Invoke(args);
            OnPartnershipDropsUpdate += (_, args) => onPartnershipDropsUpdate?.Invoke(args);
            OnPartnershipGameOffline += (_, args) => onPartnershipGameOffline?.Invoke(args);
            OnPartnershipPunish += (_, args) => onPartnershipPunish?.Invoke(args);
            OnPerception += (_, args) => onPerception?.Invoke(args);
            OnPoll += (_, args) => onPoll?.Invoke(args);
            OnRankText += (_, args) => onRankText?.Invoke(args);
            OnRankUpdate += (_, args) => onRankUpdate?.Invoke(args);
            OnRoomMessage += (_, args) => onRoomMessage?.Invoke(args);
            OnRoomUpdate += (_, args) => onRoomUpdate?.Invoke(args);
            OnRoomVerify += (_, args) => onRoomVerify?.Invoke(args);
            OnSocialMessage += (_, args) => onSocialMessage?.Invoke(args);
            OnUnhandledSocialMessage += (_, args) => onUnhandledSocialMessage?.Invoke(args);
            OnFollow += (_, args) => onFollow?.Invoke(args);
            OnShare += (_, args) => onShare?.Invoke(args);
            OnSpeaker += (_, args) => onSpeaker?.Invoke(args);
            OnSubCapsule += (_, args) => onSubCapsule?.Invoke(args);
            OnSubNotify += (_, args) => onSubNotify?.Invoke(args);
            OnSubPinEvent += (_, args) => onSubPinEvent?.Invoke(args);
            OnSubscriptionNotify += (_, args) => onSubscriptionNotify?.Invoke(args);
            OnToast += (_, args) => onToast?.Invoke(args);
            OnUnauthorizedMember += (_, args) => onUnauthorizedMember?.Invoke(args);
            OnUnhandled += (_, args) => onUnhandled?.Invoke(args);
            OnGiftBroadcast += (_, args) => onGiftBroadcast?.Invoke(args);
            OnHourlyRank += (_, args) => onHourlyRank?.Invoke(args);
            OnLinkMicArmies += (_, args) => onLinkMicArmies?.Invoke(args);
            OnLinkMicBattle += (_, args) => onLinkMicBattle?.Invoke(args);
            OnLinkMicBattlePunishFinish += (_, args) => onLinkMicBattlePunishFinish?.Invoke(args);
            OnLinkmicBattleTask += (_, args) => onLinkmicBattleTask?.Invoke(args);
            OnOecLiveShopping += (_, args) => onOecLiveShopping?.Invoke(args);
            OnQuestionNew += (_, args) => onQuestionNew?.Invoke(args);
            OnRoomPinMessage += (_, args) => onRoomPinMessage?.Invoke(args);
            OnSystemMessage += (_, args) => onSystemMessage?.Invoke(args);
        }
        /// <summary>
        /// Links Events to Client
        /// </summary>
        /// <param name="client">Client to Link to</param>
        private void SetupEvents(TikTokLiveClient client)
        {
            client.OnException += InvokeOnException;
            client.OnConnected += InvokeOnConnected;
            client.OnDisconnected += InvokeOnDisconnected;
            client.OnAccessControl += InvokeOnAccessControl;
            client.OnAccessRecall += InvokeOnAccessRecall;
            client.OnAlertBoxAuditResult += InvokeOnAlertBoxAuditResult;
            client.OnBarrage += InvokeOnBarrage;
            client.OnBindingGift += InvokeOnBindingGift;
            client.OnBoostCardMessage += InvokeOnBoostCardMessage;
            client.OnBottomMessage += InvokeOnBottomMessage;
            client.OnCaption += InvokeOnCaption;
            client.OnChatMessage += InvokeOnChatMessage;
            client.OnControlMessage += InvokeOnControlMessage;
            client.OnLivePaused += InvokeOnLivePaused;
            client.OnLiveEnded += InvokeOnLiveEnded;
            client.OnUnhandledControlMessage += InvokeOnUnhandledControlMessage;
            client.OnEmoteChat += InvokeOnEmoteChat;
            client.OnEnvelope += InvokeOnEnvelope;
            client.OnGameRankNotify += InvokeOnGameRankNotify;
            client.OnGift += InvokeOnGift;
            client.OnGiftMessage += InvokeOnGiftMessage;
            client.OnGiftPrompt += InvokeOnGiftPrompt;
            client.OnGoalUpdate += InvokeOnGoalUpdate;
            client.OnImDelete += InvokeOnImDelete;
            client.OnInRoomBannerMessage += InvokeOnInRoomBannerMessage;
            client.OnLike += InvokeOnLike;
            client.OnLinkLayerMessage += InvokeOnLinkLayerMessage;
            client.OnLinkMessage += InvokeOnLinkMessage;
            client.OnLinkMicFanTicketMethod += InvokeOnLinkMicFanTicketMethod;
            client.OnLinkMicMethod += InvokeOnLinkMicMethod;
            client.OnLinkState += InvokeOnLinkState;
            client.OnLiveIntro += InvokeOnLiveIntro;
            client.OnMarqueeAnnouncement += InvokeOnMarqueeAnnouncement;
            client.OnMemberMessage += InvokeOnMemberMessage;
            client.OnUnhandledMemberMessage += InvokeOnUnhandledMemberMessage;
            client.OnJoin += InvokeOnJoin;
            client.OnSubscribe += InvokeOnSubscribe;
            client.OnMsgDetect += InvokeOnMsgDetect;
            client.OnNotice += InvokeOnNotice;
            client.OnNotify += InvokeOnNotify;
            client.OnPartnershipDropsUpdate += InvokeOnPartnershipDropsUpdate;
            client.OnPartnershipGameOffline += InvokeOnPartnershipGameOffline;
            client.OnPartnershipPunish += InvokeOnPartnershipPunish;
            client.OnPerception += InvokeOnPerception;
            client.OnPoll += InvokeOnPoll;
            client.OnRankText += InvokeOnRankText;
            client.OnRankUpdate += InvokeOnRankUpdate;
            client.OnRoomMessage += InvokeOnRoomMessage;
            client.OnRoomUpdate += InvokeOnRoomUpdate;
            client.OnRoomVerify += InvokeOnRoomVerify;
            client.OnSocialMessage += InvokeOnSocialMessage;
            client.OnUnhandledSocialMessage += InvokeOnUnhandledSocialMessage;
            client.OnFollow += InvokeOnFollow;
            client.OnShare += InvokeOnShare;
            client.OnSpeaker += InvokeOnSpeaker;
            client.OnSubCapsule += InvokeOnSubCapsule;
            client.OnSubNotify += InvokeOnSubNotify;
            client.OnSubPinEvent += InvokeOnSubPinEvent;
            client.OnSubscriptionNotify += InvokeOnSubscriptionNotify;
            client.OnToast += InvokeOnToast;
            client.OnUnauthorizedMember += InvokeOnUnauthorizedMember;
            client.OnUnhandled += InvokeOnUnhandled;
            // Beta Events:
            client.OnGiftBroadcast += InvokeOnGiftBroadcast;
            client.OnHourlyRank += InvokeOnHourlyRank;
            client.OnLinkMicArmies += InvokeOnLinkMicArmies;
            client.OnLinkMicBattle += InvokeOnLinkMicBattle;
            client.OnLinkMicBattlePunishFinish += InvokeOnLinkMicBattlePunishFinish;
            client.OnLinkmicBattleTask += InvokeOnLinkmicBattleTask;
            client.OnOecLiveShopping += InvokeOnOecLiveShopping;
            client.OnQuestionNew += InvokeOnQuestionNew;
            client.OnRoomPinMessage += InvokeOnRoomPinMessage;
            client.OnSystemMessage += InvokeOnSystemMessage;
        }

        /// <summary>
        /// Unlinks Events from Client
        /// </summary>
        /// <param name="client">Client to Unlink from</param>
        private void TearDownEvents(TikTokLiveClient client)
        {
            client.OnException -= InvokeOnException;
            client.OnConnected -= InvokeOnConnected;
            client.OnDisconnected -= InvokeOnDisconnected;
            client.OnAccessControl -= InvokeOnAccessControl;
            client.OnAccessRecall -= InvokeOnAccessRecall;
            client.OnAlertBoxAuditResult -= InvokeOnAlertBoxAuditResult;
            client.OnBarrage -= InvokeOnBarrage;
            client.OnBindingGift -= InvokeOnBindingGift;
            client.OnBoostCardMessage -= InvokeOnBoostCardMessage;
            client.OnBottomMessage -= InvokeOnBottomMessage;
            client.OnCaption -= InvokeOnCaption;
            client.OnChatMessage -= InvokeOnChatMessage;
            client.OnControlMessage -= InvokeOnControlMessage;
            client.OnLivePaused -= InvokeOnLivePaused;
            client.OnLiveEnded -= InvokeOnLiveEnded;
            client.OnUnhandledControlMessage -= InvokeOnUnhandledControlMessage;
            client.OnEmoteChat -= InvokeOnEmoteChat;
            client.OnEnvelope -= InvokeOnEnvelope;
            client.OnGameRankNotify -= InvokeOnGameRankNotify;
            client.OnGift -= InvokeOnGift;
            client.OnGiftMessage -= InvokeOnGiftMessage;
            client.OnGiftPrompt -= InvokeOnGiftPrompt;
            client.OnGoalUpdate -= InvokeOnGoalUpdate;
            client.OnImDelete -= InvokeOnImDelete;
            client.OnInRoomBannerMessage -= InvokeOnInRoomBannerMessage;
            client.OnLike -= InvokeOnLike;
            client.OnLinkLayerMessage -= InvokeOnLinkLayerMessage;
            client.OnLinkMessage -= InvokeOnLinkMessage;
            client.OnLinkMicFanTicketMethod -= InvokeOnLinkMicFanTicketMethod;
            client.OnLinkMicMethod -= InvokeOnLinkMicMethod;
            client.OnLinkState -= InvokeOnLinkState;
            client.OnLiveIntro -= InvokeOnLiveIntro;
            client.OnMarqueeAnnouncement -= InvokeOnMarqueeAnnouncement;
            client.OnMemberMessage -= InvokeOnMemberMessage;
            client.OnUnhandledMemberMessage -= InvokeOnUnhandledMemberMessage;
            client.OnJoin -= InvokeOnJoin;
            client.OnSubscribe -= InvokeOnSubscribe;
            client.OnMsgDetect -= InvokeOnMsgDetect;
            client.OnNotice -= InvokeOnNotice;
            client.OnNotify -= InvokeOnNotify;
            client.OnPartnershipDropsUpdate -= InvokeOnPartnershipDropsUpdate;
            client.OnPartnershipGameOffline -= InvokeOnPartnershipGameOffline;
            client.OnPartnershipPunish -= InvokeOnPartnershipPunish;
            client.OnPerception -= InvokeOnPerception;
            client.OnPoll -= InvokeOnPoll;
            client.OnRankText -= InvokeOnRankText;
            client.OnRankUpdate -= InvokeOnRankUpdate;
            client.OnRoomMessage -= InvokeOnRoomMessage;
            client.OnRoomUpdate -= InvokeOnRoomUpdate;
            client.OnRoomVerify -= InvokeOnRoomVerify;
            client.OnSocialMessage -= InvokeOnSocialMessage;
            client.OnUnhandledSocialMessage -= InvokeOnUnhandledSocialMessage;
            client.OnFollow -= InvokeOnFollow;
            client.OnShare -= InvokeOnShare;
            client.OnSpeaker -= InvokeOnSpeaker;
            client.OnSubCapsule -= InvokeOnSubCapsule;
            client.OnSubNotify -= InvokeOnSubNotify;
            client.OnSubPinEvent -= InvokeOnSubPinEvent;
            client.OnSubscriptionNotify -= InvokeOnSubscriptionNotify;
            client.OnToast -= InvokeOnToast;
            client.OnUnauthorizedMember -= InvokeOnUnauthorizedMember;
            client.OnUnhandled -= InvokeOnUnhandled;
            // Beta Events:
            client.OnGiftBroadcast -= InvokeOnGiftBroadcast;
            client.OnHourlyRank -= InvokeOnHourlyRank;
            client.OnLinkMicArmies -= InvokeOnLinkMicArmies;
            client.OnLinkMicBattle -= InvokeOnLinkMicBattle;
            client.OnLinkMicBattlePunishFinish -= InvokeOnLinkMicBattlePunishFinish;
            client.OnLinkmicBattleTask -= InvokeOnLinkmicBattleTask;
            client.OnOecLiveShopping -= InvokeOnOecLiveShopping;
            client.OnQuestionNew -= InvokeOnQuestionNew;
            client.OnRoomPinMessage -= InvokeOnRoomPinMessage;
            client.OnSystemMessage -= InvokeOnSystemMessage;
        }

        /// <summary>
        /// Checks whether a LogMessage should be printed
        /// </summary>
        /// <param name="msgLevel">LogLevel for Message</param>
        /// <returns>True if Message should be printed to Console</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool ShouldLog(LogLevel msgLevel) => settings.PrintToConsole && settings.LogLevel.HasFlag(msgLevel);
        #endregion
        #endregion
    }
}
