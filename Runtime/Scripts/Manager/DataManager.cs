using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

using AimXRToolkit.Models;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.Video;

namespace AimXRToolkit.Managers
{
    public sealed class DataManager
    {
        private static DataManager Instance;
        // api url
        [SerializeField]
        [Tooltip("The url of the api")]
        public static string APIUrl = "http://localhost:8000";
        // artifact cache dictionary
        private readonly Dictionary<int, CacheItem<Artifact>> ArtifactCache;
        private readonly Dictionary<int, CacheItem<Activity>> ActivityCache;
        private readonly Dictionary<int, CacheItem<Workplace>> WorkplaceCache;
        private readonly Dictionary<int, CacheItem<Models.Action>> ActionCache;
        private readonly Dictionary<int, CacheItem<List<Models.Component>>> TargetComponentsCache;
        private readonly Dictionary<int, CacheItem<Models.Target>> TargetCache;
        private readonly Dictionary<int, CacheItem<Models.Component>> ComponentCache;
        private readonly Dictionary<int, CacheItem<byte[]>> ResourceCache;
        private DataManager()
        {
            ArtifactCache = new Dictionary<int, CacheItem<Artifact>>();
            ActivityCache = new Dictionary<int, CacheItem<Activity>>();
            WorkplaceCache = new Dictionary<int, CacheItem<Workplace>>();
            ActionCache = new Dictionary<int, CacheItem<Models.Action>>();
            TargetComponentsCache = new Dictionary<int, CacheItem<List<Models.Component>>>();
            TargetCache = new Dictionary<int, CacheItem<Models.Target>>();
            ComponentCache = new Dictionary<int, CacheItem<Models.Component>>();
            ResourceCache = new Dictionary<int, CacheItem<byte[]>>();
        }

        public static DataManager GetInstance()
        {
            return Instance ??= new DataManager();
        }

        public async Task<Artifact> GetArtifactAsync(int id)
        {
            if (!ArtifactCache.ContainsKey(id) || IsCacheExpired(ArtifactCache[id]))
            {
                // get artifact from api
                Artifact artifact = await DownloadArtifact(id);
                // add to cache
                ArtifactCache[id] = new CacheItem<Artifact> { CachedTime = DateTime.Now, Data = artifact };
            }
            return ArtifactCache[id].Data;
        }
        public async Task<Models.Target> GetTargetAsync(int id)
        {
            if (!TargetComponentsCache.ContainsKey(id) || IsCacheExpired(TargetComponentsCache[id]))
            {
                var target = await DownloadTarget(id);
                TargetCache[id] = new CacheItem<Target> { CachedTime = DateTime.Now, Data = target };
            }
            return TargetCache[id].Data;
        }

        public async Task<Models.Component> GetComponentAsync(int id)
        {
            if (!ComponentCache.ContainsKey(id) || IsCacheExpired(ComponentCache[id]))
            {

                var component = await DownloadComponent(id);
                ComponentCache[id] = new CacheItem<Models.Component> { CachedTime = DateTime.Now, Data = component };

            }
            return ComponentCache[id].Data;
        }

        private async Task<Models.Component> DownloadComponent(int id)
        {
            var res = await API.GetAsync(API.ROUTE.COMPONENTS + id);

            if (res.responseCode >= 200 && res.responseCode < 300)
            {
                JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
                return new Models.Component(data);
            }

            switch (res.responseCode)
            {
                case 404: throw new ComponentNotFoundException(id);
                case 0: throw new TimeoutException();
                default: throw new Exception();
            }
        }

        private async Task<Models.Target> DownloadTarget(int id)
        {
            var res = await API.GetAsync(API.ROUTE.TARGETS + id);

            if (res.responseCode >= 200 && res.responseCode < 300)
            {
                JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
                return new Target(data);
            }

            switch (res.responseCode)
            {
                case 404: throw new TargetNotFoundException(id);
                case 0: throw new TimeoutException();
                default: throw new Exception();
            }
        }

        public async Task<Models.Action> GetActionAsync(int id)
        {
            if (!ActionCache.ContainsKey(id) || IsCacheExpired(ActionCache[id]))
            {
                // get action from api
                Models.Action action = await DownloadAction(id);
                // add to cache
                ActionCache[id] = new CacheItem<Models.Action> { CachedTime = DateTime.Now, Data = action };
            }
            return ActionCache[id].Data;
        }

