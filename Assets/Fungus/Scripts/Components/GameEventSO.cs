using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    public class GameEventSO<T> : ScriptableObject, IGameEvent<T>
    {
        protected List<IGameEventListener<T>> subs = new List<IGameEventListener<T>>();
        protected List<Action<T>> actions = new List<Action<T>>();

        public void Fire(T t)
        {
            for (int i = subs.Count-1; i >= 0 ; i--)
            {
                subs[i].OnEventFired(t);
            }

            for (int i = actions.Count - 1; i >= 0; i--)
            {
                actions[i].Invoke(t);
            }
        }

        public void Subscribe(IGameEventListener<T> sub)
        {
            subs.Add(sub);
        }

        public void Subscribe(Action<T> action)
        {
            actions.Add(action);
        }

        public void Unsubscribe(IGameEventListener<T> sub)
        {
            subs.Remove(sub);
        }

        public void Unsubscribe(Action<T> action)
        {
            actions.Remove(action);
        }
    }
}