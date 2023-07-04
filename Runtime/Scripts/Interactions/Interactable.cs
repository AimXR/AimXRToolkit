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

using MoonSharp.Interpreter;
using UnityEngine;
using AimXRToolkit.Managers;

namespace AimXRToolkit.Interactions
{
    public abstract class Interactable : MonoBehaviour
    {
        private ArtifactManager _artifactManager; // machine to which the interactable is attached
        private string _tag; // unique tag of the component

        protected bool initiated;

        /// <summary>
        /// We need to add all components to the gameobject before his configuration in the Parse Method <br/>
        /// We can't use Start because it will be call the next frame, so we use Awake but we need to make that the code
        /// execute only one time
        /// </summary>
        protected virtual void Awake()
        {
            
        }
        public void setArtifactManager(ArtifactManager artifactManager)
        {
            _artifactManager = artifactManager;
        }
        public ArtifactManager getArtifactManager()
        {
            return _artifactManager;
        }
        public void SetTag(string componentTag)
        {
            this._tag = componentTag;
        }
        public string GetTag()
        {
            return _tag;
        }
    }
}