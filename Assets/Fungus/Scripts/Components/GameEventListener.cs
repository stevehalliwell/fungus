using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fungus
{
    [AddComponentMenu("")]
    public abstract class GameEventListener<T, GE, UE> : MonoBehaviour, IGameEventListener<T> where GE : GameEventSO<T> where UE : UnityEvent<T>, new()
    {
        public GE subscribeTo;

        public UE OnFire = new UE();

        public void OnEventFired(T t)
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