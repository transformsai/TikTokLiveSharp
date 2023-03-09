using System;
using System.Runtime.CompilerServices;

namespace TikTokLiveSharp.Debugging
{
    /// <summary>
    /// Simple Logging-Wrapper for TikTokLiveSharp
    /// </summary>
    public static class Debug
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(string message, UnityEngine.Object obj = null) =>
#if UNITY
            UnityEngine.Debug.Log($"[TikTokClient] {message}", obj);
#else
            System.Console.WriteLine(message);
#endif


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning(string message, UnityEngine.Object obj = null) =>
#if UNITY
            UnityEngine.Debug.LogWarning($"[TikTokClient] {message}", obj);
#else
            System.Console.WriteLine($"[WARNING] {message}");
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogError(string message, UnityEngine.Object obj = null) =>
#if UNITY
            UnityEngine.Debug.LogError($"[TikTokClient] {message}", obj);
#else
            System.Console.WriteLine($"[ERROR] {message}");
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogException(Exception e, UnityEngine.Object obj = null) =>
#if UNITY
            UnityEngine.Debug.LogException(e, obj);
#else
            System.Console.WriteLine($"[ERROR] {e.ToString()}");
#endif
    }
}

