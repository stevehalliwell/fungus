using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// Pull out the x,y,z fields of a vector3 to floatvars
    /// </summary>
    [CommandInfo("Vector3",
                 "GetFields",
                 "Extracts the x,y,z to float variables")]
    [AddComponentMenu("")]
    public class Vector3GetFields : Command
    {
        //[Tooltip("Start of the debug line")]
        [SerializeField]
        protected Vector3Data vec3;

        [Tooltip("Optional FloatVariable for saving component in Vector3 to.")]
        [SerializeField]
        protected FloatData xval, yval, zval;

        public override void OnEnter()
        {
            var v = vec3.Value;

            xval.Value = v.x;
            yval.Value = v.y;
            zval.Value = v.z;

            Continue();
        }

        public override string GetSummary()
        {
            return "Get Fields";
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

    }
}