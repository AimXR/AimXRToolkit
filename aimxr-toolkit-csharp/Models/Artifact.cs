using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Models
{
    public struct Anchor
    {
        public Vector3 position;
        public Vector3 rotation;

        public Vector3 GetPosition() { return position; }
        public Vector3 GetRotation() { return rotation; }
    }
    public class Artifact : IPaginable
    {
        private readonly int _id;
        private readonly string _name;
        private readonly string _description;
        private readonly string _language;
        private readonly List<int> _targets;
        private readonly Anchor _anchor;

        public Artifact(JsonData data)
        {
            this._id = (int)data["id"];
            this._name = (string)data["name"];
            this._description = (string)data["description"];
            this._language = (string)data["language"];
            this._targets = JsonMapper.ToObject<List<int>>(data["targets"].ToJson());
            this._anchor = new Anchor
            {
                position = new Vector3((float)(double)data["anchor"]["position"]["x"], (float)(double)data["anchor"]["position"]["y"], (float)(double)data["anchor"]["position"]["z"]),
                rotation = new Vector3((float)(double)data["anchor"]["rotation"]["x"], (float)(double)data["anchor"]["rotation"]["y"], (float)(double)data["anchor"]["rotation"]["z"])
            };
        }
        public override string ToString()
        {
            return "Artifact: " + _name + " id: " + _id + " description: " + _description + " language: " + _language + " targets: " + _targets + " anchor: " + _anchor;
        }

        public Anchor GetAnchor() { return _anchor; }
        public int GetId() { return _id; }
        public string GetName() { return _name; }
        public string GetDescription() { return _description; }
        public string GetLanguage() { return _language; }
        public List<int> GetTargets() { return _targets; }

    }

}
