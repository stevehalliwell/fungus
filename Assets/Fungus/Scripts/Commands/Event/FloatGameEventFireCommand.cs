using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// 
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
    }
}