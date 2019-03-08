using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Fire the given IntGameEvent with the value set in intData.
    /// </summary>
    [CommandInfo("Event",
                 "Fire (int)",
                 "Fire the targeted IntGameEvent")]
    public class IntGameEventFireCommand : BaseGameEventFireCommand<int, IntGameEvent>
    {
        [SerializeField]
        protected IntegerData intData;

        public override void OnEnterInner()
        {
            gameEvent.Fire(intData.Value);
        }

        public override bool HasReference(Variable variable)
        {
            return intData.integerRef == variable || base.HasReference(variable);
        }
    }
}