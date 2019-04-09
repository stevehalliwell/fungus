using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Base class for <TYPE>GameEventHandlers, deals with sub, unsub and summaries. Child classes will
    /// execute block when the GameEvent they listen to is fired.
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

        #endregion Public members
    }
}