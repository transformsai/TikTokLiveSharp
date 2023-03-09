using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TikTokLiveSharp.Client.Proxy;
using TikTokLiveSharp.Debugging;
using TikTokLiveSharp.Errors.Messaging;
using TikTokLiveSharp.Events;
using TikTokLiveSharp.Models;
#if UNITY
using TikTokLiveSharp.Unity.Utils;
#endif

namespace TikTokLiveSharp.Client
{
    public class TikTokLiveClient : TikTokBaseClient
    {
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

        /// <summary>
        /// Event fired when comments are received.
        /// </summary>
        public event TikTokEventHandler<WebcastChatMessage> OnCommentReceived;

        /// <summary>
        /// Event fired when the client connects.
        /// </summary>
        public event TikTokEventHandler<ConnectionEventArgs> OnConnected;

        /// <summary>
        /// Event fired when the client disconnects.
        /// </summary>
        public event TikTokEventHandler<ConnectionEventArgs> OnDisconnected;

        /// <summary>
        /// Event fired when someone follows.
        /// </summary>
        public event TikTokEventHandler<User> OnFollow;

        /// <summary>
        /// Event fired when a gift is received.
        /// </summary>
        public event TikTokEventHandler<WebcastGiftMessage> OnGiftReceived;

        /// <summary>
        /// Event fired when someone joins the stream.
        /// </summary>
        public event TikTokEventHandler<User> OnJoin;

        /// <summary>
        /// Event fired when someone likes the stream.
        /// </summary>
        public event TikTokEventHandler<User> OnLike;

        /// <summary>
        /// Event fired when groups of likes are received.
        /// Note: OnLike is more likely to be useful here, as OnLikesReceived is called infrequently.
        /// </summary>
        public event TikTokEventHandler<WebcastLikeMessage> OnLikesReceived;

        /// <summary>
        /// Event fired when questions are received.
        /// </summary>
        public event TikTokEventHandler<WebcastQuestionNewMessage> OnQuestionReceived;

        /// <summary>
        /// Event fired when the stream is shared.
        /// </summary>
        public event TikTokEventHandler<ShareEventArgs> OnShare;

        /// <summary>
        /// Event fired when the stream is shared to 5 or more users / 10 or more users.
        /// </summary>
        public event TikTokEventHandler<ShareEventArgs> OnMoreShare;

        /// <summary>
        /// Event fired when the view count updates.
        /// </summary>
        public event TikTokEventHandler<WebcastRoomUserSeqMessage> OnViewerCountUpdated;

        /// <summary>
        /// Event fired when the live has ended.
        /// </summary>
        public event TikTokEventHandler OnLiveEnded;

        /// <summary>
        /// Event fired when emotes are received.
        /// </summary>
        public event TikTokEventHandler<WebcastEmoteChatMessage> OnEmoteReceived;

        /// <summary>
        /// Event fired when envelopes are received.
        /// </summary>
        public event TikTokEventHandler<WebcastEnvelopeMessage> OnEnvelopeReceived;

        /// <summary>
        /// Event fired when a user subscribes.
        /// </summary>
        public event TikTokEventHandler<WebcastMemberMessage> OnSubscribe;

        /// <summary>
        /// Event fired when the weekly ranking updates.
        /// </summary>
        public event TikTokEventHandler<WebcastHourlyRankMessage> OnWeeklyRankingUpdated;

        /// <summary>
        /// Event fired when a mic battle begins.
        /// </summary>
        public event TikTokEventHandler<WebcastLinkMicBattle> OnMicBattle;

        /// <summary>
        /// Event fired during a mic battle when an update is received.
        /// </summary>
        public event TikTokEventHandler<WebcastLinkMicArmies> OnMicBattleUpdated;

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
        /// 
        /// </summary>
        /// <param name="onConnectException">Callback for possible Exceptions during Connect</param>
        /// <returns></returns>
        protected override async Task<string> Connect(Action<Exception> onConnectException = null)
        {
            string roomID = await base.Connect(onConnectException);
            if (Connected)
            {
                if (Settings.PrintToConsole)
                    Debug.Log($"Connected to Room {roomID}");
                RunEvent(OnConnected, new ConnectionEventArgs(true));
            }
            return roomID;
        }

