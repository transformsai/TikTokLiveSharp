/// Basic profile picture handling example.
/// - @sebheron 2022

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Events.MessageData.Messages;
using TikTokLiveSharp.Models;
using TikTokLiveSharp.Models.Protobuf;
using TikTokLiveSharp.Unity;
using UnityEngine;
using UnityEngine.Networking;

public class Example : MonoBehaviour
{
    /// <summary>
    /// The uniqueId for the stream.
    /// </summary>
    public string id;

    /// <summary>
    /// The TikTokLiveClient, initalised on Start.
    /// </summary>
    private TikTokLiveClient _client;

    /// <summary>
    /// Queue of profile pictures
    /// </summary>
    private Queue<string> _profilePictures;

    private CancellationTokenSource tokenSource;
    private CancellationToken cancelToken;

    /// <summary>
    /// Start method. Initalises 
    /// </summary>
    IEnumerator Start()
    {
        yield return null;
        TikTokLiveManager.Instance.ConnectToStreamAsync(id, null);
//        tokenSource = new CancellationTokenSource();
//        cancelToken = tokenSource.Token;
//        _client = new TikTokLiveClient(id);
//        _profilePictures = new Queue<string>();
//        // Subscribe to the OnCommentReceived event.
//        _client.OnComment += Client_OnCommentReceived;
//        _client.OnException += Client_OnException;
//        yield return new WaitForSeconds(.5f); // Wait .5s between Startup & Starting Connection
//        // Try catch doesn't work well here due to Async. Exceptions are handled via Callback instead
//#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
//        _client.Start(cancelToken, e =>
//        {
//            Debug.LogWarning($"Caught Exception while Connecting: {e.Message}");
//            Debug.LogException(e);
//        });
//#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    private void Client_OnException(object sender, System.Exception e)
    {
        Debug.LogWarning($"Caught Exception: {e.Message}");
        Debug.LogException(e);
    }

    /// <summary>
    /// On Destroy method.
    /// Required as seperate threads continuing running outside of play mode in Unity.
    /// Without this Client_OnCommentReceived would continue to be called.
    /// </summary>
    void OnDestroy()
    {
        if (TikTokLiveManager.Exists)
            TikTokLiveManager.Instance.DisconnectFromLivestream();
        //tokenSource.Cancel();
        //_client?.Stop();
        // Unsubscribe from the OnCommentReceived event.
        //_client.OnComment -= Client_OnCommentReceived;
    }

    /// <summary>
    /// Comment received method.
    /// </summary>
    void Client_OnCommentReceived(object sender, Comment e)
    {
        // We need to lock the profile pictures queue as we're not currently working on the main thread.
        lock (_profilePictures)
        {
            // Get the JPEG url for the users profile picture.
            var url = e.User.ProfilePicture.URLs.FirstOrDefault(x => x.Contains(".jpeg"));
            if (url != null)
            {
                // Enqueue the url of the user.
                _profilePictures.Enqueue(url);
            }
        }
    }

    /// <summary>
    /// Update method called once every frame.
    /// </summary>
    void Update()
    {
        /* We need to lock the profile picture queue here
           as we don't want any modifications to the queue while using it. */
    //    lock (_profilePictures)
    //    {
    //        if (_profilePictures.Count > 0)
    //        {
    //            // Dequeue the url.
    //            var url = _profilePictures.Dequeue();
    //            // Add a new cube.
    //            StartCoroutine(AddNewCube(url));
    //        }
    //    }
    }

    /// <summary>
    /// Enumerator method called from Update.
    /// This method adds a new cube with the players profile picture on it.
    /// </summary>
    IEnumerator AddNewCube(string url)
    {
        // Download the texture from the web.
        var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        var tex = DownloadHandlerTexture.GetContent(request);

        // Build a cube with the texture applied to it.
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = Random.insideUnitSphere * 5;
        obj.GetComponent<MeshRenderer>().material.mainTexture = tex;
    }
}