using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace AimXRToolkit.Interactions
{
    public class Grabbable : Interactable
    {
        private XRGrabInteractable _grabInteractable;
        private bool _useGravity;
        private bool _recreateWhenMove;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public static Interactable Parse(Models.Component component, GameObject gameObject)
        {
            Interactions.Grabbable c = gameObject.AddComponent<Interactions.Grabbable>();
            c.SetTag(component.GetTag());
            var collider = gameObject.AddComponent<MeshCollider>();
            c._grabInteractable.colliders.Add(collider);
            return c;
        }

        public void UseGravity(bool use)
        {
            this._useGravity = use;
            this._grabInteractable.forceGravityOnDetach = use;
        }

        public void RecreateWhenMoved(bool recreate)
        {
            this._recreateWhenMove = recreate;
        }

    }
}
