using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Excecute block when empy game event is fired.
    /// </summary>
    [EventHandlerInfo("GameEvent",
                      "EmptyEvent",
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