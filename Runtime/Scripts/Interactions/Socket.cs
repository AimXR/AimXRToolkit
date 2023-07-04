using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
namespace AimXRToolkit.Interactions
{
    public class Socket : Interactable
    {
        private SocketType _socketType;
        private Vector3 _position;
        private Quaternion _rotation;
        private List<int> _accepted;

        private XRSocketInteractor _socketInteractor;
        // Start is called before the first frame update
        void Start()
        {

        }
        new void Awake()
        {
            if (!base.initiated)
            {
                _accepted = new List<int>();
                base.initiated = true;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static Interactable Parse(Models.Component component, GameObject gameObject)
        {
            // var interactable = new GameObject().AddComponent<Color>();
            var c = gameObject.AddComponent<Socket>();
            c.SetTag(component.GetTag());
            return c;
        }
        public void OnTriggerEnter(Collider other)
        {
          /*  if (other.gameObject.GetComponent<Grabbable>() != null)
            {
                _accepted.Add(other.gameObject.GetInstanceID());
            }*/
        }

        public SocketType getSocketType()
        {
            return _socketType;
        }
        public void SetPosition(Vector3 position)
        {
            _position = Utilities.ConvertCoordinates(position);
        }
        public void SetRotation(Vector3 rotation)
        {
            _rotation = Quaternion.Euler(rotation.x, -rotation.y, -rotation.z);
        }
        public Vector3 GetPosition(){
            return Utilities.ConvertCoordinates(_position);
        }
        public Vector3 GetRotation(){
            return Utilities.ConvertRotation(_rotation.eulerAngles).eulerAngles;
        }

        public enum SocketType
        {
            TARGET,
            ARTIFACT
        }
    }

}
