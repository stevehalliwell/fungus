using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fungus
{
    [System.Serializable]
    public class IntUnityEvent : UnityEvent<int> { }

    [CreateAssetMenu(menuName = "Fungus/IntEvent")]
    public class IntGameEvent : GameEventSO<int>
    {
    }
}