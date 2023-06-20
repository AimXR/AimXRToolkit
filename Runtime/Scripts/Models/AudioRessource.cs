using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Models
{
    public class AudioRessource : Ressource
    {
        private readonly AudioClip _audioClip;
        public AudioRessource(int actionId, string ressourceName, AudioClip audioClip) : base(actionId, ressourceName)
        {
            _audioClip = audioClip;
        }

        public override void accept(IRessourceVisitor visitor)
        {
            visitor.visit(this);
        }

        public AudioClip GetAudioClip()
        {
            return _audioClip;
        }

        public override string ToString()
        {
            return "AudioRessource: " + GetAudioClip().name + " length: " + GetAudioClip().length;
        }
    }
}
