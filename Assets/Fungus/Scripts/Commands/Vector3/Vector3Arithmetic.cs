using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Command to store the min or max of 2 values
    /// </summary>
    [CommandInfo("Vector3",
                 "Arithmetic",
                 "Vec3 add, sub, mul, div")]
    [AddComponentMenu("")]
    public class Vector3Arithmetic : Command
    {
        [SerializeField]
        protected Vector3Data lhs, rhs, output;

        public enum Operation
        {
            Add,
            Sub,
            Mul,
            Div
        }

        [SerializeField]
        protected Operation operation = Operation.Add;

        public override void OnEnter()
        {
            switch (operation)
            {
                case Operation.Add:
                    output.Value = lhs.Value + rhs.Value;
                    break;
                case Operation.Sub:
                    output.Value = lhs.Value - rhs.Value;
                    break;
                case Operation.Mul:
                    output.Value = lhs.Value;
                    output.Value.Scale(rhs.Value);
                    break;
                case Operation.Div:
                    output.Value = lhs.Value;
                    output.Value.Scale(new Vector3(1.0f / rhs.Value.x,
                        1.0f / rhs.Value.y,
                        1.0f / rhs.Value.z));
                    break;
                default:
                    break;
            }
            Continue();
        }

        public override string GetSummary()
        {
            return operation.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}