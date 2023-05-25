using LitJson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AimXRToolkit.Managers
{
    public class DataManager
    {
        public static DataManager Instance;
        // api url
        [SerializeField]
        [Tooltip("The url of the api")]
        public static string APIUrl = "http://localhost:8000";
        // artifact cache dictionary
        private Dictionary<int, CacheItem<Models.Artifact>> ArtifactCache;
        private Dictionary<int, CacheItem<Models.Activity>> ActivityCache;
        private Dictionary<int, string> WorkplaceCache;
        private Dictionary<int, CacheItem<Models.Action>> ActionCache;
        private DataManager()
        {
            ArtifactCache = new Dictionary<int, CacheItem<Models.Artifact>>();
            ActivityCache = new Dictionary<int, CacheItem<Models.Activity>>();
            WorkplaceCache = new Dictionary<int, string>();
            ActionCache = new Dictionary<int, CacheItem<Models.Action>>();
        }

        public static DataManager GetInstance()
        {
            if (Instance == null)
            {
                Instance = new DataManager();
            }
            return Instance;
        }

        public async Task<Models.Artifact> GetArtifactAsync(int id)
        {
            if (!ArtifactCache.ContainsKey(id) || IsCacheExpired(ArtifactCache[id]))
            {
                // get artifact from api
                Models.Artifact artifact = await DownloadArtifact(id);
                // add to cache
                ArtifactCache[id] = new CacheItem<Models.Artifact> { CachedTime = DateTime.Now, Data = artifact };
            }
            return ArtifactCache[id].Data;
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

        public async Task<Models.Activity> GetActivityAsync(int id)
        {
            if (!ActivityCache.ContainsKey(id) || IsCacheExpired(ActivityCache[id]))
            {
                // get activity from api
                Models.Activity activity = await DownloadActivity(id);
                // add to cache
                ActivityCache[id] = new CacheItem<Models.Activity> { CachedTime = DateTime.Now, Data = activity };
            }
            return ActivityCache[id].Data;
        }

        private async Task<Models.Artifact> DownloadArtifact(int id)
        {
            var res = await API.GetAsync(API.ROUTE.ARTIFACTS + id);
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Models.Artifact(data);
        }

        private async Task<Models.Action> DownloadAction(int id)
        {
            var res = await API.GetAsync(API.ROUTE.ACTIONS + id);
            if (res.responseCode == 404)
            {
                return null;
            }
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            Debug.Log(data.ToJson());
            return new Models.Action(data);
        }

        private async Task<Models.Activity> DownloadActivity(int id)
        {
            var res = await API.GetAsync(API.ROUTE.ACTIVITIES + id);
            if(res.responseCode == 404)
            {
                throw new ActivityNotFoundException(id);
            }
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Models.Activity(data);
        }
        private bool IsCacheExpired<T>(CacheItem<T> cache)
        {
            return (DateTime.Now - cache.CachedTime).TotalSeconds > 10;
        }

        public void ClearCache()
        {
            ArtifactCache.Clear();
            ActivityCache.Clear();
            WorkplaceCache.Clear();
            ActionCache.Clear();
        }
    }

    struct CacheItem<T>
    {
        public DateTime CachedTime;
        public T Data;
    }
}

