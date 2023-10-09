using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace TikTokLiveUnity.Utils
{
    /// <summary>
    /// Downloads & Caches Textures & Sprites
    /// <para>
    /// Script by Frank van Hoof
    /// https://vanhoof.dev
    /// </para>
    /// </summary>
    public sealed class TextureCache
    {
        #region Properties
        /// <summary>
        /// Event raised when an Exception occurred
        /// </summary>
        public static event Action<Exception> OnException;
        /// <summary>
        /// Max number of Textures Cached (to limit Memory-Usage)
        /// </summary>
        private readonly uint MAX_CACHE_SIZE;
        /// <summary>
        /// Cache for Textures. Key is URL for texture
        /// </summary>
        private readonly Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
        /// <summary>
        /// Callbacks for active Downloads. Key is URL for texture
        /// </summary>
        private readonly Dictionary<string, List<Action<Texture2D>>> textureCallbacks = new Dictionary<string, List<Action<Texture2D>>>();
        /// <summary>
        /// Cache for Sprites. Key is URL for Sprite
        /// </summary>
        private readonly Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
        /// <summary>
        /// Callbacks for active Downloads. Key is URL for Sprite
        /// </summary>
        private readonly Dictionary<string, List<Action<Sprite>>> spriteCallbacks = new Dictionary<string, List<Action<Sprite>>>();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for a TextureCache
        /// </summary>
        /// <param name="maxCacheSize">Max number of Textures Cached (to limit Memory-Usage)</param>
        public TextureCache(uint maxCacheSize = 1024)
        {
            MAX_CACHE_SIZE = maxCacheSize;
        }

        /// <summary>
        /// Clears Dictionaries to minimize floating References to Textures
        /// </summary>
        ~TextureCache() 
        {
            lock(textureCache)
                textureCache.Clear();
            lock(textureCallbacks)
                textureCallbacks.Clear();
            lock(spriteCache)
                spriteCache.Clear();
            lock(spriteCallbacks)
                spriteCallbacks.Clear();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Requests a Texture from the Caches
        /// </summary>
        /// <param name="urls">URLs for Texture. Uses first with valid extension</param>
        /// <param name="onComplete">Callback for Texture-Retrieval</param>
        public void RequestImage(IEnumerable<string> urls, Action<Texture2D> onComplete = null)
        {
            IEnumerable<string> enumerable = urls as string[] ?? urls.ToArray();
            if (!enumerable.Any())
                return;
            string url = enumerable.FirstOrDefault(url => url.Contains("jpg") || url.Contains("jpeg") || url.Contains("png")) ??
                         enumerable.FirstOrDefault(url => url.Contains("webp"));
            if (url != null)
                RequestImage(url, onComplete);
            else onComplete?.Invoke(null);
        }

        /// <summary>
        /// Requests a Texture from the Cache
        /// </summary>
        /// <param name="url">URL for Texture</param>
        /// <param name="onComplete">Callback for Texture-Retrieval</param>
        public void RequestImage(string url, Action<Texture2D> onComplete = null)
        {
            if (string.IsNullOrEmpty(url))
                return;
            lock (textureCache)
                if (textureCache.TryGetValue(url, out Texture2D value))
                {
                    onComplete?.Invoke(value); // Texture was Cached
                    return;
                }
            lock (textureCallbacks)
            {
                if (textureCallbacks.TryGetValue(url, out List<Action<Texture2D>> callbacks))
                {
                    callbacks.Add(onComplete); // Texture is already being downloaded
                    return;
                }
                textureCallbacks.Add(url, new List<Action<Texture2D>> { onComplete });
            }
            Dispatcher.RunCoroutineOnMainThread(DownloadTexture(url, OnCompleteDownload));
        }

        /// <summary>
        /// Requests a Sprite from the Caches
        /// </summary>
        /// <param name="urls">URLs for Texture. Uses first with valid extension</param>
        /// <param name="onComplete">Callback for Sprite-Retrieval</param>
        public void RequestSprite(IEnumerable<string> urls, Action<Sprite> onComplete = null)
        {
            IEnumerable<string> enumerable = urls as string[] ?? urls.ToArray();
            if (!enumerable.Any())
                return;
            string url = enumerable.FirstOrDefault(url => url.Contains("jpg") || url.Contains("jpeg") || url.Contains("png")) ??
                         enumerable.FirstOrDefault(url => url.Contains("webp"));
            if (url != null)
                RequestSprite(url, onComplete);
            else onComplete?.Invoke(null);
        }

        /// <summary>
        /// Requests a Sprite from the Cache
        /// </summary>
        /// <param name="url">URL for Texture</param>
        /// <param name="onComplete">Callback for Sprite-Retrieval</param>
        public void RequestSprite(string url, Action<Sprite> onComplete = null)
        {
            if (string.IsNullOrEmpty(url))
                return;
            lock (spriteCache)
                if (spriteCache.TryGetValue(url, out Sprite value)) // Sprite Cached
                {
                    onComplete?.Invoke(value);
                    return;
                }
            lock (textureCache)
                if (textureCache.TryGetValue(url, out Texture2D tex)) // Texture Cached, but no Sprite
                {
                    Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    lock(spriteCache)
                        spriteCache.Add(url, spr);
                    onComplete?.Invoke(spr);
                    return;
                }
            lock (spriteCallbacks)
            {
                if (spriteCallbacks.TryGetValue(url, out List<Action<Sprite>> callbacks))
                {
                    callbacks.Add(onComplete);
                    return;
                }
                spriteCallbacks.Add(url, new List<Action<Sprite>>() { onComplete });
            }
            RequestImage(url);
        }

        /// <summary>
        /// Callback for Download-Completion. 
        /// Handles stored User-Callbacks
        /// </summary>
        /// <param name="url">URL for Download</param>
        /// <param name="tex">Texture for Download</param>
        private void OnCompleteDownload(string url, Texture2D tex)
        {
            if (tex == null)
            {
                OnException?.Invoke(new NullReferenceException("Texture was NULL after downloading"));
                return;
            }
            lock (textureCallbacks)
            {
                if (textureCallbacks.TryGetValue(url, out List<Action<Texture2D>> texCallbacks))
                {
                    textureCallbacks.Remove(url);
                    for (int i = 0; i < texCallbacks.Count; i++)
                        texCallbacks[i]?.Invoke(tex);
                }
            }
            Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            lock (spriteCache)
                spriteCache.Add(url, s);
            lock (spriteCallbacks)
            {
                if (spriteCallbacks.TryGetValue(url, out List<Action<Sprite>> sprCallbacks))
                {
                    spriteCallbacks.Remove(url);
                    for (int i = 0; i < sprCallbacks.Count; i++)
                        sprCallbacks[i]?.Invoke(s);
                }
            }
        }

        /// <summary>
        /// Coroutine for Downloading Texture from URL
        /// </summary>
        /// <param name="url">URL for Texture</param>
        /// <param name="onComplete">Callback for Download-Completion</param>
        private IEnumerator DownloadTexture(string url, Action<string, Texture2D> onComplete = null)
        {
            UnityWebRequest texRequest = UnityWebRequestTexture.GetTexture(url);
            yield return texRequest.SendWebRequest();
            try
            {
                if (texRequest.result == UnityWebRequest.Result.Success)
                {
                    Texture2D pic = DownloadHandlerTexture.GetContent(texRequest);
                    AddTextureToCache(url, pic);
                    onComplete?.Invoke(url, pic);
                }
#if WEBP_INSTALLED
                else if (url.Contains("webp") && texRequest.result == UnityWebRequest.Result.DataProcessingError)
                {
                    // Process webp-file to Texture2D
                    byte[] bytes = texRequest.downloadHandler.data;
                    Texture2D tex = WebP.Texture2DExt.CreateTexture2DFromWebP(bytes, false, false, out WebP.Error err, null, false);
                    AddTextureToCache(url, tex);
                    if (err == WebP.Error.Success)
                        onComplete?.Invoke(url, tex);
                    else throw new Exception("WebP-Texture failed to convert");
                }
#endif
                else throw new Exception($"Texture failed to Download with result {texRequest.result} and code {texRequest.responseCode}.{Environment.NewLine}Error:{texRequest.error}");
            }
            catch (Exception e)
            {
                Debug.LogError("Error whilst downloading Texture");
                Debug.LogException(e);
                onComplete?.Invoke(url, null);
                OnException?.Invoke(e);
            }
        }

        /// <summary>
        /// Adds Texture to Cache. Cleans Cache if Count reaches <see cref="MAX_CACHE_SIZE"/>
        /// </summary>
        /// <param name="url">URL for Texture</param>
        /// <param name="tex">Texture to Add</param>
        private void AddTextureToCache(string url, Texture2D tex)
        {
            lock (textureCache)
            {
                if (textureCache.Count >= MAX_CACHE_SIZE)
                    for (int i = 0; i < MAX_CACHE_SIZE / 4; i++) // Randomly dump 1/4 of cache
                    {
                        string tempUrl = textureCache.Keys.ElementAt(UnityEngine.Random.Range(0, textureCache.Count));
                        textureCache.Remove(tempUrl);

                        lock (spriteCache)
                            if (spriteCache.ContainsKey(tempUrl))
                                spriteCache.Remove(tempUrl);
                    }
                textureCache.Add(url, tex);
            }
        }
        #endregion
    }
}