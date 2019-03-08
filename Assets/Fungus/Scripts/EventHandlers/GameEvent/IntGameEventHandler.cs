﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Excecute block when int game event is fired. Optionally stores the int send via the event.
    /// </summary>
    [EventHandlerInfo("GameEvent",
                      "Int",
                      "The block will execute when target game event fires.")]
    [AddComponentMenu("")]
    public class IntGameEventHandler : BaseGameEventHandler<int, IntGameEvent>
    {
        [Tooltip("Optional var to store the param passed with the Event")]
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
