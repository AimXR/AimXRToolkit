using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Models;

public class Workplace : IPaginable
{
    private int id;
    private string name;
    private string description;
    private string language;
    private List<ArtifactInstance> artifacts;
    private Anchor _anchor;
    public Workplace(JsonData data)
    {
        this.id = (int)data["id"];
        this.name = (string)data["name"];
        this.description = (string)data["description"];
        this.language = (string)data["language"];
        this.artifacts = new List<ArtifactInstance>();
        foreach (JsonData artifact in data["artifacts"])
        {
            this.artifacts.Add(new ArtifactInstance
            {
                id = (int)artifact["id"],
                artifactId = (int)artifact["artifactID"],
                position = new Vector3((float)(double)artifact["position"]["x"], (float)(double)artifact["position"]["y"], (float)(double)artifact["position"]["z"]),
                rotation = new Vector3((float)(double)artifact["rotation"]["x"], (float)(double)artifact["rotation"]["y"], (float)(double)artifact["rotation"]["z"])
            });
        }
        this._anchor = new Anchor
        {
            position = new Vector3((float)(double)data["anchor"]["position"]["x"], (float)(double)data["anchor"]["position"]["y"], (float)(double)data["anchor"]["position"]["z"]),
            rotation = new Vector3((float)(double)data["anchor"]["rotation"]["x"], (float)(double)data["anchor"]["rotation"]["y"], (float)(double)data["anchor"]["rotation"]["z"])
        };
    }

    public int GetId() { return id; }
    public string GetName() { return name; }
    public string GetDescription() { return description; }
    public string GetLanguage() { return language; }
    /// <summary>
    /// Gets the list of artifacts in the workplace.
    /// </summary>
    /// <returns>The list of artifacts in the workplace.</returns>
    public List<ArtifactInstance> GetArtifacts() { return artifacts; }

    /// <summary>
    /// Gets the anchor of the workplace.
    /// </summary>
    /// <returns>The anchor of the workplace.</returns>
    public Anchor GetAnchor() { return _anchor; }
}


public struct ArtifactInstance
{
    public int id;
    public int artifactId;
    public Vector3 position;
    public Vector3 rotation;
}