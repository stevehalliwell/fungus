﻿// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
	[CommandInfo("BehaviourTree",
		"ForceResult",
		"Returns the specified result regardless of the target block's status.")]
	[AddComponentMenu("")]
	public class ForceResult : BTDecorator
	{
        [Tooltip("Force result to success, if false forces failure.")]
		[SerializeField]
		protected bool isForcingSuccess = true;
		
		protected override void OnBlockCompleted(Block compBlock)
		{
			//if success next
			if (isForcingSuccess)
			{
				Continue();
			}
			else
			{
				ParentBlock.Fail();
			}
		}
	}
}
