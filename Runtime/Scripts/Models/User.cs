using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;

namespace AimXRToolkit.Models
{
    /// <summary>
    /// User class, used to store User's basic informations in the game
    /// </summary>
    public class User
    {
        /// <summary>
        /// Permissions available for a User such as Visitor, Learner, Teacher, Admin
        /// </summary>
        public enum PERMISSIONS { VISITOR, LEARNER, TEACHER, ADMIN };

        /// <summary>
        /// Unique ID of the app's user
        /// </summary>
        public int id;
        /// <summary>
        /// Username of the app's user
        /// </summary>
        public string? username;
        /// <summary>
        /// Password of the app's user
        /// </summary>
        public string? password;
        /// <summary>
        /// Token of the app's user
        /// </summary>
        public string? token;
        /// <summary>
        /// Token type of the app's user
        /// </summary>
        public string? tokenType;
        /// <summary>
        /// First name of the app's user
        /// </summary>
        public string firstname;
        /// <summary>
        /// Last name of the app's user
        /// </summary>
        public string lastname;
        /// <summary>
        /// Email of the app's user
        /// </summary>
        public string email;
        /// <summary>
        /// Permissions of the app's user (see User.PERMISSIONS for more info)
        /// </summary>
        public PERMISSIONS permissions;

        public string language;

        /// <summary>
        /// Returns the full name of the user, based on his firstname and lastname.
        /// </summary>
        public string FullName
        {
            get { return firstname + " " + lastname; }
        }

        public void Awake()
        {
        }
        /// <summary>
        /// Makes an API request to retreive all the User's informations based on his credentials.
        /// If the User's token is not defined, this function will first retreive his token using
        /// the user's username and password.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonData> FetchInformations()
        {
            var res = await API.ExecuteLoggedAsync(API.ROUTE.USER, API.Method.Get, this.token, API.Type.Json);
            if (res.responseCode != 200)
            {
                throw new Exception("Error while fetching user informations: " + res.error);
            }

            LitJson.JsonData data = LitJson.JsonMapper.ToObject(res.downloadHandler.text);
            this.id = (int)data["id"];
            this.username = (string)data["username"];
            this.firstname = (string)data["firstname"];
            this.lastname = (string)data["lastname"];
            this.email = (string)data["email"];
            Debug.Log(data["language_code"]);
            Debug.Log(data["language_code"].GetType());
            Debug.Log((string)data["language_code"]);
            this.language = data["language_code"] == null ? "" : (string)data["language_code"];
            this.permissions = (PERMISSIONS)System.Enum.Parse(typeof(PERMISSIONS), (string)data["adminLevel"]);
            return data;

        }

        /// <summary>
        /// User GET /user/me API route to retreive the user's informations based on his token.
        /// </summary>
        /// <returns>User if token is valid else null</returns>
        public static async Task<User> GetUserFromToken(string token)
        {
            var res = await API.ExecuteAsync(API.ROUTE.USERS + "me", API.Method.Get, API.Type.None, "",
                new Dictionary<string, string>() { { "Authorization", token } }
                );
            Debug.Log(res.downloadHandler.text);
            if (res.responseCode != 200) return null;
            JsonData data = JsonMapper.ToObject(res.downloadHandler.text);
            return new()
            {
                id = (int)data["id"],
                token = token,
                username = (string)data["username"],
                firstname = (string)data["firstname"],
                lastname = (string)data["lastname"],
                email = (string)data["email"],
                // language = (string)data["language_code"],
                // handle null string
                language = data["language_code"] == null ? "" : (string)data["language_code"],
                permissions = (PERMISSIONS)(int)data["adminLevel"]
            };
        }
    }
}