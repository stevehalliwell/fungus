// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
    /// <summary>
    /// Base class for all BehaviourTree related commands in Fungus, provides functionality related to running
    /// other blocks and responding based on status
    /// </summary>
    [AddComponentMenu("")]
    public abstract class BTCommand : Command
    {
        protected abstract void OnBlockCompleted(Block target);

        protected void KickOffBlock(Block compBlock)
        {
            var flowchart = GetFlowchart();

            if (compBlock != null)
            {
                // Callback action for Wait Until Finished mode
                System.Action onComplete = delegate
                {
                    flowchart.SelectedBlock = ParentBlock;
                    OnBlockCompleted(compBlock);
                };


                // If the executing block is currently selected then follow the execution 
                // onto the next block in the inspector.
                if (flowchart.SelectedBlock == ParentBlock)
                {
                    flowchart.SelectedBlock = compBlock;
                }

                compBlock.StartExecution(onComplete: onComplete);
            }
            else
            {
                Debug.LogWarning("Attempted to Kick Off a null block, suppressing and continuing");
                Continue();
            }
        }

		protected void KickOffBlockDelayed(Block target, YieldInstruction waitOn)
        {
            StartCoroutine(_KickOffBlockDelayed(target, waitOn));
        }

		private IEnumerator _KickOffBlockDelayed(Block target, YieldInstruction waitOn)
        {
            yield return waitOn;
            KickOffBlock(target);
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}
