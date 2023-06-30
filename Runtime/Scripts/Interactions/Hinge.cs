using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace AimXRToolkit.Interactions
{
    public class Hinge : Interactable
    {
        private float _angle;

        private HingeJoint _hingeJoint;
        private XRGrabInteractable _XRGrabInteractable;
        private Rigidbody _rigidbody;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        private void Awake()
        {
            _rigidbody = this.gameObject.AddComponent<Rigidbody>();
            _XRGrabInteractable = this.gameObject.AddComponent<XRGrabInteractable>();
            _hingeJoint = this.gameObject.AddComponent<UnityEngine.HingeJoint>();
        }

        public static Interactable Parse(Models.Component component, GameObject gameObject)
        {
            Interactions.Hinge c = gameObject.AddComponent<Interactions.Hinge>();
            c.SetTag(component.GetTag());
            return c;
        }
        public void SetMinAngle(float angle)
        {
            this._hingeJoint.limits = new JointLimits()
            {
                min = angle,
                max = this._hingeJoint.limits.max
            };
        }
        public void SetMaxAngle(float angle)
        {
            this._hingeJoint.limits = new JointLimits()
            {
                min = this._hingeJoint.limits.min,
                max = -angle
            };
        }
        public void SetAngle(float angle)
        {
            this.transform.localEulerAngles = new Vector3(-angle, 0, 0);
        }
        public void UseLimits(bool use)
        {
            this._hingeJoint.useLimits = use;
        }
        public void UseGravity(bool use)
        {
            this._rigidbody.isKinematic = use;
        }

        public void SetTarget(string target)
        {
            var obj = this.getArtifactManager().GetArtifactPart(target);
            if (obj == null)
            {
                return;
            }
            Collider collider = obj.GetComponent<Collider>();
            if (collider == null)
                collider = obj.AddComponent<BoxCollider>();
            this._XRGrabInteractable.colliders.Clear();
            this._XRGrabInteractable.colliders.Add(collider);
        }
    }
}
