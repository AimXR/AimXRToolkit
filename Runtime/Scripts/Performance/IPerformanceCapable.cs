using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Performance
{
    public interface IPerformanceCapable
    {
        public PerformanceObject ToPerformanceObject();
    }

}