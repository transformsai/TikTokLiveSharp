using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TikTokLiveSharp.Unity.Editor
{
    [CustomEditor(typeof(TikTokLiveManager))]
    public class TikTokLiveManagerEditor : UnityEditor.Editor
    {
        private static Dictionary<Object, Dictionary<string, bool>> openFoldOuts;

        public override void OnInspectorGUI()
        {
            if (openFoldOuts == null)
                openFoldOuts = new Dictionary<Object, Dictionary<string, bool>>();
            if (!openFoldOuts.ContainsKey(target))
                openFoldOuts.Add(target, new Dictionary<string, bool>());
            TikTokLiveManager mgr = (TikTokLiveManager)target;            
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            SerializedProperty hasRootProp = serializedObject.FindProperty("hasRootObject");
            if (mgr.transform.parent != null && !hasRootProp.boolValue)
                EditorGUILayout.HelpBox("Please set HasRootObject if your GameObject has a Parent", MessageType.Warning);
            EditorGUILayout.PropertyField(hasRootProp); // Has Root Object
            PropertyFieldWithLabel(serializedObject.FindProperty("texCacheSize"), "Cache Size"); // Texture Cache Size
            EditorGUILayout.PropertyField(serializedObject.FindProperty("autoConnect")); // Auto Connect
            PropertyFieldWithLabel(serializedObject.FindProperty("autoConnectHostId"), "Host Id"); // Auto Connect Host Id
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * .5f);
            PropertyFieldWithLabel(serializedObject.FindProperty("settings"), "Client Settings"); // Connection Settings
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * .5f);
            DrawEvents();
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        private void PropertyFieldWithLabel(SerializedProperty property, string label)
        {
            GUIContent txtLabel = new GUIContent(label, property.tooltip);
            EditorGUILayout.PropertyField(property, txtLabel);
        }

        private void DrawEvents()
        {
            if (DoFoldout("Events", new GUIContent("Events", "Unity-Events on this Manager"), true))
            {
                EditorGUI.indentLevel++;
                DrawGenericEvents();
                DrawRoomEvents();
                DrawUserEvents();
                DrawHostEvents();
                DrawRankEvents();
                DrawLinkMicEvents();
                DrawMiscEvents();
                EditorGUI.indentLevel--;
            }
        }

        private void DrawGenericEvents()
        {
            if (DoFoldout("Events/Generic", new GUIContent("Generic Events")))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onException"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onConnected"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onDisconnected"));
            }
            if (DoFoldout("Events/Connection", new GUIContent("Connection Events")))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLiveEnded"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLivePaused"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onSystemMessage"));
            }
            if (DoFoldout("Events/Generic/Unhandled", new GUIContent("Unhandled Messages")))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onUnhandledEvent"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onUnhandledSocialEvent"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onUnhandledMemberEvent"));
            }
        }

        private void DrawRoomEvents()
        {
            if (DoFoldout("Events/Room", new GUIContent("Room")))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onRoomIntro"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onViewerData"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onRoomMessage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onClosedCaption"));
            }
        }

        private void DrawUserEvents()
        {
            if (DoFoldout("Events/Viewer", new GUIContent("Viewer Events")))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onComment"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onGift"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLike"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onShare"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onFollow"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onJoin"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onSubscribe"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onGiftMessage"));
            }
        }

        private void DrawHostEvents()
        {
            if (DoFoldout("Events/Host", new GUIContent("Host Events")))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onPollMessage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onRoomPinMessage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onGoalUpdate"));
            }
        }

        private void DrawRankEvents()
        {
            if (DoFoldout("Events/Rank", new GUIContent("Ranking")))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onRankText"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onRankUpdate"));
            }
        }

        private void DrawLinkMicEvents()
        {
            if (DoFoldout("Events/LinkMic", new GUIContent("Link Mic Battle")))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLinkMicBattle"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLinkMicArmies"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLinkMicMethod"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLinkMicFanTicket"));
            }
        }

        private void DrawMiscEvents()
        {
            if (DoFoldout("Events/Misc", new GUIContent("Miscellaneous")))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onInRoomBanner"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onDetectMessage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onBarrageMessage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onUnauthorizedMember"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLinkMessage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onLinkLayerMessage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onGiftBroadcast"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onShopMessage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onIMDelete"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onQuestion"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onEnvelope"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onSubNotify"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onEmote"));
            }
        }

        private bool DoFoldout(string path, GUIContent label, bool defaultValue = false)
        {
            Dictionary<string, bool> openStates = openFoldOuts[target];
            if (!openStates.ContainsKey(path))
                openStates.Add(path, defaultValue);
            openStates[path] = EditorGUILayout.Foldout(openStates[path], label, true);
            return openStates[path];
        }
    }
}
