using System.Collections;
using System.Collections.Generic;

namespace TikTokLiveSharp.Client.Requests
{
    /// <summary>
    /// Holds Cookies for TikTok-Connection
    /// </summary>
    public class TikTokCookieJar : IEnumerable<string>
    {
        private IDictionary<string, string> cookies;

        /// <summary>
        /// Create a TikTok cookie jar instance.
        /// </summary>
        public TikTokCookieJar()
        {
            cookies = new Dictionary<string, string>();
        }

        /// <summary>
        /// Get the cookie by key.
        /// </summary>
        /// <param name="key">The cookie key.</param>
        /// <returns>Cookie value.</returns>
        public string this[string key]
        {
            get => cookies[key];
            set => cookies[key] = value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return cookies.GetEnumerator();
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var cookie in cookies)
                yield return $"{cookie.Key}={cookie.Value};";
        }
    }
}
