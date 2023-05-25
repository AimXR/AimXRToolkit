using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Models
{

    public class Activity
    {
        private readonly int _id;
        private readonly string _name;
        private readonly string _description;
        private readonly string _language;
        private readonly int _start;
        private readonly List<int> _artifacts;

        /// <summary>
        /// Construct an Activity object from a JsonData object
        /// </summary>
        /// <param name="data">JSON object of an activity from API</param>
        public Activity(JsonData data)
        {
            // get int id from data
            this._id = (int)data["id"];
            this._name = (string)data["name"];
            this._description = (string)data["description"];
            this._language = (string)data["language"];
            this._start = (int)data["start"];
            this._artifacts = JsonMapper.ToObject<List<int>>(data["artifacts"].ToJson());
        }
        /// <summary>
        /// id getter
        /// </summary>
        /// <returns>int id of the activity</returns>
        public int GetId()
        {
            return _id;
        }
        public string GetName()
        {
            return _name;
        }
        /// <summary>
        /// description getter
        /// </summary>
        /// <returns>description of the activity</returns>
        public string GetDescription()
        {
            return _description;
        }
        /// <summary>
        /// language getter
        /// </summary>
        /// <returns>ISO 639-1 string</returns>
        public string GetLanguage()
        {
            return _language;
        }
        /// <summary>
        /// start getter
        /// </summary>
        /// <returns>Id of the first action of the activity</returns>
        public int GetStart()
        {
            return _start;
        }
        public List<int> GetArtifacts()
        {
            return _artifacts;
        }
    }

}