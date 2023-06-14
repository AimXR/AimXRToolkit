using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;
using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using AimXRToolkit.Performance;

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
        public ActivityManager? activityManager;
        void Start()
        {
            _platform = Application.identifier + "/" + Application.version + " " + Application.productName;
            if (activityManager == null)
            {
                activityManager = GetComponent<ActivityManager>();
                if (activityManager == null)
                    Debug.LogError("No ActivityManager found for this Performance Manager");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        // action<Verb template> method
        private async Task<JsonData?> SendActionStatement(Verb verb, Models.Action action)
        {
            if (action == null)
            {
                Debug.LogFormat("Can't send statement with verb {0} because action is null", verb.Value);
                return null;
            }
            var res = await SendPerformanceStatementAsync(verb, action.ToPerformanceObject(), new PerformanceContext(_platform, AimXRManager.Instance.GetUser().language, 0, 0, _session));
            return res;
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
        /// <returns></returns>
        public async void ActionComplete(Models.Action action)
        {
            _ = await SendActionStatement(Verb.Complete, action);
        }

        /// <summary>
        /// User skipped an action
        /// </summary>
        /// <param name="action"></param>
        /// <returns>Statement return by the LRS in JSON</returns>
        public async void ActionSkip(Models.Action action)
        {
            _ = await SendActionStatement(Verb.Skip, action);
        }

        /// <summary>
        /// User has requested help for an action
        /// </summary>
        /// <param name="action"></param>
        /// <returns>Statement return by the LRS in JSON</returns>
        public async void ActionHelp(Models.Action action)
        {
            _ = await SendActionStatement(Verb.Help, action);
        }

        public async Task<JsonData?> SendPerformanceStatementAsync(Verb verb, PerformanceObject performanceObject, PerformanceContext context)
        {
            JsonData jsonBody = new JsonData();
            jsonBody["actor"] = 1;
            jsonBody["verb"] = verb.Value;
            jsonBody["object"] = performanceObject.ToJson();
            jsonBody["context"] = context.ToJson();
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
        private readonly string? _language;
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
