using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
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
        }

        // public override Color GetButtonColor()
        // {
        //     return new Color32(235, 191, 217, 255);
        // }
    }
}