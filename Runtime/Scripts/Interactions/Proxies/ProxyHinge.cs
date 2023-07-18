using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Interactions.Proxies
{
    public class ProxyHinge
    {
        private Hinge _hinge;
        public ProxyHinge(Hinge h)
        {
            _hinge = h;
        }
        public void SetMinAngle(float angle)
        {
            this._hinge.SetMinAngle(angle);
        }

        public void SetMaxAngle(float angle)
        {
            this._hinge.SetMaxAngle(angle);
        }
        public void UseLimits(bool use)
        {
            this._hinge.UseLimits(use);
        }
        public void UseGravity(bool use)
        {
            this._hinge.UseGravity(use);
        }
        public void SetTarget(string target)
        {
            this._hinge.SetTarget(target);
        }
        public void SetAngle(float angle)
        {
            _hinge.SetAngle(angle);
        }
    }
}
