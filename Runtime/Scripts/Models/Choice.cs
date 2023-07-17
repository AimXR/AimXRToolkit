using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace AimXRToolkit
{
    public class Choice
    {
        private int _leftTargetActionId;
        private int _rightTargetActionId;

        private string _leftButtonText;
        private string _rightButtonText;
        public Choice(JsonData data)
        {
            _leftTargetActionId = (int) data["left"]["target"];
            _rightTargetActionId = (int) data["right"]["target"];
            _leftButtonText = (string) data["left"]["name"];
            _rightButtonText = (string) data["right"]["name"];
        }

        /// <summary>
        /// Getter for the left button text of the choice
        /// </summary>
        /// <returns>Left text of the choice</returns>
        public string GetLeftText()
        {
            return _leftButtonText;
        }

        /// <summary>
        /// Getter for the right button of the choice
        /// </summary>
        /// <returns>Right text of the choice</returns>
        public string GetRightText()
        {
            return _rightButtonText;
        }

        /// <summary>
        /// Getter for the left action of the choice
        /// </summary>
        /// <returns>Id of the action</returns>
        public int GetLeftActionId()
        {
            return _leftTargetActionId;
        }

        /// <summary>
        /// Getter for the right action of the choice
        /// </summary>
        /// <returns>Id of the action</returns>
        public int GetRightActionId()
        {
            return _rightTargetActionId;
        }
        public override string ToString()
        {
            return "(Choice)\n" + _leftButtonText + " : " + _leftTargetActionId + " | " + _rightButtonText + " : " + _rightTargetActionId;
        }
    }
}
