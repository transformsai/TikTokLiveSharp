using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public static class WebPInstaller
{
    private const string WEBP_URL = "https://github.com/netpyoung/unity.webp.git?path=unity_project/Assets/unity.webp";
    private const string WEBP_NAME = "Unity.WebP";

    private static Request PackageManagerRequest = null;

    [MenuItem("TikTokLive/Setup/Install WebP Support")]
    private static void InstallWebP()
    {
        if (PackageManagerRequest != null)
            return; // Already Processing a Request
        Debug.Log("[TikTokLive] Attempting to install WebP-Support...");
        PackageManagerRequest = Client.List(); // List Installed Packages
        EditorApplication.update += CheckProgress;
    }

    private static void CheckProgress()
    {
        if (PackageManagerRequest.IsCompleted)
        {
            if (PackageManagerRequest.Status == StatusCode.Success)
            {
                if (PackageManagerRequest as ListRequest != null)
                {
                    ListRequest listReq = (ListRequest)PackageManagerRequest;
                    foreach (var pkg in listReq.Result)
                        if (pkg.displayName.Equals(WEBP_NAME))
                        {
                            Debug.Log("[TikTokLive] WebP already installed!");
                            EditorApplication.update -= CheckProgress;
                            PackageManagerRequest = null;
                            return;
                        }
                    // No WebP Installed. Add Now
                    Debug.Log("[TikTokLive] Installing WebP-Package. Please wait.");
                    PackageManagerRequest = Client.Add(WEBP_URL);
                    return;
                }
                else if (PackageManagerRequest as AddRequest != null)
                {
                    Debug.Log("[TikTokLive] Installed WebP-Support");
                    EditorApplication.update -= CheckProgress;
                    PackageManagerRequest = null;
                }
                else
                {
                    Debug.LogError("[TikTokLive] Error while parsing PackageManager-Request. Stopping.");
                    EditorApplication.update -= CheckProgress;
                }
            }
            else
            {
                Debug.LogError($"[TikTokLive] Error while Installing WebP Package: {PackageManagerRequest.Error.message}");
                EditorApplication.update -= CheckProgress;
            }
            PackageManagerRequest = null;
        }
    }
}
