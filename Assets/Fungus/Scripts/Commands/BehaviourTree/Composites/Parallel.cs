// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// Execute all blocks simultaneously, fails if any fail, success on all 
    /// </summary>
    [CommandInfo("BT",
                 "Parallel",
                 "Execute blocks until one succeeds, return fail if all fail")]
    [AddComponentMenu("")]
    public class Parallel : BTComposite
    {
        private int blocksCompleted = 0;

        public override void OnEnter()
        {
            if (shuffleCommands)
            {
                ShuffleCommandBlockOrder();
            }

            blocksCompleted = 0;
            for (curIndex = 0; curIndex < targetBlocks.Count; curIndex++)
            {
                KickOffBlock(targetBlocks[curIndex]);
            }
        }

        protected override void OnBlockCompleted(Block compBlock)
        {
            //if success next
            if (compBlock.State == ExecutionState.Failed)
            {
                ParentBlock.Fail();
            }
            else
            {
                blocksCompleted++;
                if (blocksCompleted >= targetBlocks.Count)
                {
                    //something worked, return a success
                    Continue();
                }
            }
        }
    }
}