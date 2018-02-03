// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    [CommandInfo("BehaviourTree",
        "Invert",
        "Inverts the success or failure of the called block")]
    [AddComponentMenu("")]
    public class InvertBT : BTDecorator
    {
        protected override void OnBlockCompleted(Block compBlock)
        {
            //if success next
            if (block.BehaviourState == BehaviourState.Succeeded)
            {
                ParentBlock.Fail();
            }
            else
            {
                Continue();
            }
        }
    }
}
