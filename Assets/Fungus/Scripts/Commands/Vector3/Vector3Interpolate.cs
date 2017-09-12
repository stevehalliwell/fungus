using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Lerp from a start to end point based on percentage.
    /// </summary>
    [CommandInfo("Vector3",
                 "Interpolate",
                 "Lerp or Slerp from a start to end point based on percentage.")]
    [AddComponentMenu("")]
    public class Vector3Interpolate : Command
    {
        public enum Mode
        {
            Lerp,
            LerpUnclamped,
            Slerp,
            SlerpUnclamped
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
            switch (mode)
            {
                case Mode.Lerp:
                    outValue.Value = Vector3.Lerp(a, b, percentage);
                    break;
                case Mode.LerpUnclamped:
                    outValue.Value = Vector3.LerpUnclamped(a, b, percentage);
                    break;
                case Mode.Slerp:
                    outValue.Value = Vector3.Slerp(a, b, percentage);
                    break;
                case Mode.SlerpUnclamped:
                    outValue.Value = Vector3.SlerpUnclamped(a, b, percentage);
                    break;
                default:
                    break;
            }

            Continue();
        }

        public override string GetSummary()
        {
            if (outValue.vector3Ref == null)
            {
                return "Error: no output set";
            }

            return mode.ToString() + " stored in " + outValue.vector3Ref.Key; ;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}