// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// Execute sub behaviour tree commands until one succeeds, return fail if all fail
    /// </summary>
    [CommandInfo("BehaviourTree",
                 "Selector",
                 "Execute blocks until one succeeds, return fail if all fail")]
    [AddComponentMenu("")]
    public class Selector : BTComposite
    {
        protected override void OnBlockCompleted(Block compBlock)
        {
            //if success next
            if (targetBlocks[curIndex].BehaviourState == BehaviourState.Failed)
            {
                curIndex++;

                //if next doesn't exist, set success and continue
                if (curIndex < targetBlocks.Count)
                {
                    KickOffBlock(targetBlocks[curIndex]);
                }
                else
                {
                    //all failed
                    ParentBlock.Fail();
                }
            }
            else
            {
                //something worked, return a success
                Continue();
            }
        }
    }
}