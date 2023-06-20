using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Models
{
    public interface IRessourceVisitor
    {
        public virtual void visit(AudioRessource ressource)
        {
            Debug.Log("IRessourceVisitor: Default behavior");
        }

        public virtual void visit(ImageRessource ressource)
        {
            Debug.Log("IRessourceVisitor: Default behavior");
        }

        public virtual void visit(VideoRessource ressource)
        {
            Debug.Log("IRessourceVisitor: Default behavior");
        }
    }
}
