using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Interactions.Proxies
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class ProxySlider
    {

        private Slider _slider;
        public ProxySlider(Slider slider)
        {
            _slider = slider;
        }

        public void SetSpeed(float speed)
        {
            _slider.SetSpeed(speed);
        }
        public void GoTo(float distance)
        {
            _slider.GoTo(distance);
        }
        public void SetStop(float maxDisplacement)
        {
            _slider.SetStop(maxDisplacement);
        }
        public float GetStop()
        {
            return _slider.GetStop();
        }

        public void StartMovement(bool reverse)
        {
            _slider.StartMovement(reverse);
        }
        public void StopMovement()
        {
            _slider.StopMovement();
        }
    }
}
