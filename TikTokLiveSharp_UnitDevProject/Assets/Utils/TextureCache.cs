using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace TikTokLiveSharp.Unity.Utils
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
        #endregion

        #region Methods
        /// <summary>
        /// Requests a Texture from the Cache
        /// </summary>
        /// <param name="url">URL for Texture</param>
        /// <param name="onComplete">Callback for Texture-Retrieval</param>
        public void RequestImage(string url, Action<Texture2D> onComplete = null)
        {
            if (string.IsNullOrEmpty(url))
                return;
            if (textureCache.ContainsKey(url))
            {
                onComplete?.Invoke(textureCache[url]);
                return;
            }
            lock (textureCallbacks)
            {
                if (textureCallbacks.ContainsKey(url))
                {
                    textureCallbacks[url].Add(onComplete);
                    return;
                }
                textureCallbacks.Add(url, new List<Action<Texture2D>> { onComplete });
            }
            Dispatcher.RunCoroutineOnMainThread(DownloadTexture(url, OnCompleteDownload));
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
            if (spriteCache.ContainsKey(url)) // Sprite Cached
            {
                onComplete?.Invoke(spriteCache[url]);
                return;
            }
            if (textureCache.ContainsKey(url)) // Texture Cached, but no Sprite
            {
                Texture2D tex = textureCache[url];
                Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2f, tex.height / 2f));
                spriteCache.Add(url, spr);
                onComplete?.Invoke(spr);
                return;
            }
            if (spriteCallbacks.ContainsKey(url))
            {
                spriteCallbacks[url].Add(onComplete);
                return;
            }
            spriteCallbacks.Add(url, new List<Action<Sprite>>() { onComplete });
            Action<Texture2D> texOnComplete = tex =>
            {
                Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2f, tex.height / 2f));
                spriteCache.Add(url, spr);
                List<Action<Sprite>> sprCallbacks = null;
                lock(spriteCallbacks)
                {
                    if (spriteCallbacks.ContainsKey(url))
                        sprCallbacks = spriteCallbacks[url];
                    spriteCallbacks.Remove(url);
                }
                if (sprCallbacks != null)
                    for (int i = 0; i < sprCallbacks.Count; i++)
                        sprCallbacks[i]?.Invoke(spr);
            };
            RequestImage(url, texOnComplete);
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
                // TODO: Handle Error
                return;
            }
            if (textureCallbacks.ContainsKey(url))
            {
                List<Action<Texture2D>> texCallbacks;
                lock (textureCallbacks)
                {
                    texCallbacks = textureCallbacks[url];
                    textureCallbacks.Remove(url);
                }
                for (int i = 0; i < texCallbacks.Count; i++)
                    texCallbacks[i]?.Invoke(tex);
            }
            if (spriteCallbacks.ContainsKey(url))
            {
                Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f * tex.width, 0.5f * tex.width));
                lock (spriteCache)
                    spriteCache.Add(url, s);
                List<Action<Sprite>> sprCallbacks;
                lock (spriteCallbacks)
                {
                    sprCallbacks = spriteCallbacks[url];
                    spriteCallbacks.Remove(url);
                }
                for (int i = 0; i < sprCallbacks.Count; i++)
                    sprCallbacks[i]?.Invoke(s);
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
                else onComplete?.Invoke(url, null); // TODO: Handle Parsing-Failure
            }
#endif
            else
            {
                // TODO: Handle Download-Failure
                onComplete?.Invoke(url, null);
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
                        string tempURL = textureCache.Keys.ElementAt(UnityEngine.Random.Range(0, textureCache.Count));
                        textureCache.Remove(tempURL);
                        if (spriteCache.ContainsKey(tempURL))
                            lock (spriteCache)
                                spriteCache.Remove(tempURL);
                    }
                textureCache.Add(url, tex);
            }
        }
        #endregion
    }
}