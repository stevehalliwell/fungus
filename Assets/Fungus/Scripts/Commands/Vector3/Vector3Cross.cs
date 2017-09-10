using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Calculate the cross product of 2 vector3s.
    /// </summary>
    [CommandInfo("Vector3",
                 "Cross",
                 "Calculate the cross product of 2 vector3s.")]
    [AddComponentMenu("")]
    public class Vector3Cross : Command
    {
        [SerializeField]
        protected Vector3Data lhs, rhs, output;

        public override void OnEnter()
        {
            Vector3 tmp;

            tmp = Vector3.Cross(lhs, rhs);

            output.Value = tmp;
            
            Continue();
        }

        public override string GetSummary()
        {
            if (output.vector3Ref == null)
            {
                return "Error: no output set";
            }

            return "Cross stored in " + output.vector3Ref.Key;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}