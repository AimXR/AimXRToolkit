// Copyright (C) 2023 Antonin Rousseau
// 
// aimxr-toolkit-csharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// aimxr-toolkit-csharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with aimxr-toolkit-csharp. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
namespace AimXRToolkit
{
    public class API
    {
        public static string UserAgent => Application.identifier + "/" + Application.version + " " + Application.productName;

        // ROOT URL of the API
        private static string _API_URL = "https://api.aimxr.io";
        public static string API_URL
        {
            get => _API_URL;
            set => _API_URL = value;
        }
        /// <summary>
        /// API's available routes
        public struct ROUTE
        {
            public static string LOGIN = "/auth/token/";
            public static string PASSWORD = "/auth/password/";
            public static string SCENARIOS = "/scenarios/";
            public static string ACTIVITIES = "/activities/";
            public static string ACTIONS = "/actions/";
            public static string ARTIFACTS = "/artifacts/";
            public static string WORKPLACES = "/workplaces/";
            public static string COMPONENTS = "/components/";
            public static string TARGETS = "/targets/";
            public static string PERFORMANCE = "/performance/";
            public static string USERS = "/users/";
            public static string USER = "/users/me/";
            public static string EASY_GENERATE = "/easy/generate/";
            public static string EASY_CODE = "/easy/";
            public static string LANGUAGES = "/langs/";
            [Obsolete("Use /lang/tts instead")]
            public static string TTS = "/tts/";
            public struct STATS
            {
                public static string USERS = "/stats/users/";
                public static string __SESSIONS = "/sessions/";
                public static string SESSIONS = "/stats/sessions/";
                public static string __PLAYED_STEPS = "/playedSteps/";
                public static string PLAYED_STEPS = "/stats/sessions/playedSteps/";
            }
            public static string ACTIONS_DATA = "/data/actions/";
            public static string ACTIVITIES_SEARCH = "/activities/search";
        }

        /// <summary>
        /// API's available request methods
        /// </summary>
        public struct Method
        {
            private Method(string value) { Value = value; }
            public string Value { get; set; }
            public static Method Get { get { return new Method("GET"); } }
            public static Method Post { get { return new Method("POST"); } }
            public static Method Put { get { return new Method("PUT"); } }
            public static Method Delete { get { return new Method("DELETE"); } }
            public static bool operator ==(Method a, Method b) => a.Value == b.Value;
            public static bool operator !=(Method a, Method b) => a.Value != b.Value;

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                    return false;
                return Value == ((Method)obj).Value;
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }

        /// <summary>
        /// API's available request types
        /// </summary>
        public enum Type { Json, Form, None };

        public static async Task<UnityWebRequest> GetAsync(string path, Dictionary<string, string> headers = null)
        {
            return await ExecuteAsync(path, Method.Get, Type.None, "", headers);
        }
        public static async Task<UnityWebRequest> PostAsync(string path, string body, Type type = Type.Json, Dictionary<string, string> headers = null)
        {
            return await ExecuteAsync(path, Method.Post, type, body, headers);
        }

        public static async Task<UnityWebRequest> ExecuteAsync(string path, Method method, Type type = Type.None, string body = "", Dictionary<string, string> headers = null)
        {
            UnityWebRequest request = new UnityWebRequest();
            request.url = _API_URL + path;
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw((string.IsNullOrEmpty(body) || method == Method.Get) ? null : System.Text.Encoding.UTF8.GetBytes(body));
            request.timeout = 10; // sets the request timeout to 10 seconds

            // default request headers
            request.SetRequestHeader("User-Agent", Application.identifier + "/" + Application.version + " " + Application.productName);
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Accept-Language", "fr,fr-FR;q=0.8,en-US;q=0.5,en;q=0.3");

            // if additionnal headers are given, include them
            if (headers != null)
                foreach (string key in headers.Keys)
                    request.SetRequestHeader(key, headers[key]);

            // apply the desired request METHOD
            request.method = method.Value;
            // apply the desired request TYPE
            switch (type)
            {
                case Type.Json: request.SetRequestHeader("Content-Type", "application/json"); break;
                case Type.Form: request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); break;
                default: break;
            }
            TaskCompletionSource<UnityWebRequest> tcs = new TaskCompletionSource<UnityWebRequest>();
            request.SendWebRequest().completed += (op) =>
            {
                tcs.SetResult(request);
            };
            return await tcs.Task;
        }

        public static async Task<UnityWebRequest> ExecuteLoggedAsync(string path, Method method, string token, Type type = Type.None, string body = "", Dictionary<string, string> headers = null)
        {
            UnityWebRequest request = new UnityWebRequest();
            request.url = _API_URL + path;
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw((string.IsNullOrEmpty(body) || method == Method.Get) ? null : System.Text.Encoding.UTF8.GetBytes(body));
            request.timeout = 10; // sets the request timeout to 10 seconds
                                  // default request headers
            request.SetRequestHeader("User-Agent", Application.identifier + "/" + Application.version + " " + Application.productName);
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Accept-Language", "fr,fr-FR;q=0.8,en-US;q=0.5,en;q=0.3");
            // if additionnal headers are given, include them
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                    request.SetRequestHeader(key, headers[key]);
            }
            // apply the desired request METHOD
            request.method = method.Value;
            // apply the desired request TYPE
            switch (type)
            {
                case Type.Json: request.SetRequestHeader("Content-Type", "application/json"); break;
                case Type.Form: request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); break;
                default: break;
            }
            request.SetRequestHeader("Authorization", "Bearer " + token);
            TaskCompletionSource<UnityWebRequest> tcs = new TaskCompletionSource<UnityWebRequest>();
            request.SendWebRequest().completed += (op) => tcs.SetResult(request);
            return await tcs.Task;
        }
    }
    public class NotFoundException : Exception
    {
        public NotFoundException(string obj, int id) : base(obj + " with id " + id + " not found") { }
    }

    public class ActivityNotFoundException : NotFoundException
    {
        public ActivityNotFoundException(int id) : base("Activity", id) { }

    }
    public class TargetNotFoundException : NotFoundException
    {
        public TargetNotFoundException(int id) : base("Target", id) { }
    }

    public class ComponentNotFoundException : NotFoundException
    {
        public ComponentNotFoundException(int id) : base("Component", id) { }
    }

    public class ArtifactNotFoundException : NotFoundException
    {
        public ArtifactNotFoundException(int id) : base("Artifact", id) { }
    }

    public class WorkplaceNotFoundException : NotFoundException
    {
        public WorkplaceNotFoundException(int id) : base("Workplace", id) { }
    }

    public class ActionNotFoundException : NotFoundException
    {
        public ActionNotFoundException(int id) : base("Action", id) { }
    }
}
