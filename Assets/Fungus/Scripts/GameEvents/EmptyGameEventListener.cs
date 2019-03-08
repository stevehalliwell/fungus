using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    ///  Listen to Fungus.EmptyGameEvent, routes param through a empty UnityEvent configured in the inspector.
    /// </summary>
    public class EmptyGameEventListener : GameEventListener<Empty, EmptyGameEvent, EmptyUnityEvent>
    {
    }
}