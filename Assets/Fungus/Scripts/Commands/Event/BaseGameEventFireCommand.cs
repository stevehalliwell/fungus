using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseGameEventFireCommand<T, GE> : Command where GE : GameEventSO<T>
    {
        [SerializeField]
        protected GE gameEvent;

        public override void OnEnter()
        {
            if (gameEvent != null)
                OnEnterInner();

            Continue();
        }

        public override Color GetButtonColor()
        {
            return new Color32(191, 235, 217, 255);
        }

        public override string GetSummary()
        {
            if (gameEvent == null)
                return "Error: no game event set";

            return gameEvent.name;
        }

        public abstract void OnEnterInner();
    }
}