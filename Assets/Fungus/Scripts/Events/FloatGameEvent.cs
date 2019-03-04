using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fungus
{
    [System.Serializable]
    public class FloatUnityEvent : UnityEvent<float> { }

    [CreateAssetMenu(menuName = "Fungus/FloatEvent")]
    public class FloatGameEvent : GameEventSO<float>
    {
    }
}