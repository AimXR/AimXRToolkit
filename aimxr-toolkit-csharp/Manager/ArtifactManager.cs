using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using AimXRToolkit.Interactions;
using AimXRToolkit.Interactions.Proxies;
using AimXRToolkit.Managers;
using System.Text.RegularExpressions;

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
                        interactable.setArtifactManager(this);
                        this._interactables.Add(interactable);
                        this._script.Globals[componentObj.GetTag()] = interactable;


                        // create a lua function in the script 
                        string pattern = @"function\s+(\w+)\s*\(\)\s*(.*?)\s*end";

                        MatchCollection matches = Regex.Matches(componentObj.GetScript(), pattern, RegexOptions.Singleline);

                        foreach (Match match in matches)
                        {
                            string name = match.Groups[1].Value;
                            string body = match.Groups[2].Value;
                            const string code = @"
                            _G['{0}'] = function()
                                {1}
                            end
                            ";
                            this._script.Globals[name] = this._script.DoString(string.Format(code, name, body));
                        }
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
        public void CallFunction(string tag, string function)
        {
            var component = this._script.Globals.Get(tag);
            Debug.Log(component.ToDebugPrintString());
            Debug.Log(component.Table.ToString());
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
