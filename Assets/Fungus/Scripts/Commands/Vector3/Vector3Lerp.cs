using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Lerp from a start to end point based on percentage.
    /// </summary>
    [CommandInfo("Vector3",
                 "Lerp",
                 "Lerp from a start to end point based on percentage.")]
    [AddComponentMenu("")]
    public class Vector3Lerp : Command
    {
        public enum Mode
        {
            Lerp,
            LerpUnclamped
        }

        [SerializeField]
        protected Mode mode = Mode.Lerp;

        [SerializeField]
        protected Vector3Data a, b;

        //[Tooltip("LHS Value ")]
        [SerializeField]
        protected FloatData percentage;

        //[Tooltip("Where the result of the function is stored.")]
        [SerializeField]
        protected Vector3Data outValue;


        public override void OnEnter()
        {
            if (mode == Mode.Lerp)
            {
                outValue.Value = Vector3.Lerp(a, b, percentage);
            }
            else
            {
                outValue.Value = Vector3.LerpUnclamped(a, b, percentage);
            }

            Continue();
        }

        public override string GetSummary()
        {
            if (outValue.vector3Ref == null)
            {
                return "Error: no output set";
            }

            return (mode == Mode.Lerp ? "Lerp" : "Lerp Unclamped") + " stored in " + outValue.vector3Ref.Key; ;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}