// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    [CommandInfo("BT",
                 "Sequence",
                 "Execute blocks in order until an error is found")]
    [AddComponentMenu("")]
    public class Sequence : Command
    {
        [Tooltip("Block to start executing")]
        [SerializeField]
        protected List<Block> targetBlock = new List<Block>();
        
        private int curIndex = 0;



        #region Public members

        private void OnBlockCompleted()
        {
            //if success next
            if (targetBlock[curIndex].State == ExecutionState.Succeeded)
            {
                curIndex++;

                //if next doesn't exist, set success and continue
                if (curIndex < targetBlock.Count)
                {
                    KickOffBlock();
                }
                else
                {
                    Continue();
                }
            }
            //if fail set fail state and stop
            else
            {
                ParentBlock.Fail();
            }
        }

        private void KickOffBlock()
        {
            var flowchart = GetFlowchart();

            if (targetBlock != null)
            {
                // Callback action for Wait Until Finished mode
                Action onComplete = delegate
                {
                    flowchart.SelectedBlock = ParentBlock;
                    OnBlockCompleted();
                };
                

                // If the executing block is currently selected then follow the execution 
                // onto the next block in the inspector.
                if (flowchart.SelectedBlock == ParentBlock)
                {
                    flowchart.SelectedBlock = targetBlock[curIndex];
                }

                StartCoroutine(targetBlock[curIndex].Execute(onComplete: onComplete));
            }
        }

        public override void OnEnter()
        {
            curIndex = 0;
            KickOffBlock();
        }

        public override void GetConnectedBlocks(ref List<Block> connectedBlocks)
        {
            if (targetBlock != null)
            {
                connectedBlocks.AddRange(targetBlock);
            }
        }

        public override string GetSummary()
        {
            string summary = "";



            return summary;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        #endregion
    }
}