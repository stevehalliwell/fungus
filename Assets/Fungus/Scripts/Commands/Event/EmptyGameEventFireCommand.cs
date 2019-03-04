using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// 
    /// </summary>
    [CommandInfo("Event",
                 "Fire (Empty)",
                 "Fire the targeted EmptyGameEvent")]
    public class EmptyGameEventFireCommand : BaseGameEventFireCommand<Empty, EmptyGameEvent>
    {
        public override void OnEnterInner()
        {
            gameEvent.Fire();
        }
    }
}