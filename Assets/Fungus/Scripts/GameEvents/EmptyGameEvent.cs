using UnityEngine;
using UnityEngine.Events;

namespace Fungus
{
    /// <summary>
    /// An object to indicate the lack of information. A workaround for c# lack of void as param for generic type
    /// </summary>
    public struct Empty { }

    /// <summary>
    /// Single param UnityEvent that passes through an Empty, so we can have them match the base generic classes that want
    /// a UnityEvent<T>
    /// </summary>
    [System.Serializable]
    public class EmptyUnityEvent : UnityEvent<Empty> { }

    /// <summary>
    /// A GameEventSO that passes an Empty or no information as param when it is fired.
    /// </summary>
    [CreateAssetMenu(menuName = "Fungus/EmptyEvent")]
    public class EmptyGameEvent : GameEventSO<Empty>
    {
        private static readonly Empty empty = new Empty();

        public void Fire()
        {
            Fire(empty);
        }
    }
}