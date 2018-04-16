using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// Stores if the target FSM is currently transitioning between states.
    /// </summary>
    [CommandInfo("FSM",
                 "Is Transitioning",
                 "Stores if the target FSM is currently transitioning between states.")]
    [AddComponentMenu("")]
    public class FSMTransitioning : Command
    {
        [SerializeField] protected FSM fsm;

        [VariableProperty(typeof(BooleanVariable))]
        [SerializeField] protected BooleanVariable output;

        public override void OnEnter()
        {
            output.Value = fsm.IsTransitioningState;

            Continue();
        }

        public override string GetSummary()
        {
            if (fsm == null)
            {
                return "Error: no FSM provided.";
            }
            else if (output == null)
            {
                return "Error: no output set.";
            }

            return fsm.gameObject.name + " in " + output.Key;
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