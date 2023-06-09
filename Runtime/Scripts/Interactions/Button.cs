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

namespace AimXRToolkit.Interactions
{
    using System;
    using UnityEngine;

    public class Button : Interactable
    {
        private MeshCollider _collider;
        private AudioClip _sound; // sound to play when the button is pressed

        /// <summary>
        /// <inheritdoc cref="Interactable.Awake" />
        /// </summary>
        private new void Awake()
        {
            if (!initiated)
            {
                _collider = gameObject.AddComponent<MeshCollider>();
                _collider.convex = true;
                _collider.isTrigger = true;
                initiated = true;
            }
        }
        public static Interactable Parse(Models.Component component, GameObject gameObject)
        {
            var interactable = gameObject.AddComponent<Interactions.Button>();
            interactable.SetTag(component.GetTag());
            return interactable;
        }

        public void SetCollider(MeshCollider collider)
        {
            _collider = collider;
        }

        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "controller")
            {
                base.getArtifactManager().CallFunction(base.GetTag(), "WhenPressed");
            }
        }

        void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.tag == "controller")
            {
                base.getArtifactManager().CallFunction(base.GetTag(), "WhenReleased");
            }
        }
    }
}