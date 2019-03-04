using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// 
    /// </summary>
    [CommandInfo("Event",
                 "Fire (int)",
                 "Fire the targeted IntGameEvent")]
    public class IntGameEventFireCommand : BaseGameEventFireCommand<int, IntGameEvent>
    {
        [SerializeField]
        protected IntegerData intData;

        public override void OnEnterInner()
        {
            gameEvent.Fire(intData.Value);
        }
    }
}