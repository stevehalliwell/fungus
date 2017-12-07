// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    [CommandInfo("BT",
                 "Fail",
                 "Sets this block's execution state / status to failed and stops the block")]
    [AddComponentMenu("")]
    public class FailBlock : Command
    {

        public override void OnEnter()
        {
            ParentBlock.Fail();
        }
        
        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}