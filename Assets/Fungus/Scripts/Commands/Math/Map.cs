using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Map a value that exists in 1 range of numbers to another.
    /// </summary>
    [CommandInfo("Math",
                 "Map",
                 "Map a value that exists in 1 range of numbers to another.")]
    [AddComponentMenu("")]
    public class Map : Command
    {
        //[Tooltip("LHS Value ")]
        [SerializeField]
        protected FloatData initialRangeLower, initialRangeUpper, value;
        
        [SerializeField]
        protected FloatData newRangeLower, newRangeUpper;
        
        [SerializeField]
        protected FloatData outValue;

        public override void OnEnter()
        {
            var p = value.Value - initialRangeLower.Value;
            p /= initialRangeUpper.Value - initialRangeLower.Value;

            var res = p * (newRangeUpper.Value - newRangeLower.Value);
            res += newRangeLower.Value;

            outValue.Value = res;

            Continue();
        }

        public override string GetSummary()
        {
            return "Remap a number to a new range.";
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}