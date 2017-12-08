// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// Execute sub behaviour tree commands until one fails, return success if all are successful
    /// </summary>
    [CommandInfo("BT",
                 "Sequence",
                 "Execute blocks until one fails, return success if all are successful")]
    [AddComponentMenu("")]
    public class Sequence : BTComposite
    {
        protected override void OnBlockCompleted()
        {
            //if success next
            if (targetBlocks[curIndex].State == ExecutionState.Succeeded)
            {
                curIndex++;

                //if next doesn't exist, set success and continue
                if (curIndex < targetBlocks.Count)
                {
                    KickOffBlock(targetBlocks[curIndex]);
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
    }
}