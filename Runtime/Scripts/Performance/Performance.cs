using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;
using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using AimXRToolkit.Performance;
using System;

namespace AimXRToolkit.Performance
{
    /// <summary>
    /// Class use to send performance statements to the API
    /// </summary>
    public class Performance : MonoBehaviour
    {
        private string _platform = "UNITY EDITOR";

        private int _session;

        [SerializeField]
        [Tooltip("The activity manager of the scene")]
        // public ActivityManager activityManager;
        void Start()
        {
            _platform = Application.identifier + "/" + Application.version + " " + Application.productName;
            // if (activityManager == null)
            // {
            //     activityManager = GetComponent<ActivityManager>();
            //     if (activityManager == null)
            //         Debug.LogError("No ActivityManager found for this Performance Manager");
            // }
        }

        // Update is called once per frame
        void Update()
        {

        }
        // action<Verb template> method
        private async Task<JsonData> SendActionStatement(Verb verb, Models.Action action)
        {
            if (action == null)
            {
                Debug.LogFormat("Can't send statement with verb {0} because action is null", verb.Value);
                return null;
            }
            var res = await SendPerformanceStatementAsync(verb, action.ToPerformanceObject(), new PerformanceContext(_platform, AimXRManager.Instance.GetUser().language, 0, 0, _session));
            return res;
        }
        private async Task<JsonData> SendActivityStatement(Verb verb, Models.Activity activity)
        {
            if (activity == null)
            {
                Debug.LogFormat("Can't send statement with verb {0} because activity is null", verb.Value);
                return null;
            }
            return await SendPerformanceStatementAsync(verb, activity.ToPerformanceObject(), new PerformanceContext(_platform, AimXRManager.Instance.GetUser().language, 0, 0, _session));
        }
        /// <summary>
        /// User has started an action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async void ActionStart(Models.Action action)
        {
            _ = await SendActionStatement(Verb.Start, action);
        }
        /// <summary>
        /// User has completed an action
        /// </summary>
        /// <param name="action"></param>
        public async void ActionComplete(Models.Action action)
        {
            _ = await SendActionStatement(Verb.Complete, action);
        }

        /// <summary>
        /// User skipped an action
        /// </summary>
        /// <param name="action"></param>
        public async void ActionSkip(Models.Action action)
        {
            _ = await SendActionStatement(Verb.Skip, action);
        }

        /// <summary>
        /// User has requested help for an action
        /// </summary>
        /// <param name="action">Current action</param>
        public async void ActionHelp(Models.Action action)
        {
            _ = await SendActionStatement(Verb.Help, action);
        }
        public async void ActionRepeat(Models.Action action)
        {
            _ = await SendActionStatement(Verb.Repeat,action);
        }
        /// <summary>
        /// User has started an activity
        /// </summary>
        /// <param name="activity">started activity</param>
        public async Task ActivityStart(Models.Activity activity)
        {
            var res = await SendActivityStatement(Verb.Start, activity);
            this._session = (int)res["context"]["session"];
            await Task.CompletedTask;
        }

        public async void ActivityEnd(Models.Activity activity)
        {
            _ = await SendActivityStatement(Verb.Complete, activity);
        }


        public async Task<JsonData> SendPerformanceStatementAsync(Verb verb, PerformanceObject performanceObject, PerformanceContext context)
        {
            if (AimXRManager.Instance.GetUser() == null)
                throw new Exception("User must be logged in");
            JsonData jsonBody = new JsonData();
            jsonBody["actor"] = AimXRManager.Instance.GetUser().id;
            jsonBody["verb"] = verb.Value;
            jsonBody["object"] = performanceObject.ToJson();
            jsonBody["context"] = context.ToJson();
            jsonBody["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffffzzz");
            var res = await API.ExecuteLoggedAsync(API.ROUTE.PERFORMANCE + "statements", API.Method.Post, AimXRManager.Instance.GetUser().token, API.Type.Json, jsonBody.ToJson());
            if (res.responseCode != 200)
            {
                Debug.LogError("Error while sending performance statement: " + res.error);
                // log json error
                JsonData jsonError = JsonMapper.ToObject(res.downloadHandler.text);
                Debug.LogError(jsonError.ToJson());
                return null;
            }
            return JsonMapper.ToObject(res.downloadHandler.text);
        }
        public void Reset(){
            this._session = 0;
        }
    }

    public class PerformanceObject
    {
        private int id;
        private string objectType;

        protected PerformanceObject(int id, string objectType)
        {
            this.id = id;
            this.objectType = objectType;
        }

        public JsonData ToJson()
        {
            JsonData json = new JsonData
            {
                ["id"] = id,
                ["objectType"] = objectType
            };
            return json;
        }
    }

    public class PerformanceContext
    {
        private readonly string _platform;
        private readonly string _language;
        private readonly int _activity;
        private readonly int _action;
        private readonly int _session;

        public PerformanceContext(string platform)
        {
            _platform = platform;
        }
        public PerformanceContext(string platform, string language)
        {
            _platform = platform;
            _language = language;
        }
        public PerformanceContext(string platform, string language, int activity, int action, int session)
        {
            _platform = platform;
            _language = language;
            _activity = activity;
            _action = action;
            _session = session;
        }

        public JsonData ToJson()
        {
            // add only field that are not null so not null or "" or 0
            JsonData json = new JsonData();
            if (!string.IsNullOrEmpty(_platform))
            {
                json["platform"] = _platform;
            }

            if (!string.IsNullOrEmpty(_language))
            {
                json["language"] = _language;
            }

            if (_activity != 0)
            {
                json["activity"] = _activity;
            }

            if (_action != 0)
            {
                json["action"] = _action;
            }

            if (_session != 0)
            {
                json["session"] = _session;
            }
            return json;
        }
    }

    public class ActivityObject : PerformanceObject
    {
        public ActivityObject(int id) : base(id, "activity")
        {
        }
    }

    public class ActionObject : PerformanceObject
    {
        public ActionObject(int id) : base(id, "action")
        {
        }
    }

    public class AgentObject : PerformanceObject
    {
        public AgentObject(int id) : base(id, "agent")
        {
        }
    }

    public class TargetObject : PerformanceObject
    {
        public TargetObject(int id) : base(id, "target")
        {
        }
    }

    /// <summary>
    /// Contains all verbs supported by the API
    /// </summary>
    public class Verb
    {
        private Verb(string value) { Value = value; }
        public string Value { get; private set; }
        public static Verb Complete => new Verb("complete");
        public static Verb Start => new Verb("start");
        public static Verb Interact => new Verb("interact");
        public static Verb Launch => new Verb("launch");
        public static Verb Help => new Verb("help");
        public static Verb Press => new Verb("press");
        public static Verb Release => new Verb("release");
        public static Verb Pause => new Verb("pause");
        public static Verb Resume => new Verb("resume");
        public static Verb Skip => new Verb("skip");

        public static Verb View => new Verb("view");
        public static Verb Repeat => new Verb("repeat");

    }

    public struct Context
    {
        public string platform;
        public string language;
        public int activity;
        public int action;
        public string session;
    }

}
