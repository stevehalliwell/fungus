using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fungus
{
    [System.Serializable]
    public class IntUnityEvent : UnityEvent<int> { }

    /// <summary>
    /// A GameEventSO that passes an int param when it is fired.
    /// </summary>
    [CreateAssetMenu(menuName = "Fungus/IntEvent")]
    public class IntGameEvent : GameEventSO<int>
    {
    }
}