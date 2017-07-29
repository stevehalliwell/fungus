﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// The block will execute when the desired OnAnimator* message for the monobehaviour is received.
    /// </summary>
    [EventHandlerInfo("MonoBehaviour",
                      "Animator",
                      "The block will execute when the desired OnAnimator* message for the monobehaviour is received.")]
    [AddComponentMenu("")]
    public class AnimatorState : EventHandler
    {

        [System.Flags]
        public enum AnimatorMessageFlags
        {
            OnAnimatorIK = 1 << 0,
            OnAnimatorMove = 1 << 1,
        }

        [Tooltip("Which of the OnAnimator messages to trigger on.")]
        [SerializeField]
        [EnumFlag]
        protected AnimatorMessageFlags FireOn = AnimatorMessageFlags.OnAnimatorMove;

        [Tooltip("IK layer to trigger on")]
        [SerializeField]
        protected int IKLayer = 0;
        
        private void OnAnimatorIK(int layer)
        {
            if ((FireOn & AnimatorMessageFlags.OnAnimatorIK) != 0 && IKLayer == layer)
            {
                ExecuteBlock();
            }
        }

        private void OnAnimatorMove()
        {
            if ((FireOn & AnimatorMessageFlags.OnAnimatorMove) != 0)
            {
                ExecuteBlock();
            }
        }
    }
}
