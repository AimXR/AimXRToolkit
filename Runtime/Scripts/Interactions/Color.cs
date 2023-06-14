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

using AimXRToolkit.Models;
using UnityEngine;

namespace AimXRToolkit.Interactions;

public class Color : Interactable
{
    public static new Interactable Parse(Models.Component component, GameObject gameObject)
    {
        // var interactable = new GameObject().AddComponent<Color>();
        Interactions.Color c = gameObject.AddComponent<Interactions.Color>();
        c.SetTag(component.GetTag());
        return c;
    }

    public void SetColor(string hexa)
    {
        if (ColorUtility.TryParseHtmlString(hexa, out UnityEngine.Color color))
            GetComponent<Renderer>().material.color = color;
        Debug.Log("Color set to " + hexa);
    }
}