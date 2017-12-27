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

        [SerializeField]
        protected StringData stateNameToChangeTo;

        [Tooltip("If Name is empty, this index will be used instead.")]
        [SerializeField] protected IntegerData stateIndexToChangeTo = new IntegerData(-1);

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