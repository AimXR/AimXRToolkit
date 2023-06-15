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
    public class Translate : Interactable
    {
        private Vector3 _origin;
        private Vector3 _direction;

        /// <summary>
        /// The speed at which the object will move in meters per second
        /// </summary>
        private float _speed;

        private bool _move;

        public static Interactable Parse(Models.Component component, GameObject gameObject)
        {
            // var interactable = new GameObject().AddComponent<Color>();
            var c = gameObject.AddComponent<Translate>();
            c.SetTag(component.GetTag());
            c._origin = gameObject.transform.position;
            c._direction = gameObject.transform.forward;
            c._speed = 0.1f;
            c._direction = Vector3.forward;
            return c;
        }
        /// <summary>
        /// Sets the position of the object based on a distance from the origin in the direction of the Translate object
        /// </summary>
        /// <param name="distance">The distance from the origin to move the object</param>
        public void SetPos(float distance)
        {
            gameObject.transform.position = _origin + (_direction * distance);
        }
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
        public void Move()
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, _origin + (_direction * 0.1f), _speed * Time.deltaTime);
        }
        void Update()
        {
            if (_move)
            {
                Move();
            }
        }
        public void StartMovemment()
        {
            _move = true;
        }
        public void StopDrag()
        {
            _move = false;
        }
    }
}