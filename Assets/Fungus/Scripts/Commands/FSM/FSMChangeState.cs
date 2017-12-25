using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    [CommandInfo("FSM",
                 "Change State",
                 "Calls change state on the target FSM.")]
    [AddComponentMenu("")]
    public class FSMChangeState : Command
    {
        [SerializeField] protected FSM fsm;

        [VariableProperty(typeof(IntegerVariable), typeof(StringVariable))]
        [SerializeField]
        protected Variable stateToChangeTo;

        public override void OnEnter()
        {
            var asInt = stateToChangeTo as IntegerVariable;
            var asString = stateToChangeTo as StringVariable;

            if (asInt != null)
            {
                fsm.ChangeState(asInt.Value);
            }
            else if (asString != null)
            {
                fsm.ChangeState(asString.Value);
            }
            else
            {
                Debug.LogWarning("Calling FSMChangeState with no valid output variable");
            }
        }

        // public override Color GetButtonColor()
        // {
        //     return new Color32(235, 191, 217, 255);
        // }

        public override bool HasReference(Variable variable)
        {
            return stateToChangeTo == variable;
        }
    }
}