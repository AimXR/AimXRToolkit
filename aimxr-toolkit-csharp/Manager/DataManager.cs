using LitJson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using AimXRToolkit.Models;

namespace AimXRToolkit.Managers
{
    public sealed class DataManager
    {
        private static DataManager? Instance;
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
        private DataManager()
        {
            ArtifactCache = new Dictionary<int, CacheItem<Artifact>>();
            ActivityCache = new Dictionary<int, CacheItem<Activity>>();
            WorkplaceCache = new Dictionary<int, CacheItem<Workplace>>();
            ActionCache = new Dictionary<int, CacheItem<Models.Action>>();
            TargetComponentsCache = new();
            TargetCache = new();
            ComponentCache = new();
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
            if (res.responseCode == 404)
            {
                throw new ComponentNotFoundException(id);
            }
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Models.Component(data);
        }

        private async Task<Models.Target> DownloadTarget(int id)
        {
            var res = await API.GetAsync(API.ROUTE.TARGETS + id);
            if (res.responseCode == 404)
            {
                throw new TargetNotFoundException(id);
            }
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Target(data);
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
            if (res.responseCode == 404)
            {
                throw new WorkplaceNotFoundException(id);
            }
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Workplace(data);
        }

        private async Task<Artifact> DownloadArtifact(int id)
        {
            var res = await API.GetAsync(API.ROUTE.ARTIFACTS + id);
            if (res.responseCode == 404)
            {
                throw new ArtifactNotFoundException(id);
            }
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Artifact(data);
        }

        private async Task<Models.Action> DownloadAction(int id)
        {
            var res = await API.GetAsync(API.ROUTE.ACTIONS + id);
            if (res.responseCode == 404)
            {
                throw new ActionNotFoundException(id);
            }
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            Debug.Log(data.ToJson());
            return new Models.Action(data);
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

        public async Task<Page<Activity>> GetActivitiesAsync(int page, int pageSize)
        {
            var res = await API.GetAsync(API.ROUTE.ACTIVITIES + "?page=" + page + "&size=" + pageSize);
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Page<Activity>(data);
        }

        public async Task<Page<WorkplaceShort>> GetWorkplacesAsync(int page, int pageSize)
        {
            var res = await API.GetAsync(API.ROUTE.WORKPLACES + "?page=" + page + "&size=" + pageSize);
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new Page<WorkplaceShort>(data);
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
            TargetCache.Clear();
        }

        internal async Task<List<Activity>> GetCompatibleActivitiesAsync(int workplaceId)
        {
            var res = await API.GetAsync(API.ROUTE.ACTIVITIES + "?workplace=" + workplaceId);
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            List<Activity> activities = new();
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
