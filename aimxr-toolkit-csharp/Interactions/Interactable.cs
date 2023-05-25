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

namespace AimXRToolkit.Interactions;
using MoonSharp.Interpreter;
using UnityEngine;
public abstract class Interactable : MonoBehaviour
{
    private ArtifactManager _artifactManager; // machine to which the interactable is attached
    private string tag; // unique tag of the component
    public void setArtifactManager(ArtifactManager artifactManager)
    {
        _artifactManager = artifactManager;
    }
    public ArtifactManager getArtifactManager()
    {
        return _artifactManager;
    }
    public void SetTag(string tag)
    {
        this.tag = tag;
    }
    public string GetTag()
    {
        return tag;
    }
    abstract public void Action();
    public static Interactable Parse(Models.Component component){
        throw new System.NotImplementedException();
    }
}