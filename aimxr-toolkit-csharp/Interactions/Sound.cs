// Copyright (C) 2023 Antonin Rousseau
// 
// aimxr-toolkit-csharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// aimxr-toolkit-csharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with aimxr-toolkit-csharp. If not, see <http://www.gnu.org/licenses/>.

using UnityEngine;
namespace AimXRToolkit.Interactions;

/// <summary>
/// Play sound on the machine
/// </summary>
public class Sound : Interactable
{
    [SerializeField]
    public static AudioClip _audioClip;

    private AudioSource _audioSource;

    private bool loop;

    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.spatialize = true;
        _audioSource.loop = this.loop;
        _audioSource.outputAudioMixerGroup = AimXRToolkit.Managers.AimXRManager.Instance.audioMixerGroup;
        _audioSource.clip = AimXRToolkit.Managers.AimXRManager.Instance.testClip;
    }

    /// <summary>
    /// Parse a component to a Interactable of type Sound
    /// </summary>
    /// <param name="component"></param>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static Interactable Parse(Models.Component component, GameObject gameObject)
    {
        // var interactable = new GameObject().AddComponent<Color>();
        Interactions.Sound c = gameObject.AddComponent<Interactions.Sound>();
        c.SetTag(component.GetTag());
        return c;
    }

    /// <summary>
    /// Play the sound
    /// </summary>
    public void Play()
    {
        _audioSource.Play();
    }

    public void SetLoop(bool loop)
    {
        this.loop = loop;
    }
    public bool GetLoop()
    {
        return loop;
    }
}