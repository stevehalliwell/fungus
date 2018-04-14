using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// Calls change state on the target FSM.
    /// </summary>
    [CommandInfo("FSM",
                 "Change State",
                 "Calls change state on the target FSM.")]
    [AddComponentMenu("")]
    public class FSMChangeState : Command
    {
        [SerializeField] protected FSM fsm;

        [SerializeField]
        protected StringData stateNameToChangeTo;

        [Tooltip("If Name is empty, this index will be used instead.")]
        [SerializeField]
        protected IntegerData stateIndexToChangeTo = new IntegerData(-1);

        public override void OnEnter()
        {
            if (!string.IsNullOrEmpty(stateNameToChangeTo.Value))
            {
                fsm.ChangeState(stateNameToChangeTo.Value);
            }
            else
            {
                fsm.ChangeState(stateIndexToChangeTo.Value);
            }

            Continue();
        }

        public override string GetSummary()
        {
            if (fsm == null)
            {
                return "Error: no FSM provided.";
            }
            else if (!string.IsNullOrEmpty(stateNameToChangeTo.Value))
            {
                if (fsm.GetIndexFromStateName(stateNameToChangeTo.Value) == -1)
                {
                    return "Error: Invalid state name";
                }
            }
            else if(stateIndexToChangeTo.Value < 0 || stateIndexToChangeTo.Value >= fsm.States.Count)
            {
                return "Error: Invalid state name or index";
            }

            if (fsm.GetIndexFromStateName(stateNameToChangeTo.Value) == -1)
                return fsm.States[stateIndexToChangeTo.Value].Name;

            return fsm.name + " to " + stateNameToChangeTo.Value;
        }

        // public override Color GetButtonColor()
        // {
        //     return new Color32(235, 191, 217, 255);
        // }

        public override bool HasReference(Variable variable)
        {
            return stateNameToChangeTo.stringRef == variable || stateIndexToChangeTo.integerRef == variable;
        }
    }
}