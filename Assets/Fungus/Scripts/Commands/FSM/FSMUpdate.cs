using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// Calls the tick/update on the target FSM.
    /// </summary>
    [CommandInfo("FSM",
                 "Tick",
                 "Calls the tick/update on the target FSM.")]
    [AddComponentMenu("")]
    public class FSMUpdate : Command
    {
        [SerializeField] protected FSM fsm;

        public override void OnEnter()
        {
            fsm.Tick();

            Continue();
        }

        // public override Color GetButtonColor()
        // {
        //     return new Color32(235, 191, 217, 255);
        // }

        public override string GetSummary()
        {
            if (fsm == null)
            {
                return "Error: no FSM provided.";
            }

            return fsm.Name;
        }
    }
}