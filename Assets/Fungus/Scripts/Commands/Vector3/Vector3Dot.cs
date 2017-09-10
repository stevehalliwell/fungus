using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Calculate the dot product of 2 vector3s.
    /// </summary>
    [CommandInfo("Vector3",
                 "Dot",
                 "Calculate the cross product of 2 vector3s.")]
    [AddComponentMenu("")]
    public class Vector3Dot : Command
    {
        [SerializeField]
        protected Vector3Data lhs, rhs;

        [SerializeField]
        protected FloatData output;
        
        [Tooltip("If true, lhs and rhs will be copied and normalised before dot product is calculated")]
        [SerializeField]
        protected bool normaliseInputs = false;

        public override void OnEnter()
        {
            if(normaliseInputs)
            {
                output.Value = Vector3.Dot(lhs.Value.normalized, rhs.Value.normalized);
            }
            else
            {
                output.Value = Vector3.Dot(lhs, rhs);
            }

            Continue();
        }

        public override string GetSummary()
        {
            if (output.floatRef == null)
            {
                return "Error: no output set";
            }

            return "Dot stored in " + output.floatRef.Key;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}