using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fungus
{
    public struct Empty { }

    [System.Serializable]
    public class EmptyUnityEvent : UnityEvent<Empty> { }

    [CreateAssetMenu(menuName = "Fungus/EmptyEvent")]
    public class EmptyGameEvent : GameEventSO<Empty>
    {
        static readonly Empty empty = new Empty();

        public void Fire()
        {
            Fire(empty);
        }
    }
}