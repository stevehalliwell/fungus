using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Linearly Interpolate from A to B
    /// </summary>
    [CommandInfo("Math",
                 "Lerp",
                 "Linearly Interpolate from A to B")]
    [AddComponentMenu("")]
    public class Lerp : Command
    {
        public enum Mode
        {
            Lerp,
            LerpUnclamped,
            LerpAngle
        }

        //[Tooltip("Min Or Max")]
        [SerializeField]
        protected Mode mode = Mode.Lerp;

        //[Tooltip("LHS Value ")]
        [SerializeField]
        protected FloatData a, b, percentage;

        //[Tooltip("Where the result of the function is stored.")]
        [SerializeField]
        protected FloatData outValue;

        public override void OnEnter()
        {
            switch (mode)
            {
                case Mode.Lerp:
                    outValue.Value = Mathf.Lerp(a.Value, b.Value, percentage.Value);
                    break;
                case Mode.LerpUnclamped:
                    outValue.Value = Mathf.LerpUnclamped(a.Value, b.Value, percentage.Value);
                    break;
                case Mode.LerpAngle:
                    outValue.Value = Mathf.LerpAngle(a.Value, b.Value, percentage.Value);
                    break;
                default:
                    break;
            }

            Continue();
        }

        public override string GetSummary()
        {
            return mode.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

    }
}