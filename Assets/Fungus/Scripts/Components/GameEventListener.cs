using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fungus
{
    /// <summary>
    /// Base for components that listen to Fungus.GameEvent sub types, routes param through a UnityEvent configured in the inspector of the same parameter type.
    /// 
    /// Primarily exists to allow for inspector compatible sub types to be simply a matter of declaring types in their own file. See IntGameEventListener for
    /// an example.
    /// </summary>
    /// <typeparam name="T">data based by event, eg int for a IntGameEvent</typeparam>
    /// <typeparam name="GE">Specific GameEvent subtype, so we can show elements correctly in the inspector</typeparam>
    /// <typeparam name="UE">Specific UnityEvent subtype, so we can show elements correctly in the inspector</typeparam>
    [AddComponentMenu("")]
    public abstract class GameEventListener<T, GE, UE> : MonoBehaviour, IGameEventListener<T> where GE : GameEventSO<T> where UE : UnityEvent<T>, new()
    {
        public GE subscribeTo;

        public UE OnFire = new UE();

        public virtual void OnEventFired(T t)
        {
            OnFire.Invoke(t);
        }

        protected virtual void OnEnable()
        {
            subscribeTo.Subscribe(this);
        }

        protected virtual void OnDisable()
        {
            subscribeTo.Unsubscribe(this);
        }
    }
}