        protected override async Task Disconnect()
        {
            await base.Disconnect();
            if (!Connected)
            {
                if (Settings.PrintToConsole)
                    Debug.Log("Disconnected");
                RunEvent(OnDisconnected, new ConnectionEventArgs(false));
            }
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

        private T Deserialize<T>(MemoryStream stream) where T : ProtoBuf.IExtensible
        {
            try
            {
                T msg = ProtoBuf.Serializer.Deserialize<T>(stream);
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

        private void HandleMessage(Message message)
        {
            Debug.Log("Handling Message: " + message.Type);
            using (var stream = new MemoryStream(message.Binary))
                switch (message.Type)
                {
                    case nameof(WebcastChatMessage):
                        WebcastChatMessage chatMessage = Deserialize<WebcastChatMessage>(stream);
                        if (chatMessage != null)
                            RunEvent(OnCommentReceived, chatMessage);
                        return;

                    case nameof(WebcastGiftMessage):
                        WebcastGiftMessage giftMessage = Deserialize<WebcastGiftMessage>(stream);
                        if (giftMessage != null)
                            RunEvent(OnGiftReceived, giftMessage);
                        return;

                    case nameof(WebcastLikeMessage):
                        WebcastLikeMessage likeMessage = Deserialize<WebcastLikeMessage>(stream);
                        if (likeMessage != null)
                            RunEvent(OnLikesReceived, likeMessage);
                        return;

                    case nameof(WebcastQuestionNewMessage):
                        WebcastQuestionNewMessage questionMessage = Deserialize<WebcastQuestionNewMessage>(stream);
                        if (questionMessage != null)
                            RunEvent(OnQuestionReceived, questionMessage);
                        return;

                    case nameof(WebcastRoomUserSeqMessage):
                        WebcastRoomUserSeqMessage roomMessage = Deserialize<WebcastRoomUserSeqMessage>(stream);
                        if (roomMessage != null)
                            RunEvent(OnViewerCountUpdated, roomMessage);
                        return;

                    case nameof(WebcastEmoteChatMessage):
                        WebcastEmoteChatMessage emoteMessage = Deserialize<WebcastEmoteChatMessage>(stream);
                        if (emoteMessage != null)
                            RunEvent(OnEmoteReceived, emoteMessage);
                        return;

                    case nameof(WebcastHourlyRankMessage):
                        WebcastHourlyRankMessage hourlyRankMessage = Deserialize<WebcastHourlyRankMessage>(stream);
                        if (hourlyRankMessage != null)
                            RunEvent(OnWeeklyRankingUpdated, hourlyRankMessage);
                        return;

                    case nameof(WebcastEnvelopeMessage):
                        WebcastEnvelopeMessage envelopeMessage = Deserialize<WebcastEnvelopeMessage>(stream);
                        if (envelopeMessage != null)
                            RunEvent(OnEnvelopeReceived, envelopeMessage);
                        return;
                    case nameof(WebcastLinkMicBattle):
                        WebcastLinkMicBattle linkMicBattle = Deserialize<WebcastLinkMicBattle>(stream);
                        if (linkMicBattle != null)
                            RunEvent(OnMicBattle, linkMicBattle);
                        return;
                    case nameof(WebcastLinkMicArmies):
                        WebcastLinkMicArmies linkMicArmies = Deserialize<WebcastLinkMicArmies>(stream);
                        if (linkMicArmies != null)
                            RunEvent(OnMicBattleUpdated, linkMicArmies);
                        return;
                    case nameof(WebcastSocialMessage):
                        WebcastSocialMessage socialMessage = Deserialize<WebcastSocialMessage>(stream);
                        if (socialMessage != null)
                            InvokeSpecialEvent(socialMessage);
                        return;
                    case nameof(WebcastMemberMessage):
                        WebcastMemberMessage memberMessage = Deserialize<WebcastMemberMessage>(stream);
                        if (memberMessage == null)
                            return;
                        if (memberMessage.Event.eventDetails?.displayType == "live_room_enter_toast")
                            RunEvent(OnJoin, memberMessage.User);
                        else if (memberMessage.actionId == 7)
                            RunEvent(OnSubscribe, memberMessage);
                        else RunEvent(UnhandledMemberEvent, memberMessage);
                        return;
                    case nameof(WebcastControlMessage):
                        var controlMessage = ProtoBuf.Serializer.Deserialize<WebcastControlMessage>(stream);
                        if (controlMessage.Action == 3)
                            RunEvent(OnLiveEnded, new EventArgs());
                        return;
                }
            // Run Unhandled
            RunEvent(UnhandledEvent, message);
        }

        private void InvokeSpecialEvent(WebcastSocialMessage messageEvent)
        {
            Match match = Regex.Match(messageEvent.Event.eventDetails.displayType, "pm_mt_guidance_viewer_([0-9]+)_share");
            if (match.Success)
            {
                if (int.TryParse(match.Groups[0].Value, out var result))
                    RunEvent(OnMoreShare, new ShareEventArgs(messageEvent.User, result));
                return;
            }
            switch (messageEvent.Event.eventDetails.displayType)
            {
                case "pm_mt_msg_viewer":
                    RunEvent(OnLike, null);
                    return;

                case "pm_main_follow_message_viewer_2":
                    RunEvent(OnFollow, messageEvent.User);
                    return;

                case "pm_mt_guidance_share":
                    RunEvent(OnShare, new ShareEventArgs(messageEvent.User, 1));
                    return;

                case "pm_mt_join_message_other_viewer":
                    RunEvent(OnJoin, messageEvent.User);
                    return;
            }
            // Run Unhandled
            RunEvent(UnhandledSocialEvent, messageEvent);
        }
    }
}