
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System;

namespace Fungus
{
    /// <summary>
    /// Rotates a game object to the specified angles over time.
    /// </summary>
    [CommandInfo("LeanTween",
                 "Rotate To",
                 "Rotates a game object to the specified angles over time.")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class RotateToLean : BaseLeanTweenCommand
    {
        [Tooltip("Target transform that the GameObject will rotate to")]
        [SerializeField]
        protected TransformData _toTransform;

        [Tooltip("Target rotation that the GameObject will rotate to, if no To Transform is set")]
        [SerializeField]
        protected Vector3Data _toRotation;

        [Tooltip("Whether to animate in world space or relative to the parent. False by default.")]
        [SerializeField]
        protected bool isLocal;

        public override LTDescr ExecuteTween()
        {
            var rot = _toTransform.Value == null ? _toRotation.Value : _toTransform.Value.rotation.eulerAngles;

            if (isLocal)
                return LeanTween.rotateLocal(_targetObject.Value, rot, _duration);
            else
                return LeanTween.rotate(_targetObject.Value, rot, _duration);
        }
    }
}