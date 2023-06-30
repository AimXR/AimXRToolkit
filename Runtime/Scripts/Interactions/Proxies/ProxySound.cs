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

using MoonSharp.Interpreter;
using UnityEngine;

namespace AimXRToolkit.Interactions.Proxies
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class ProxySound
    {
        private Sound _sound;
        public ProxySound(Sound sound)
        {
            _sound = sound;
        }

        public void Play()
        {
            _sound.GetAudioSource().Play();
        }
        public void Stop()
        {
            _sound.GetAudioSource().Stop();
        }
        public void SetLoop(bool loop)
        {
            _sound.GetAudioSource().loop = loop;
        }
        public bool GetLoop()
        {
            return _sound.GetAudioSource().loop;
        }
        public void SetVolume(float volume)
        {
            _sound.GetAudioSource().volume = volume;
        }
        public void SetPlayOnStart(bool playOnStart)
        {
            _sound.GetAudioSource().playOnAwake = playOnStart;
        }

        public void setSoundId(int soundId)
        {
            _sound.SetSoundId(soundId);
        }
    }
}
