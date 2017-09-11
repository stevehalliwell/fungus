using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Calculate the distance between 2 positions.
    /// </summary>
    [CommandInfo("Vector3",
                 "Angle",
                 "Calculate the distance between 2 positions.")]
    [AddComponentMenu("")]
    public class Vector3Distance : Vector3BinaryCommandFloatOut
    {
        public override void OnEnter()
        {
            output.Value = Vector3.Distance(lhs, rhs);

            Continue();
        }

        public override string GetInnerSummary()
        {
            return "Distance";
        }
    }
}