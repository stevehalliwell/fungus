using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// A ScriptableObject (SO) that is an event that can be fired notifying all listeners and invoking all actions.
    /// 
    /// Being an SO allow the event to be referenced in scene and in prefabs.
    /// Events allow the objecing firing and the listener to have no prior knowledge of eachother. Such as
    /// having c# code Fire an event that is listened to by a Fungus block, a Fungus command firing event without
    /// haveing to know what the result is.
    /// 
    /// See IntGameEvent as an example of how to create your own subtypes.
    /// </summary>
    /// <typeparam name="T">type of the param passed with when the event fires</typeparam>
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