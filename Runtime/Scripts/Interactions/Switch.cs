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

using UnityEngine;

namespace AimXRToolkit.Interactions
{

    /// <summary>
    /// Represents an interactable switch.
    /// </summary>
    public class Switch : Interactable
    {
        private MeshCollider _collider;
        private AudioSource _audioSource;
        private bool _isOn;
        /// <summary>
        /// The angle of the switch.
        /// </summary>
        private float _angle;
        public static Interactable Parse(Models.Component component, GameObject gameObject)
        {
            // var interactable = new GameObject().AddComponent<Color>();
            Interactions.Switch c = gameObject.AddComponent<Interactions.Switch>();
            c.SetTag(component.GetTag());
            c._angle = 90.0f;
            return c;
        }
        public void SetCollider(MeshCollider collider)
        {
            _collider = collider;
        }
        void Start()
        {
            _collider = gameObject.AddComponent<MeshCollider>();
            _collider.convex = true;
            _collider.isTrigger = true;
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.clip = Managers.AimXRManager.Instance.testClip;
        }

        public void SetStop(float angle)
        {
            _angle = angle;
        }

        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "controller")
            {
                _isOn = !_isOn;
                _audioSource.Play();
                ReactToState();
            }

        }
        private void ReactToState()
        {
            // flip the switch by the angle
            if (_isOn)
            {
                this.gameObject.transform.Rotate(new Vector3(-_angle, 0, 0));
                base.getArtifactManager().CallFunction(base.GetTag(), "WhenTurnedOn");
            }
            else
            {
                this.gameObject.transform.Rotate(new Vector3(0, 0, 0));
                base.getArtifactManager().CallFunction(base.GetTag(), "WhenTurnedOff");
            }
        }

        public void SetState(bool state)
        {
            _isOn = state;
            ReactToState();
        }

        public bool GetState()
        {
            return _isOn;
        }
    }
}
