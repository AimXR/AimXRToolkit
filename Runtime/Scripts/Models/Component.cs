// Copyright (C) 2023 Antonin
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


using System.Collections.Generic;
using UnityEngine;
namespace AimXRToolkit.Models
{

    public class Component
    {
        private readonly int _id;
        private readonly string _script;
        private readonly string _tag;
        private readonly string _type;
        private readonly int _target;

        private readonly Dictionary<string, string> _properties;

        public Component(LitJson.JsonData data)
        {

            _id = (int)data["id"];
            _script = (string)data["script"];
            _tag = (string)data["tag"];
            _type = (string)data["type"];
            _properties = new Dictionary<string, string>();
            _target = (int)data["target"];
            foreach (LitJson.JsonData property in data["properties"])
            {
                _properties.Add((string)property["name"], (string)property["value"]);
            }
        }
        public int GetId()
        {
            return _id;
        }

        /// <summary>
        /// return the lua code to execute when something interact with the component
        /// </summary>
        public string GetScript()
        {
            return _script;
        }
        public string GetTag()
        {
            return _tag;
        }
        public string GetType()
        {
            return _type;
        }
        public Dictionary<string, string> GetProperties()
        {
            return _properties;
        }
    }
}