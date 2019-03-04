using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// 
    /// </summary>
    [AddComponentMenu("")]
    public abstract class BaseGameEventHandler<T, GE> : EventHandler, IGameEventListener<T> where GE : GameEventSO<T>
    {
        public GE gameEvent;

        #region Public members

        protected virtual void OnEnable()
        {
            gameEvent.Subscribe(this);
        }

        protected virtual void OnDisable()
        {
            gameEvent.Unsubscribe(this);
        }

        public override string GetSummary()
        {
            if (gameEvent != null)
            {
                return gameEvent.name;
            }

            return "Error: no event set";
        }

        public abstract void OnEventFired(T t);

        #endregion
    }
}
