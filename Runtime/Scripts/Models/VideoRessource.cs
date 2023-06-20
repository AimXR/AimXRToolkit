using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit.Models
{
    public class VideoRessource : Ressource
    {
        public VideoRessource(int actionId, string ressourceName) : base(actionId, ressourceName)
        {
        }

        public override void accept(IRessourceVisitor visitor)
        {
            visitor.visit(this);
        }

        /// <summary>
        /// Video URL to use in VideoPlayer
        /// </summary>
        /// <returns></returns>
        public string GetVideoUrl()
        {
            return API.API_URL + API.ROUTE.ACTIONS_DATA + getActionId() + "/" + getRessourceName();
        }

        public override string ToString()
        {
            return "VideoRessource: " + getRessourceName();
        }
    }
}