        public async Task<Activity> GetActivityAsync(int id)
        {
            if (!ActivityCache.ContainsKey(id) || IsCacheExpired(ActivityCache[id]))
            {
                // get activity from api
                Activity activity = await DownloadActivity(id);
                // add to cache
                ActivityCache[id] = new CacheItem<Activity> { CachedTime = DateTime.Now, Data = activity };
            }
            return ActivityCache[id].Data;
        }

        public async Task<Workplace> GetWorkplaceAsync(int id)
        {
            if (!WorkplaceCache.ContainsKey(id) || IsCacheExpired(WorkplaceCache[id]))
            {
                // get workplace from api
                Workplace workplace = await DownloadWorkplace(id);
                // add to cache
                WorkplaceCache[id] = new CacheItem<Workplace> { CachedTime = DateTime.Now, Data = workplace };
            }
            return WorkplaceCache[id].Data;
        }

        private async Task<Workplace> DownloadWorkplace(int id)
        {
            var res = await API.GetAsync(API.ROUTE.WORKPLACES + id);

            if (res.responseCode >= 200 && res.responseCode < 300)
            {
                JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
                return new Workplace(data);
            }

            switch (res.responseCode)
            {
                case 404: throw new WorkplaceNotFoundException(id);
                case 0: throw new TimeoutException();
                default: throw new Exception();
            }
        }

        private async Task<Artifact> DownloadArtifact(int id)
        {
            var res = await API.GetAsync(API.ROUTE.ARTIFACTS + id);

            if (res.responseCode >= 200 && res.responseCode < 300)
            {
                JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
                return new Artifact(data);
            }

            switch (res.responseCode)
            {
                case 404: throw new ArtifactNotFoundException(id);
                case 0: throw new TimeoutException();
                default: throw new Exception();
            }
        }

        private async Task<Models.Action> DownloadAction(int id)
        {
            var res = await API.GetAsync(API.ROUTE.ACTIONS + id);

            if (res.responseCode >= 200 && res.responseCode < 300)
            {
                JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
                return new Models.Action(data);
            }

            switch (res.responseCode)
            {
                case 404: throw new ActionNotFoundException(id);
                case 0: throw new TimeoutException();
                default: throw new Exception();
            }
        }


        /// <summary>
        /// Get activity from api
        /// </summary>
        /// <param name="id">activity id</param>
        /// <returns>activity</returns>
        /// <exception cref="WorkplaceNotFoundException">Thrown when activity not found</exception>
        private async Task<Activity> DownloadActivity(int id)
        {
            var res = await API.GetAsync(API.ROUTE.ACTIVITIES + id);
            if (res.responseCode == 404)
            {
                throw new ActivityNotFoundException(id);
            }
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Activity(data);
        }

        public async Task<Page<ActivityShort>> GetActivitiesAsync(int page, int pageSize, int workplaceId = 0)
        {
            var res = workplaceId > 0
                ? await API.GetAsync(API.ROUTE.ACTIVITIES_SEARCH + "?page=" + page + "&size=" + pageSize + "&workplace=" + workplaceId)
                : await API.GetAsync(API.ROUTE.ACTIVITIES + "?page=" + page + "&size=" + pageSize);
            if (res.responseCode == 404)
            {
                throw new WorkplaceNotFoundException(workplaceId);
            }
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Page<ActivityShort>(data);
        }

        public async Task<Page<WorkplaceShort>> GetWorkplacesAsync(int page, int pageSize)
        {
            var res = await API.GetAsync(API.ROUTE.WORKPLACES + "?page=" + page + "&size=" + pageSize);
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Page<WorkplaceShort>(data);
        }

