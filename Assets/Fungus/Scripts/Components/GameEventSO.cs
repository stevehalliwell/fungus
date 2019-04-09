using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// A ScriptableObject (SO) that is an event that can be fired notifying all listeners and invoking all actions.
    ///
    /// Being an SO allow the event to be referenced directly in scenes and in prefabs. Also allows new events with 
    /// same data payloads to be made without changing code itself as they are just data.
    /// Events allow the object firing and the listener to have no prior knowledge of eachother. Such as
    /// having c# code Fire an event that is listened to by a Fungus block, a Fungus command firing event without
    /// having to know what the result is. Similarly, it may be a useful mechanism for multi-dispatch, wanting
    /// multiple blocks to Execute when something happens.
    ///
    /// See IntGameEvent as an example of how to create your own subtypes.
    /// </summary>
    /// <typeparam name="T">type of the param passed with when the event fires</typeparam>
    public class GameEventSO<T> : ScriptableObject, IGameEvent<T>
    {
        protected List<IGameEventListener<T>> subs = new List<IGameEventListener<T>>();
        protected List<System.Action<T>> actions = new List<System.Action<T>>();

        public void Fire(T t)
        {
            for (int i = subs.Count - 1; i >= 0; i--)
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

        public void Subscribe(System.Action<T> action)
        {
            actions.Add(action);
        }

        public void Unsubscribe(IGameEventListener<T> sub)
        {
            subs.Remove(sub);
        }

        public void Unsubscribe(System.Action<T> action)
        {
            actions.Remove(action);
        }
    }
}