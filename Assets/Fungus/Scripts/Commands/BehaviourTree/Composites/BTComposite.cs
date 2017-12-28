// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Fungus
{
    /// <summary>
    /// Base class for all composite BehaviourTree commands in Fungus
    /// </summary>
    [AddComponentMenu("")]
    public abstract class BTComposite : BTCommand
    {
        [Tooltip("Block to start executing")]
        [SerializeField]
        protected List<Block> targetBlocks = new List<Block>();

        [Tooltip("Shuffle will reorder the sub blocks on every execution")]
        [SerializeField]
        protected BooleanData shuffleCommands = new BooleanData(false);

        protected int curIndex = 0;

        public override void OnEnter()
        {
            for (int i = 0; i < targetBlocks.Count; i++)
            {
                targetBlocks[i].BehaviourState = BehaviourState.Idle;
            }

            if (shuffleCommands)
            {
                ShuffleCommandBlockOrder();
            }

            curIndex = 0;
            KickOffBlock(targetBlocks[curIndex]);
        }

        protected void ShuffleCommandBlockOrder()
        {
            for (int i = 0; i < targetBlocks.Count; i++)
            {
                var rndIndex = Random.Range(0, targetBlocks.Count);
                var tmp = targetBlocks[rndIndex];
                targetBlocks[rndIndex] = targetBlocks[i];
                targetBlocks[i] = tmp;
            }
        }

        public override void GetConnectedBlocks(ref List<Block> connectedBlocks)
        {
            if (targetBlocks != null)
            {
                connectedBlocks.AddRange(targetBlocks);
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

        public override bool IsReorderableArray(string propertyName)
        {
            return propertyName == "targetBlocks";
        }
    }
}