using UnityEngine;
using UnityEngine.Events;

namespace Fungus
{
    [System.Serializable]
    public class FloatUnityEvent : UnityEvent<float> { }

    /// <summary>
    /// A GameEventSO that passes a float param when it is fired.
    /// </summary>
    [CreateAssetMenu(menuName = "Fungus/FloatEvent")]
    public class FloatGameEvent : GameEventSO<float>
    {
    }
}