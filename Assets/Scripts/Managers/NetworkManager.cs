using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace PlayNoob.Controller {
    public static class NetworkManager {
        public static IEnumerator GetRequest(string url, UnityAction<string> onComplete, UnityAction<string> onFailure = null) {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                switch (webRequest.result) {
                    case UnityWebRequest.Result.ConnectionError:
                        onFailure?.Invoke(webRequest.error);
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError("Error: " + webRequest.error);
                        onFailure?.Invoke(webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError("HTTP Error: " + webRequest.error);
                        onFailure?.Invoke(webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Received: " + webRequest.downloadHandler.text);
                        onComplete.Invoke(webRequest.downloadHandler.text);
                        break;
                    default:
                        onFailure?.Invoke(webRequest.error);
                        break;
                }
            }
        }
    }
}