﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Calculate the cross product of 2 vector3s.
    /// </summary>
    [CommandInfo("Vector3",
                 "Cross",
                 "Calculate the cross product of 2 vector3s.")]
    [AddComponentMenu("")]
    public class Vector3Cross : Vector3BinaryCommand
    {
        public override void OnEnter()
        {
            Vector3 tmp;

            tmp = Vector3.Cross(lhs, rhs);

            output.Value = tmp;
            
            Continue();
        }

        public override string GetInnerSummary()
        {
            return "Cross";
        }
    }
}