        /// <summary>
        /// Get ressource of an action
        /// </summary>
        /// <param name="id">id of an action</param>
        /// <returns></returns>
        /// <exception cref="RessourceNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<Ressource> GetActionRessourceAsync(int id)
        {

            var action = await GetActionAsync(id);
            string ressourceName = action.GetRessourceName();
            Debug.Log("Ressource name : " + ressourceName);
            if (string.IsNullOrEmpty(ressourceName))
                throw new NoRessourceException(id);
            string fileExtension = GetFileExtension(ressourceName);
            string[] images = { "png", "jpg", "jpeg" };
            string[] videos = { "mp4", "webm" };
            string[] audios = { "mp3", "wav", "ogg" };
            if (fileExtension == null)
            {
                throw new RessourceNotFoundException(id, ressourceName);
            }
            if (images.Contains(fileExtension.ToLower()))
            {
                return await GetImageRessourceAsync(action.GetId(), action.GetRessourceName());
            }

            if (videos.Contains(fileExtension.ToLower()))
            {
                return await GetVideoRessourceAsync(action.GetId(), action.GetRessourceName());
            }

            if (audios.Contains(fileExtension.ToLower()))
            {
                return await GetAudioRessourceAsync(action.GetId(), action.GetRessourceName());
            }
            throw new Exception("Resource type not supported");
        }

        private async Task<AudioRessource> GetAudioRessourceAsync(int getId, string getRessourceName)
        {
            UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(API.API_URL + API.ROUTE.ACTIONS_DATA + getId + "/ressources/" + getRessourceName, GetAudioType(GetFileExtension(getRessourceName)));
            req.SetRequestHeader("Authorization", "Bearer " + AimXRManager.Instance.GetUser().token);
            TaskCompletionSource<AudioRessource> tcs = new TaskCompletionSource<AudioRessource>();
            req.SendWebRequest().completed += (asyncOperation) =>
            {
                if (req.result != UnityWebRequest.Result.Success)
                {
                    tcs.SetException(new Exception(req.error));
                }
                else
                {
                    tcs.SetResult(new AudioRessource(getId, getRessourceName, DownloadHandlerAudioClip.GetContent(req)));
                }
            };
            return await tcs.Task;
        }

        private AudioType GetAudioType(string fileExtension)
        {
            switch (fileExtension)
            {
                case "mp3":
                    return AudioType.MPEG;
                case "wav":
                    return AudioType.WAV;
                case "ogg":
                    return AudioType.OGGVORBIS;
                default:
                    return AudioType.UNKNOWN;
            }
        }

        private async Task<VideoRessource> GetVideoRessourceAsync(int getId, string getRessourceName)
        {
            // unity does not support VideoClip creation at runtime yet , you must use URL ase a source in the video player
            return new VideoRessource(getId, getRessourceName);
        }

        private async Task<Ressource> GetRessourceAsync(int id, string getRessourceName)
        {
            var res = await API.GetAsync(API.ROUTE.ACTIONS_DATA + id + "/ressources/" + getRessourceName);
            return null;
        }

        private async Task<ImageRessource> GetImageRessourceAsync(int id, string getRessourceName)
        {
            UnityWebRequest req = UnityWebRequestTexture.GetTexture(API.API_URL + API.ROUTE.ACTIONS_DATA + id + "/ressources/" + getRessourceName);
            req.SetRequestHeader("Authorization", "Bearer " + AimXRManager.Instance.GetUser().token);
            // wait for request to finish
            TaskCompletionSource<ImageRessource> tcs = new TaskCompletionSource<ImageRessource>();

            req.SendWebRequest().completed += (asyncOperation) =>
            {
                if (req.isNetworkError || req.isHttpError)
                {
                    Debug.LogError(req.error);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(req);
                    tcs.SetResult(new ImageRessource(id, getRessourceName, texture));
                }
            };
            return await tcs.Task;
        }

        private bool IsCacheExpired<T>(CacheItem<T> cache)
        {
            return (DateTime.Now - cache.CachedTime).TotalSeconds > 10;
        }

        static string GetFileExtension(string fileName)
        {
            string pattern = @"\.(.+)$";
            Match match = Regex.Match(fileName, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return string.Empty;
        }

        public void ClearCache()
        {
            ArtifactCache.Clear();
            ActivityCache.Clear();
            WorkplaceCache.Clear();
            ActionCache.Clear();
            TargetCache.Clear();
        }

        internal async Task<List<Activity>> GetCompatibleActivitiesAsync(int workplaceId)
        {
            var res = await API.GetAsync(API.ROUTE.ACTIVITIES + "?workplace=" + workplaceId);
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            List<Activity> activities = new List<Activity>();
            foreach (JsonData item in data["items"])
            {
                activities.Add(new Activity(item));
            }
            return activities;
        }
    }

    struct CacheItem<T>
    {
        public DateTime CachedTime;
        public T Data;
    }
}
