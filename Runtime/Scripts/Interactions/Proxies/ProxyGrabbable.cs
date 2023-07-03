using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Interactions.Proxies
{
    [MoonSharpUserData]
    public class ProxyGrabbable
    {
        private Grabbable _grabbable;

        [MoonSharpHidden]
        public ProxyGrabbable(Grabbable grabbable)
        {
            this._grabbable = grabbable;
        }

        public void UseGravity(bool use)
        {
            this._grabbable.UseGravity(use);
        }
    }
}
