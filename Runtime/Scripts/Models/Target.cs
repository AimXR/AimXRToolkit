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

using System.Collections.Generic;

namespace AimXRToolkit.Models
{

    public class Target
    {
        private int id;
        private string name;
        private int artifact;
        private List<int> components;

        public Target(LitJson.JsonData data)
        {
            id = (int)data["id"];
            name = (string)data["name"];
            artifact = (int)data["artifact"];
            components = new List<int>();
            foreach (LitJson.JsonData component in data["components"])
            {
                components.Add((int)component);
            }
        }

        public int GetId()
        {
            return id;
        }
        public string GetName()
        {
            return name;
        }
        public int GetArtifact()
        {
            return artifact;
        }
        public List<int> GetComponents()
        {
            return components;
        }

    }
}