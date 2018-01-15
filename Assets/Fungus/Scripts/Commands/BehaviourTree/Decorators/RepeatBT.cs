// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Fungus
{
    [CommandInfo("BT",
        "Repeat",
        "Repeats the sub block until a certain condition is met.")]
    [AddComponentMenu("")]
    public class RepeatBT : BTDecorator
    {
        [Tooltip("Continue to call the target block until it returns this state.")]
        [SerializeField]
        protected BehaviourState repeatUntil = BehaviourState.Succeeded;


        [Tooltip("Seconds to wait before next call, after a return does not meet termination state.")]
        [SerializeField]
		protected float timeBetweenRepeats = 0;

        protected override void OnBlockCompleted(Block compBlock)
        {
            if (block.BehaviourState != repeatUntil)
            {
                //wait 1 frame then repeat
				KickOffBlockDelayed(block, timeBetweenRepeats == 0 ? null : new WaitForSeconds(timeBetweenRepeats));
            }
            else
            {
                Continue();
            }
        }
    }
}
