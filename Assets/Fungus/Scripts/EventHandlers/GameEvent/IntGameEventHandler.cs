using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// 
    /// </summary>
    [EventHandlerInfo("GameEvent",
                      "Int",
                      "The block will execute when target game event fires.")]
    [AddComponentMenu("")]
    public class IntGameEventHandler : BaseGameEventHandler<int, IntGameEvent>
    {
        [SerializeField]
        [VariableProperty(typeof(IntegerVariable))]
        protected IntegerVariable intVar;

        public override void OnEventFired(int t)
        {
            if(intVar != null)
            {
                intVar.Value = t;
            }

            ExecuteBlock();
        }
    }
}
