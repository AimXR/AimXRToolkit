using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using AimXRToolkit.Interactions;
using AimXRToolkit.Interactions.Proxies;

namespace AimXRToolkit
{
    public class ArtifactManager : MonoBehaviour
    {
        private Models.Artifact _artifact;
        private List<Interactable> _interactables;
        private Script _script;
        // Start is called before the first frame update
        void Start()
        {
            _interactables = new List<Interactable>();
            this._script = new Script();
            this._script.Options.DebugPrint = s => Debug.Log(s);
            // UserData.RegisterProxyType<ProxyInteractable, Interactable>(r => new ProxyInteractable(r));
            // UserData.RegisterProxyType<ProxySlide, Slide>(r => new ProxySlide(r));
            // UserData.RegisterProxyType<ProxyButton, SwitchCollider>(r => new ProxySwitch(r));
            UserData.RegisterProxyType<ProxyButton, Button>(r => new ProxyButton(r));

            // call function onTouch in script
        }

        // Update is called once per frame
        void Update()
        {

        }
        public async void InitLogic()
        {

        }
        public void SetArtifact(Models.Artifact artifact)
        {
            this._artifact = artifact;
        }
        public bool RegisterComponent(AimXRToolkit.Models.Component component)
        {
            this._script.Globals[component.GetName()] = component;
            return true;
        }
        private void RegisterInteractable(Interactable interactable)
        {
            interactable.setArtifactManager(this);
            this._interactables.Add(interactable);
        }
        public void CallFunction(string tag, string function)
        {
            this._script.Globals.Get(tag).Table.Get(function).Function.Call();
        }

        private Interactable? ParseComponent(Models.Component component)
        {
            switch (component.GetType())
            {
                case "button":
                    return Button.Parse(component);
                // case "slide":
                //     return Slide.Parse(component);
                // case "switch":
                //     return Switch.Parse(component);
                case "color":
                    return Interactions.Color.Parse(component);
                default:
                    return null;
            }
        }
    }

}
