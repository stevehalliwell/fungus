// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// Base for all BehaviourTree decorator style nodes in fungus. 
    /// </summary>
    [AddComponentMenu("")]
    public abstract class BTDecorator : BTCommand
    {
        [Tooltip("Block to call.")]
        [SerializeField]
        protected Block block;

        public override void OnEnter()
        {
            block.BehaviourState = BehaviourState.Idle;
            KickOffBlock(block);
        }

        public override void GetConnectedBlocks(ref List<Block> connectedBlocks)
        {
            connectedBlocks.Add(block);
            base.GetConnectedBlocks(ref connectedBlocks);
        }
    }
}