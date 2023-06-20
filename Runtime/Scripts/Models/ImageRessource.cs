using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Models
{
    public class ImageRessource : Ressource
    {
        private readonly Texture _texture;

        public ImageRessource(int actionId, string ressourceName, Texture texture) : base(actionId, ressourceName)
        {
            _texture = texture;
        }

        public override void accept(IRessourceVisitor visitor)
        {
            visitor.visit(this);
        }

        public Texture GetTexture()
        {
            return _texture;
        }

        public override string ToString()
        {
            return "ImageRessource: " + GetTexture().name;
        }
    }
}
