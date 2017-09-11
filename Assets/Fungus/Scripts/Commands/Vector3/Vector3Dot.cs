using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    public abstract class Vector3BinaryCommandFloatOut : Command
    {
        [SerializeField]
        protected Vector3Data lhs, rhs;

        [SerializeField]
        protected FloatData output;

        public override string GetSummary()
        {
            if (output.floatRef == null)
            {
                return "Error: no output set";
            }

            return GetInnerSummary() +" stored in " + output.floatRef.Key;
        }

        public abstract string GetInnerSummary();

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }

    /// <summary>
    /// Calculate the dot product of 2 vector3s.
    /// </summary>
    [CommandInfo("Vector3",
                 "Dot",
                 "Calculate the dot product of 2 vector3s.")]
    [AddComponentMenu("")]
    public class Vector3Dot : Vector3BinaryCommandFloatOut
    {
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

        public override string GetInnerSummary()
        {
            return "Dot";// stored in " + output.floatRef.Key;
        }
    }
}