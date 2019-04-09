using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Fire the given FloatGameEvent with the value set in floatData.
    /// </summary>
    [CommandInfo("Event",
                 "Fire (float)",
                 "Fire the targeted FloatGameEvent")]
    public class FloatGameEventFireCommand : BaseGameEventFireCommand<float, FloatGameEvent>
    {
        [SerializeField]
        protected FloatData floatData;

        public override void OnEnterInner()
        {
            gameEvent.Fire(floatData.Value);
        }

        public override bool HasReference(Variable variable)
        {
            return floatData.floatRef == variable || base.HasReference(variable);
        }
    }
}