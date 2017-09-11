using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Calculate the angle between 2 vector3s
    /// </summary>
    [CommandInfo("Vector3",
                 "Angle",
                 "Calculate the angle between 2 vector3s")]
    [AddComponentMenu("")]
    public class Vector3Angle : Vector3BinaryCommandFloatOut
    {
        [Tooltip("If true, returns signed angle, making order of lhs and rhs important.")]
        [SerializeField]
        protected bool signedAngle = false;

        [Tooltip("Only needed for signedAngle. Defines axis on which to base the signedness of the angle.")]
        [SerializeField]
        protected Vector3Data axis = new Vector3Data(Vector3.forward);


        public override void OnEnter()
        {
            if (signedAngle)
            {
                output.Value = Vector3.SignedAngle(lhs, rhs, axis);
            }
            else
            {
                output.Value = Vector3.Angle(lhs, rhs);
            }

            Continue();
        }

        public override string GetInnerSummary()
        {
            return (signedAngle ? "Signed Angle" : "Angle");
        }
    }
}