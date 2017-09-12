using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Store the min or max of componentwise of 2 vector3s.
    /// </summary>
    [CommandInfo("Vector3",
                 "MinMax",
                 "Store the min or max of componentwise of 2 vector3s.")]
    [AddComponentMenu("")]
    public class Vector3MinMax : Vector3BinaryCommand
    {
        public enum Function
        {
            Min,
            Max
        }

        [Tooltip("Min Or Max")]
        [SerializeField]
        protected Function function = Function.Min;

        public override void OnEnter()
        {
            switch (function)
            {
                case Function.Min:
                    output.Value = Vector3.Min(lhs.Value, rhs.Value);
                    break;
                case Function.Max:
                    output.Value = Vector3.Max(lhs.Value, rhs.Value);
                    break;
                default:
                    break;
            }
            
            Continue();
        }
        
        public override string GetInnerSummary()
        {
            return function.ToString();
        }
    }
}