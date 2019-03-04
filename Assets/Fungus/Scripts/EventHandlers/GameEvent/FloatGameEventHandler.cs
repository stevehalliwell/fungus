using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// 
    /// </summary>
    [EventHandlerInfo("GameEvent",
                      "Float",
                      "The block will execute when target game event fires.")]
    [AddComponentMenu("")]
    public class FloatGameEventHandler : BaseGameEventHandler<float, FloatGameEvent>
    {
        [SerializeField]
        [VariableProperty(typeof(FloatVariable))]
        protected FloatVariable floatVar;

        public override void OnEventFired(float t)
        {
            if(floatVar != null)
            {
                floatVar.Value = t;
            }

            ExecuteBlock();
        }
    }
}
