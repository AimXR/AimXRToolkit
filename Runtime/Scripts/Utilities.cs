using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimXRToolkit
{
    public class Utilities
    {
        /// <summary>
        /// Parse a nullable integer from a JsonData object
        /// </summary>
        /// <param name="data">JsonData</param>
        /// <param name="key">name of the field in json</param>
        /// <returns>the id of the action or 0 if null</returns>
        public static int NullableInteger(JsonData data, string key)
        {
            if (data[key] == null)
                return 0;
            return (int)data[key];
        }
    }
}
