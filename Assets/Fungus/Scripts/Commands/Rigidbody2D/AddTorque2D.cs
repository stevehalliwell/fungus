﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// Add Torque to a Rigidbody2D
    /// </summary>
    [CommandInfo("Rigidbody2D",
                 "AddTorque2D",
                 "Add Torque to a Rigidbody2D")]
    [AddComponentMenu("")]
    public class AddTorque2D : Command
    {
        [SerializeField]
        protected Rigidbody2DData rb;

        [SerializeField]
        protected ForceMode2D forceMode = ForceMode2D.Force;

        [Tooltip("Amount of torque to be added")]
        [SerializeField]
        protected FloatData force;

        public override void OnEnter()
        {
            rb.Value.AddTorque(force.Value, forceMode);

            Continue();
        }

        public override string GetSummary()
        {
            return forceMode.ToString() + ": " + force.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

    }
}