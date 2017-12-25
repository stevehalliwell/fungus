using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    [CommandInfo("FSM",
                 "Current State",
                 "Stores if the target FSM is currently transitioning between states.")]
    [AddComponentMenu("")]
    public class FSMState : Command
    {
        [SerializeField] protected FSM fsm;

        [VariableProperty(typeof(IntegerVariable), typeof(StringVariable))]
        [SerializeField]
        protected Variable output;

        public override void OnEnter()
        {
            var asInt = output as IntegerVariable;
            var asString = output as StringVariable;

            if (asInt != null)
            {
                asInt.Value = fsm.CurrentState;
            }
            else if (asString != null)
            {
                asString.Value = fsm.CurrentStateName;
            }
            else
            {
                Debug.LogWarning("Calling FSMState with no valid output variable");
            }
        }

        // public override Color GetButtonColor()
        // {
        //     return new Color32(235, 191, 217, 255);
        // }

        public override bool HasReference(Variable variable)
        {
            return output == variable;
        }
    }
}