using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace TikTokLiveUnity.Utils
{
    /// <summary>
    /// Dispatches Actions & Coroutines between Threads
    /// <para>
    /// Script by Frank van Hoof
    /// https://vanhoof.dev
    /// </para>
    /// </summary>
    public class Dispatcher : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Singleton-Instance
        /// </summary>
        private static Dispatcher _instance = null;
        /// <summary>
        /// Whether there is currently Work for the Main Thread
        /// </summary>
        private static volatile bool _hasWork = false;
        /// <summary>
        /// Backlog of Work for Simple Actions (No Parameters)
        /// </summary>
        private static readonly List<Action> _simpleActionBacklog = new List<Action>();
        /// <summary>
        /// Backlog of Work for Complex Actions (With Object Parameters)
        /// </summary>
        private static readonly Dictionary<Action<object[]>, object[]> _complexActionBacklog = new Dictionary<Action<object[]>, object[]>();
        #endregion

        #region Methods
        #region Async
        /// <summary>
        /// Runs Action on Thread
        /// </summary>
        /// <param name="action">Action to Invoke</param>
        public static void RunAsync(Action action)
        {
            ThreadPool.QueueUserWorkItem(_ => action?.Invoke());
        }
        /// <summary>
        /// Runs Action with State on Thread
        /// </summary>
        /// <param name="action">Action to Invoke</param>
        /// <param name="state">State for Action</param>
        public static void RunAsync(Action<object> action, object state)
        {
            ThreadPool.QueueUserWorkItem(_ => action?.Invoke(state));
        }
        /// <summary>
        /// Runs Action with State on Thread
        /// </summary>
        /// <param name="action">Action to Invoke</param>
        /// <param name="state">State for Action</param>
        public static void RunAsync(Action<object[]> action, params object[] state)
        {
            ThreadPool.QueueUserWorkItem(_ => action?.Invoke(state));
        }
        #endregion

        #region MainThread
        /// <summary>
        /// Runs Action on the Unity Main Thread
        /// </summary>
        /// <param name="action">Action to Invoke</param>
        public static void RunOnMainThread(Action action)
        {
            lock (_simpleActionBacklog)
            {
                _simpleActionBacklog.Add(action);
                _hasWork = true;
            }
        }

        /// <summary>
        /// Runs an Action with Parameters on the Unity Main Thread
        /// </summary>
        /// <param name="action">Action to Invoke</param>
        /// <param name="parameters">Parameters for Action</param>
        public static void RunOnMainThread(Action<object[]> action, params object[] parameters)
        {
            lock (_complexActionBacklog)
            {
                _complexActionBacklog.Add(action, parameters);
                _hasWork = true;
            }
        }
        #endregion

        #region Coroutine
        /// <summary>
        /// Runs Coroutine on Dispatcher-GameObject
        /// </summary>
        /// <param name="routine">Coroutine</param>
        /// <returns>Routine-Reference</returns>
        public static Coroutine RunCoroutine(IEnumerator routine) => _instance.StartCoroutine(routine);
        /// <summary>
        /// Runs Coroutine on Main Thread from any Thread
        /// </summary>
        /// <param name="routine">Coroutine to Run</param>
        public static void RunCoroutineOnMainThread(IEnumerator routine) => RunOnMainThread(() => RunCoroutine(routine));
        #endregion

        #region Private
        /// <summary>
        /// Creates & Initializes Dispatcher-Object when Game Loads
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_instance == null)
            {
                _instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
                DontDestroyOnLoad(_instance.gameObject);
            }
        }

        /// <summary>
        /// Runs MainThread-Actions
        /// </summary>
        private void Update()
        {
            bool work = _hasWork;
            if (work)
            {
                if (_simpleActionBacklog.Count > 0)
                {
                    Action[] arrSimple = null;
                    lock (_simpleActionBacklog)
                    {
                        arrSimple = _simpleActionBacklog.ToArray();
                        _simpleActionBacklog.Clear();
                    }
                    for (int i = 0; i < arrSimple.Length; i++)
                        arrSimple[i].Invoke();
                }
                if (_complexActionBacklog.Count > 0)
                {
                    KeyValuePair<Action<object[]>, object[]>[] arrComplex = null;
                    lock (_complexActionBacklog)
                    {
                        arrComplex = _complexActionBacklog.ToArray();
                        _complexActionBacklog.Clear();
                    }
                    for (int i = 0; i < arrComplex.Length; i++)
                        arrComplex[i].Key.Invoke(arrComplex[i].Value);
                }
                _hasWork = false;
            }
        }
        #endregion
        #endregion
    }
}
