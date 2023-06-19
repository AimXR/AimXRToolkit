using System;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using AimXRToolkit.Interactions;
using AimXRToolkit.Interactions.Proxies;
using AimXRToolkit.Managers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AimXRToolkit.Managers
{
    public class ArtifactManager : MonoBehaviour
    {
        private Models.Artifact _artifact;

        [SerializeField]
        private List<Interactable> _interactables;
        private Script _script;
        private Dictionary<string, GameObject> flatedArtifact;
        void Start()
        {
            _interactables = new List<Interactable>();
            this._script = new Script();
            this._script.Options.DebugPrint = s => Debug.Log(s);
            // UserData.RegisterProxyType<ProxyInteractable, Interactable>(r => new ProxyInteractable(r));
            // UserData.RegisterProxyType<ProxySlide, Slide>(r => new ProxySlide(r));
            // UserData.RegisterProxyType<ProxyButton, SwitchCollider>(r => new ProxySwitch(r));
            UserData.RegisterProxyType<ProxyButton, Button>(r => new ProxyButton(r));
            UserData.RegisterProxyType<ProxyColor, Interactions.Color>(r => new ProxyColor(r));
            UserData.RegisterProxyType<ProxySound, Sound>(r => new ProxySound(r));
            UserData.RegisterProxyType<ProxySwitch, Switch>(r => new ProxySwitch(r));
            flatedArtifact = Flatten(this.gameObject);
        }

        void Update()
        {

        }
        public async Task InitLogic()
        {
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


                        this._script.DoString("_G['" + componentObj.GetTag() + "'] = {}");
                        this._script.DoString("_G['" + componentObj.GetTag() + "'].events = {}");
                        this._script.DoString("_G['" + componentObj.GetTag() + "'].interactable = {}");
                        // DynValue table = (DynValue)this._script.Globals[componentObj.GetTag()];
                        Table table = (Table)this._script.Globals[componentObj.GetTag()];
                        table["interactable"] = UserData.Create(interactable);

                        string pattern = @"function\s+(\w+)\s*\(\)\s*(.*?)\s*end";
                        MatchCollection matches = Regex.Matches(componentObj.GetScript(), pattern, RegexOptions.Singleline);

                        foreach (Match match in matches)
                        {
                            string name = match.Groups[1].Value;
                            string body = match.Groups[2].Value;

                            /**
                            See https://groups.google.com/g/moonsharp/c/LjAiI5FpKHg if you want to use ['obj'].foo instead of ['obj']['foo']

                            can't add properties to a proxy object so wee use a table accessible by the tag of the component that have two fields:
                            - interactable: the proxy object
                            - events: a table that contains the functions of the component
                            **/
                            const string code = @"
                            _G['{0}'].events.{1} = function()
                                {2}
                            end
                            ";
                            string formatted = string.Format(code, componentObj.GetTag(), name, body);
                            this._script.DoString(formatted);
                        }
                    }
                }
            }

            foreach (var key in flatedArtifact.Keys)
            {
                if (flatedArtifact[key] == null)
                {
                    Debug.Log("Key: " + key + " is null");
                    continue;
                }
                var obj = flatedArtifact[key];
                if (obj.GetComponent<Collider>() == null)
                {
                    obj.AddComponent<MeshCollider>();
                }
            }
        }

        private Dictionary<string, GameObject> Flatten(GameObject obj)
        {
            Dictionary<string, GameObject> res = new Dictionary<string, GameObject>();
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
        public void CallFunction(string componentTag, string function)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            /**
            Unity is not thread safe so lot of function can't be call outside of the main thread
            if lua execution is not enough fast, consider implementing a queue system like
            https://github.com/BeardedManStudios/ForgeNetworkingRemastered/blob/master/Forge%20Networking%20Remastered%20Unity/Assets/Bearded%20Man%20Studios%20Inc/Scripts/MainThreadManager.cs
            to call lua async in another thread and then call the unity function in the main thread
            **/
            try
            {
                this._script.DoString("_G['" + componentTag + "'].events." + function + "()");
            }
            catch (Exception e)
            {
                Debug.Log("Failed to call function : " + function + "  :  " + e.Message);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.Log("elapsedMs " + elapsedMs);
        }

        private Interactable ParseComponent(Models.Component component, GameObject obj)
        {
            switch (component.GetComponentType())
            {
                case "Button":
                    return Button.Parse(component, obj);
                // case "slide":
                //     return Slide.Parse(component);
                // case "switch":
                //     return Switch.Parse(component);
                case "Color":
                    return Interactions.Color.Parse(component, obj);
                case "Sound":
                    return Sound.Parse(component, obj);
                case "Switch":
                    return Switch.Parse(component, obj);
                default:
                    return null;
            }
        }

        public Models.Artifact GetArtifact()
        {
            return this._artifact;
        }
        public GameObject GetArtifactPart(string name)
        {
            return flatedArtifact[name];
        }
    }

}
