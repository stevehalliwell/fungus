using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Base command that causes a Fungus.GameEvent subtype to be Fired.
    ///
    /// Primarily exists to allow for inspector compatible sub types to be simply a matter of 
    /// declaring types in their own file. See IntGameEventFireCommand for an example.
    /// </summary>
    /// <typeparam name="T">data based by event, eg int for a IntGameEvent</typeparam>
    /// <typeparam name="GE">Specific GameEvent subtype, so we can show elements correctly in the inspector</typeparam>
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