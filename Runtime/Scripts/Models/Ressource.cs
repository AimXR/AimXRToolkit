using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Models
{
    public abstract class Ressource
    {
        private int _actionId;
        private string _ressourceName;

        public Ressource(int actionId, string ressourceName)
        {
            _actionId = actionId;
            _ressourceName = ressourceName;
        }

        public int getActionId()
        {
            return _actionId;
        }

        public string getRessourceName()
        {
            return _ressourceName;
        }
        public abstract void accept(IRessourceVisitor visitor);
    }
}
