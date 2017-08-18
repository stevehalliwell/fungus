using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// Setthe x,y,z fields of a vector3 from floatvars
    /// </summary>
    [CommandInfo("Vector3",
                 "SetFields",
                 "Set the x,y,z from float variables")]
    [AddComponentMenu("")]
    public class Vector3SetFields : Command
    {
        [Tooltip("Optional FloatVariable for saving component in Vector3 to.")]
        [SerializeField]
        protected FloatData xval, yval, zval;

        //[Tooltip("Start of the debug line")]
        [SerializeField]
        protected Vector3Data vec3;

        public override void OnEnter()
        {
            var v = new Vector3(xval.Value, yval.Value, zval.Value);

            vec3.Value = v;

            Continue();
        }

        public override string GetSummary()
        {
            return "Set Fields";
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

    }
}