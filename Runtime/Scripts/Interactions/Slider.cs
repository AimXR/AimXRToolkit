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
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
namespace AimXRToolkit.Interactions
{
    public class Slider : Interactable
    {
        private Vector3 _origin;
        private Vector3 _direction;

        /// <summary>
        /// The maximum distance the object can move from the origin
        /// </summary>
        private float _maxDisplacement;

        /// <summary>
        /// The speed at which the object will move in meters per second
        /// </summary>
        private float _speed = 0.1f;

        /// <summary>
        /// Whether the slider is operating or not
        /// </summary>
        private bool _move = true;

        private float _wanted = 0f; // use for GoTo smooth movement
        private float lerpDuration;
        private float timeElapsed;

        void Update()
        {
            if (_move)
            {
                Move();
            }
        }
        void Start()
        {
            _origin = gameObject.transform.localPosition;
            _direction = new Vector3(-1f, 0f, 0f);
        }
        public static Interactable Parse(Models.Component component, GameObject gameObject)
        {
            // var interactable = new GameObject().AddComponent<Color>();
            var c = gameObject.AddComponent<Slider>();
            c.SetTag(component.GetTag());
            return c;
        }

        /// <summary>
        /// Sets the position of the object based on a distance from the origin in the direction of the Translate object
        /// </summary>
        /// <param name="distance">The distance from the origin to move the object</param>
        public void SetPos(float distance)
        {
            gameObject.transform.localPosition = _origin + (_direction * distance);
        }

        /// <summary>
        /// Sets the wanted position of the object to be used in the Move method
        /// </summary>
        public void GoTo(float distance)
        {
            _wanted = distance;
            lerpDuration = Vector3.Distance(gameObject.transform.localPosition, _origin + (_direction * _wanted)) / _speed;
            timeElapsed = 0f;
            _move = true;
        }

        /// <summary>
        /// Sets the speed of the slider
        /// </summary>
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        /// <summary>
        /// Moves the object in the direction
        /// </summary>
        private void Move()
        {
            if (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;
                gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, _origin + (_direction * _wanted), timeElapsed / lerpDuration);
            }
            else
            {
                gameObject.transform.localPosition = _origin + (_direction * _wanted);
            }
        }

        /// <summary>
        /// Move toward the stop or origin
        /// </summary>
        public void StartMovement(bool reverse)
        {
            GoTo(reverse ? 0f : _maxDisplacement);
        }

        /// <summary>
        /// Stops the object from moving
        /// </summary>
        public void StopMovement()
        {
            _move = false;
        }

        /// <summary>
        /// Gets the maximum distance the object can move from the origin
        /// </summary>
        public float GetStop()
        {
            return _maxDisplacement;
        }

        /// <summary>
        /// Sets the maximum distance the object can move from the origin
        /// </summary>
        public void SetStop(float displacement)
        {
            _maxDisplacement = displacement;
        }

    }
}