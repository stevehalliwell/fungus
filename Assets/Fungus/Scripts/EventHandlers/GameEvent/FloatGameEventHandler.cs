using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Excecute block when FloatGameEvent is fired. Optionally stores the float send via the event.
    /// </summary>
    [EventHandlerInfo("GameEvent",
                      "Float",
                      "The block will execute when target game event fires.")]
    [AddComponentMenu("")]
    public class FloatGameEventHandler : BaseGameEventHandler<float, FloatGameEvent>
    {
        [Tooltip("Optional var to store the param passed with the Event")]
        [SerializeField]
        [VariableProperty(typeof(FloatVariable))]
        protected FloatVariable floatVar;

        public override void OnEventFired(float t)
        {
            if (floatVar != null)
            {
                floatVar.Value = t;
            }

            ExecuteBlock();
        }
    }
}