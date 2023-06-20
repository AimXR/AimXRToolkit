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

using UnityEditor;
using AimXRToolkit;
using UnityEngine;
using AimXRToolkit.Managers;
using LitJson;
using AimXRToolkit.Models;
using System;
using System.Threading.Tasks;
namespace AimXRToolkit.Helpers
{

    // namespace AimXRToolkit.Helpers;
    [CustomEditor(typeof(AimXRManager))]
    class AimXRHelper : Editor
    {

        private AimXRManager _aimXRManager;
        private ActivityManager _activityManager;
        private WorkPlaceManager _workPlaceManager;
        private WorkplacePagination _workplacePagination;
        private string _activityId = "";
        private int _workplaceId = 0;
        string _username = "";
        string _password = "";


        int paginationSize = 10;
        void OnEnable()
        {
            _aimXRManager = (AimXRManager)target;
            // find the activity manager in the scene
            _activityManager = FindObjectOfType<ActivityManager>();
            _workPlaceManager = FindObjectOfType<WorkPlaceManager>();
            if (_activityManager == null)
            {
                Debug.LogError("No ActivityManager found in the scene");
            }
        }
        public override async void OnInspectorGUI()
        {
            DrawDefaultInspector();
            // if not in play mode , display a message that it works only in play mode
            if (!Application.isPlaying)
            {
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("This helper works only in play mode", MessageType.Info);
                return;
            }

            // draw username label with field next to it
            GUILayout.BeginHorizontal();
            GUILayout.Label("Username : ");
            _username = GUILayout.TextField(_username, 25, GUILayout.Width(100));
            GUILayout.EndHorizontal();
            // same with password
            GUILayout.BeginHorizontal();
            GUILayout.Label("Password : ");
            _password = GUILayout.PasswordField(_password, '*', 100, GUILayout.Width(100));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            // button to login
            if (GUILayout.Button("Login"))
            {
                bool res = await login(_username, _password);
                if (!res)
                {
                    EditorUtility.DisplayDialog("Error", "Invalid credentials", "OK");
                    return;
                }
                // dire bonjour � l'utilisateur
                EditorUtility.DisplayDialog("Success", "Welcome " + _aimXRManager.GetUser().firstname + " " + _aimXRManager.GetUser().lastname, "OK");
                return;
            }

            if (_aimXRManager.GetUser() != null)
            {
                if (GUILayout.Button("Logout"))
                {
                    AimXRManager.Instance.SetUser(null);
                    return;
                }
            }
            GUILayout.EndHorizontal();


            // label for id and input 
            GUILayout.BeginHorizontal();
            GUILayout.Label("Activity ID : ");
            // input for id
            //EditorGUI.IntField(new Rect(0, 0, 100, 100), value);
            _activityId = GUILayout.TextField(_activityId, 10, GUILayout.Width(100));
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Load Activity"))
            {
                try
                {
                    Activity activity;
                    activity = await DataManager.GetInstance().GetActivityAsync(int.Parse(_activityId));
                    _activityManager.SetActivity(activity);
                }
                catch (Exception)
                {
                    EditorUtility.DisplayDialog("Error", "Invalid activity ID", "OK");
                }
                return;
            }

            if (GUILayout.Button("Clear cache"))
            {
                DataManager.GetInstance().ClearCache();
                Debug.Log("Cache cleared");
                return;
            }

            if (_activityManager.GetActivity() != null)
            {
                // titre de l'activite
                GUILayout.Label("Activite : " + _activityManager.GetActivity().GetName());
                // description de l'activite
                GUILayout.Label("Description : " + _activityManager.GetActivity().GetDescription());
            }

            {
                // previous and next action buttons next to each other
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Previous action"))
                {
                    await _activityManager.PreviousAction();
                    return;
                }

                if (GUILayout.Button("TTS"))
                {
                    // if no token , display error message
                    if (string.IsNullOrEmpty(_aimXRManager?.GetUser().token))
                    {
                        EditorUtility.DisplayDialog("Error", "You must be logged in to use TTS", "OK");
                        return;
                    }
                    TTS tts = new TTS();
                    AudioSource audioSource = FindObjectOfType<AudioSource>();
                    if (audioSource == null)
                    {
                        audioSource = this._aimXRManager.gameObject.AddComponent<AudioSource>();
                    }
                    tts.speak(_activityManager.GetCurrentAction().GetName(), Language.FR, audioSource);
                }
                if (GUILayout.Button("Next action"))
                {
                    await _activityManager.NextAction();
                    return;
                }
                GUILayout.EndHorizontal();
            }

            // trait horizontal
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(5));
            if (_activityManager.GetCurrentAction() != null)
            {
                // titre de l'action
                GUILayout.Label("Action : " + _activityManager.GetCurrentAction().GetName());
                // description de l'action
                GUILayout.Label("Description : " + _activityManager.GetCurrentAction().GetDescription());
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label("Workplace ID : ");
            _workplaceId = EditorGUILayout.IntField(_workplaceId, GUILayout.Width(100));
            // spawn work place button
            if (GUILayout.Button("Load WorkPlace"))
            {
                if (_workPlaceManager == null)
                {
                    Debug.LogError("No WorkPlaceManager found in the scene");
                    return;
                }
                _workPlaceManager.SetWorkplace(await DataManager.GetInstance().GetWorkplaceAsync(_workplaceId));
                _workPlaceManager.Spawn();
                return;
            }
            GUILayout.EndHorizontal();

            // horizontal line
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(10));

            // pagination size
            GUILayout.BeginHorizontal();
            GUILayout.Label("Pagination size : ");
            paginationSize = EditorGUILayout.IntField(paginationSize, GUILayout.Width(100));
            _workplacePagination?.SetSize(paginationSize);
            GUILayout.EndHorizontal();

            // get list of work places
            if (GUILayout.Button("Get WorkPlaces"))
            {
                _workplacePagination ??= new WorkplacePagination(paginationSize);
                await _workplacePagination.LoadNextPage();
                return;
            }
            if (_workplacePagination != null && _workplacePagination.GetCurrentPage() != null)
            {
                // display list of work places in a scroll view
                GUILayout.BeginVertical();
                foreach (var workplace in _workplacePagination.GetCurrentPage().GetItems())
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("ID : " + workplace.GetId());
                    GUILayout.Label("Name : " + workplace.GetName());
                    GUILayout.Label("Description : " + workplace.GetDescription());
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                // previous and next page buttons
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Previous page"))
                {
                    await _workplacePagination.LoadPreviousPage();
                    return;
                }
                if (GUILayout.Button("Next page"))
                {
                    await _workplacePagination.LoadNextPage();
                    return;
                }
                GUILayout.EndHorizontal();
            }

            // horizontal line
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(10));
        }
        private async Task<bool> login(string username, string password)
        {

            var res = await API.ExecuteAsync(API.ROUTE.LOGIN, API.Method.Post, API.Type.Form, "username=" + _username + "&password=" + _password);
            if (res.responseCode != 200)
            {
                Debug.Log(res.downloadHandler.text);
                Debug.Log("username " + _username + " password " + _password);
                return false;
            }
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            string tokenString = (string)data["token_type"] + " " + (string)data["access_token"];
            Debug.Log(tokenString);

            User candidateUser = await User.GetUserFromToken(tokenString);
            Debug.Log("user : " + candidateUser);

            if (candidateUser == null)
                return false;
            AimXRManager.Instance.SetUser(candidateUser);
            return true;
        }
    }
}
