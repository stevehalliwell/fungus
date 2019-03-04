using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// 
    /// </summary>
    [EventHandlerInfo("GameEvent",
                      "Empty",
                      "The block will execute when target game event fires.")]
    [AddComponentMenu("")]
    public class EmptyGameEventHandler : BaseGameEventHandler<Empty, EmptyGameEvent>
    {
        public override void OnEventFired(Empty t)
        {
            ExecuteBlock();
        }
    }
}
