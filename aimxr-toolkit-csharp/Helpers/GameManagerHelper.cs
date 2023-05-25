using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace AimXRToolkit.Helpers;

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerHelper : Editor
{
    string _username = "";
    string _password = "";
    public override async void OnInspectorGUI()
    {
        DrawDefaultInspector();
        // draw username label with field next to it
        GUILayout.BeginHorizontal();
        GUILayout.Label("Username : ");
        _username = GUILayout.TextField(_username, 25, GUILayout.Width(100));
        GUILayout.EndHorizontal();
        // same with password
        GUILayout.BeginHorizontal();
        GUILayout.Label("Password : ");
        _password = GUILayout.PasswordField(_password, '*', 25, GUILayout.Width(100));
        GUILayout.EndHorizontal();
        // button to login
        if (GUILayout.Button("Login"))
        {
            bool res = await login(_username, _password);
            if (!res)
            {
                EditorUtility.DisplayDialog("Error", "Invalid credentials", "OK");
                return;
            }
            // dire bonjour à l'utilisateur
            EditorUtility.DisplayDialog("Success", "Welcome " + GameManager.instance.getUser().firstname + " " + GameManager.instance.getUser().lastname, "OK");
        }
    }

    private async Task<bool> login(string username, string password)
    {

        var res = await API.ExecuteAsync(API.ROUTE.LOGIN, API.Method.Post, API.Type.Form, "username=" + _username + "&password=" + _password);
        if (res.responseCode != 200)
        {
            return false;
        }
        JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
        string tokenString = (string)data["token_type"] + " " + (string)data["access_token"];

        User candidateUser = await User.GetUserFromToken(tokenString);

        if (candidateUser == null)
            return false;
        GameManager.instance.SetUser(candidateUser);
        return true;
    }
}

#endif