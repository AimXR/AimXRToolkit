using LitJson;
using UnityEngine;
using AimXRToolkit.Performance;

namespace AimXRToolkit.Models
{
    public class Action : IPerformanceCapable
    {

        private readonly int _artifact;
        private readonly int _id;
        private readonly int _next;
        private readonly int _previous;
        private readonly string _description;
        private readonly string _hint;
        private readonly string _language;
        private readonly string _name;
        private readonly string _ressource;
        private readonly string _tag;
        private readonly string _type;
        private readonly Vector3 _position;
        private Choice _choice;

        public Action(JsonData data)
        {
            this._artifact = Utilities.NullableInteger(data, "artifactID");
            this._id = (int)data["id"];
            this._next = Utilities.NullableInteger(data, "next");
            this._previous = Utilities.NullableInteger(data, "previous");
            this._description = (string)data["description"];
            this._hint = (string)data["hint"];
            //this._language = (string)data["language"];
            this._name = (string)data["name"];
            this._ressource = Utilities.NullableString(data, "ressource");
            this._tag = (string)data["tag"];
            this._type = (string)data["type"];
            this._position = new Vector3((float)(double)data["position"]["x"], (float)(double)data["position"]["y"], (float)(double)data["position"]["z"]);
            if (data["choice"] != null && this._type == "choice")
            {
                this._choice = new Choice(data["choice"]);
            }
        }


        public int GetArtifact()
        {
            return _artifact;
        }
        /// <summary>
        /// getter for id
        /// </summary>
        /// <returns>int id of the action</returns>
        public int GetId()
        {
            return _id;
        }
        /// <summary>
        /// getter for next
        /// </summary>
        /// <returns>int id of the next action</returns>
        public int GetNext()
        {
            return _next;
        }
        /// <summary>
        /// getter for previous
        /// </summary>
        /// <returns>int id of the previous action</returns>
        public int GetPrevious()
        {
            return _previous;
        }
        public string GetDescription()
        {
            return _description;
        }
        public string GetHint()
        {
            return _hint;
        }
        /// <summary>
        /// getter for language
        /// </summary>
        /// <returns>ISO 639-1 string</returns>
        public string GetLanguage()
        {
            return _language;
        }
        /// <summary>
        /// getter for name
        /// </summary>
        /// <returns> name of the action in the language specified by language class member </returns>
        public string GetName()
        {
            return _name;
        }
        /// <summary>
        /// getter for ressource
        /// </summary>
        /// <returns> path to the ressource </returns>
        public string GetRessourceName()
        {
            return _ressource;
        }
        /// <summary>
        /// getter for tag
        /// </summary>
        /// <returns> tag of the action </returns>
        public string GetTag()
        {
            return _tag;
        }
        /// <summary>
        /// getter for type
        /// </summary>
        /// <returns> type of the action </returns>
        public string GetActionType()
        {
            return _type;
        }

        public PerformanceObject ToPerformanceObject()
        {
            return new ActionObject(_id);
        }

        /// <summary>
        /// getter for position
        /// </summary>
        /// <returns> Vector 3 position of the action in artifact space </returns>
        public Vector3 GetPosition()
        {
            return _position;
        }

        public Choice GetChoice()
        {
            return _choice;
        }
    }
}