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
        
        /// <summary>
        /// Parse a nullable string from a JsonData object
        /// </summary>
        /// <param name="data">JsonData</param>
        /// <param name="key">name of the field in json</param>
        /// <returns>the string or null if null</returns>
        public static string NullableString(JsonData data,string key){
            if(data[key] == null)
                return null;
            return (string)data[key];
        }
        /// <summary>
        /// Convert a Vector3 position from API to Unity
        /// </summary>
        /// <param name="position">Vector3 position from API</param>
        /// <returns>Vector3 position for Unity</returns>
        public static Vector3 ConvertCoordinates(Vector3 position)
        {
            return new Vector3(-position.x, position.z, position.y);
        }
        /// <summary>
        /// Convert a Vector3 rotation from API to Unity
        /// </summary>
        /// <param name="rotation">Vector3 rotation from API</param>
        /// <returns>Quaternion rotation for Unity</returns>
        public static Quaternion ConvertRotation(Vector3 rotation)
        {
            Quaternion angle = Quaternion.Euler(rotation * Mathf.Rad2Deg);
            return new Quaternion(
                 angle.x,
                -angle.y,
                -angle.z,
                 angle.w
            );
        }
    }
}
