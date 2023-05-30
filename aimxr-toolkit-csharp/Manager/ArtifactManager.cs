using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using AimXRToolkit.Interactions;
using AimXRToolkit.Interactions.Proxies;
using AimXRToolkit.Managers;
namespace AimXRToolkit
{
    public class ArtifactManager : MonoBehaviour
    {
        private Models.Artifact _artifact;

        [SerializeField]
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
        public async Task InitLogic()
        {
            Dictionary<string, GameObject> flatedArtifact = Flatten(this.gameObject);
            DataManager dm = DataManager.GetInstance();
            foreach (var target in _artifact.GetTargets())
            {
                var targetObj = await dm.GetTargetAsync(target);
                var components = targetObj.GetComponents();
                var obj = flatedArtifact[targetObj.GetName()];
                foreach (var component in components)
                {
                    var componentObj = await dm.GetComponentAsync(component);
                    var interactable = ParseComponent(componentObj, obj);
                    if (interactable != null)
                    {
                        RegisterInteractable(interactable);
                    }
                }
            }
        }

        private Dictionary<string, GameObject> Flatten(GameObject obj)
        {
            Dictionary<string, GameObject> res = new();
            FlattenRecursive(obj.transform, res);
            return res;
        }

        private void FlattenRecursive(Transform transform, Dictionary<string, GameObject> flattenedObjects)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                flattenedObjects.Add(child.name, child);
                FlattenRecursive(child.transform, flattenedObjects);
            }
        }
        public void SetArtifact(Models.Artifact artifact)
        {
            this._artifact = artifact;
        }
        public bool RegisterComponent(Models.Component component)
        {
            this._script.Globals[component.GetTag()] = component;
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

        private Interactable? ParseComponent(Models.Component component, GameObject obj)
        {
            switch (component.GetType())
            {
                case "button":
                    return Button.Parse(component, obj);
                // case "slide":
                //     return Slide.Parse(component);
                // case "switch":
                //     return Switch.Parse(component);
                case "color":
                    return Interactions.Color.Parse(component, obj);
                default:
                    return null;
            }
        }
    }

}
