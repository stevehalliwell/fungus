// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
	[CommandInfo("BT",
		"Invert",
		"Inverts the success or failure of the called block")]
	[AddComponentMenu("")]
	public class InvertBT : Command
	{
		[SerializeField]
		protected Block block;

		public override void OnEnter()
		{
			KickOffBlock(block);
		}

		protected virtual void OnBlockCompleted()
		{
			//if success next
			if (block.State == ExecutionState.Succeeded)
			{
				ParentBlock.Fail();
			}
			else
			{
				Continue();
			}
		}

		private void KickOffBlock(Block target)
		{
			var flowchart = GetFlowchart();

			if (target != null)
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
					flowchart.SelectedBlock = target;
				}

				StartCoroutine(target.Execute(onComplete: onComplete));
			}
			else
			{
				Debug.LogWarning("Attempted to Kick Off a null block, suppressing and continuing");
				Continue();
			}
		}

		public override void GetConnectedBlocks(ref List<Block> connectedBlocks)
		{
			connectedBlocks.Add(block);
			base.GetConnectedBlocks(ref connectedBlocks);
		}

		public override Color GetButtonColor()
		{
			return new Color32(235, 191, 217, 255);
		}
	}
}
