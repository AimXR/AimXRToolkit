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
using Color = UnityEngine.Color;

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

            UserData.RegisterProxyType<ProxyButton, Button>(r => new ProxyButton(r));
            UserData.RegisterProxyType<ProxyColor, Interactions.Color>(r => new ProxyColor(r));
            UserData.RegisterProxyType<ProxySound, Sound>(r => new ProxySound(r));
            UserData.RegisterProxyType<ProxySwitch, Switch>(r => new ProxySwitch(r));
            UserData.RegisterProxyType<ProxyHinge, Hinge>(r => new ProxyHinge(r));
            UserData.RegisterProxyType<ProxySlider, Slider>(r => new ProxySlider(r));
            flatedArtifact = Flatten(this.gameObject);
            // add a box collider with max bound of the artifact children
            var boxCollider = this.gameObject.AddComponent<BoxCollider>();
            var bounds = GetMaxBounds(this.gameObject);
            boxCollider.center = bounds.center;
            boxCollider.size = bounds.size;

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
                        interactable.SetComponentObj(componentObj);
                        this._interactables.Add(interactable);
                        this._script.DoString("_G[\"" + componentObj.GetTag() + "\"] = {}");
                        this._script.DoString("_G[\"" + componentObj.GetTag() + "\"].events = {}");
                        this._script.DoString("_G[\"" + componentObj.GetTag() + "\"].interactable = {}");

                        // DynValue table = (DynValue)this._script.Globals[componentObj.GetTag()];
                        Table table = (Table)this._script.Globals[componentObj.GetTag()];
                        table["interactable"] = UserData.Create(interactable);
                    }
                    else
                    {
                        Debug.LogError("Can't parse : " + componentObj.GetComponentType());
                    }
                }
                foreach (var component in components)
                {
                    var componentObj = await dm.GetComponentAsync(component);
                    try
                    {
                        Debug.Log(componentObj.GetScript());
                        this._script.DoString(componentObj.GetScript());
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        Debug.Log("Script was \n" + componentObj.GetScript());
                        Debug.Log("Component was " + componentObj.GetId()+" "+componentObj.GetTag()+" "+componentObj.GetComponentType());
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
            foreach(var interactable in this._interactables)
            {
                Closure function = null;
                try
                {
                    function = this._script.Globals.Get(interactable.GetComponentObj().GetTag()).Table.Get("events").Table.Get("Start").Function;
                    Debug.Log("component : " + interactable.GetComponentObj().GetTag() + " id :" + interactable.GetComponentObj().GetId() + "\n" + interactable.GetComponentObj().GetScript());
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    continue;
                }
                try
                {
                    if (function != null)
                    {
                        function.Call();
                        Debug.Log("Start OK");
                    }
                    else
                        Debug.Log("No function");
                }
                catch (Exception e)
                {
                    Debug.Log("Fail to call Start function");
                    Debug.Log(e);
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
                case "Slider":
                    return Slider.Parse(component, obj);
                case "Color":
                    return Interactions.Color.Parse(component, obj);
                case "Sound":
                    return Sound.Parse(component, obj);
                case "Switch":
                    return Switch.Parse(component, obj);
                case "Hinge":
                    return Hinge.Parse(component, obj);
                case "Crank":
                    return Hinge.Parse(component, obj);
                case "Grabbable":
                    return Grabbable.Parse(component, obj);
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

        private Bounds GetMaxBounds(GameObject g)
        {
            var renderers = g.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return new Bounds(g.transform.position, Vector3.zero);
            var b = renderers[0].bounds;
            foreach (Renderer r in renderers)
            {
                b.Encapsulate(r.bounds);
            }

            Debug.DrawLine(b.min, b.max, Color.red, 0.16f);
            return b;
        }
    }

}
