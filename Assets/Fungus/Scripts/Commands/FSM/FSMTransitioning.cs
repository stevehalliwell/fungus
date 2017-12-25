using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    [CommandInfo("FSM",
                 "Is Transitioning",
                 "Stores if the target FSM is currently transitioning between states.")]
    [AddComponentMenu("")]
    public class FSMTransitioning : Command
    {
		[SerializeField] protected FSM fsm;

		[SerializeField] protected BooleanData output;

        public override void OnEnter()
        {
            output.Value = fsm.IsTransitioningState;
        }
        
        // public override Color GetButtonColor()
        // {
        //     return new Color32(235, 191, 217, 255);
        // }

        public override bool HasReference(Variable variable)
        {
            return output.booleanRef == variable;
        }
    }
}