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
using LitJson;
using UnityEngine;
namespace AimXRToolkit.Models;

public class WorkplaceShort : IPaginable
{
    private readonly string _id;
    private readonly string _name;
    private readonly string _description;
    private readonly List<string> languages;
    public WorkplaceShort(JsonData data)
    {
        _id = data["id"].ToString();
        _name = data["name"].ToString();
        _description = data["description"].ToString();
        languages = JsonMapper.ToObject<List<string>>(data["languages"].ToJson());
    }

    public string GetId()
    {
        return _id;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetDescription()
    {
        return _description;
    }

    public List<string> GetLanguages()
    {
        return languages;
    }
    public override string ToString()
    {
        return string.Format("WorkplaceShort: {0} {1} {2} {3}", _id, _name, _description, languages);
    }

}