using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Models
{
    public interface IRessourceVisitor
    {
        public void visit(AudioRessource ressource);

        public void visit(ImageRessource ressource);

        public void visit(VideoRessource ressource);
    }
}
