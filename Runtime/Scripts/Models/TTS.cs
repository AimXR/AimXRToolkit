using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using AimXRToolkit.Managers;
namespace AimXRToolkit.Models
{
    public class TTS
    {
        public async void speak(string text, Language language, AudioSource audioSource)
        {
            UnityWebRequest request = new UnityWebRequest();
            string body = "{\"language\":\"" + language.ToString().ToLower() + "\", \"text\":\"" + text + "\"\n}";
            string url = API.API_URL + API.ROUTE.TTS;

            request.url = url;
            request.downloadHandler = new DownloadHandlerAudioClip(url, AudioType.MPEG);
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));
            request.timeout = 10;

            request.method = "POST";
            request.SetRequestHeader("User-Agent", API.UserAgent);
            request.SetRequestHeader("Accept", "*/*");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept-Language", "fr,fr-FR;q=0.8,en-US;q=0.5,en;q=0.3");
            request.SetRequestHeader("Authorization", AimXRManager.Instance.GetUser().token);
            TaskCompletionSource<UnityWebRequest> tcs = new TaskCompletionSource<UnityWebRequest>();
            request.SendWebRequest().completed += (op) =>
            {
                tcs.SetResult(request);
            };
            await tcs.Task;
            if (request.responseCode != 0 && request.responseCode < 300)
            {
                audioSource.clip = DownloadHandlerAudioClip.GetContent(request);
                audioSource.time = 0f;
                audioSource.Play();
            } else
            {
                Debug.LogError("TTS request error : " + request.error + "\nURL=" + url + "\nBody=" + body);
            }
        }
    }
}
