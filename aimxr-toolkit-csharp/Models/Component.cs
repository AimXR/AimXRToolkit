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



namespace AimXRToolkit.Models;
public abstract class Component
{
    private string _name;
    private string _script;
    private string _tag;
    private string _type;
    private int _target;

    private Dictionary<string, string> _properties;

    protected Component(string name, string script, string tag, string type, Dictionary<string, string> properties)
    {
        _name = name;
        _script = script;
        _tag = tag;
        _type = type;
        _properties = properties;
    }
    public string GetName()
    {
        return _name;
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
    public abstract void Action();
}