using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// Get or Set a Vector3 property of a transform component
    /// </summary>
    [CommandInfo("Transform",
                 "Vector3Property",
                 "Get or Set a Vector3 property of a transform component")]
    [AddComponentMenu("")]
    public class TransformVector3Property : Command
    {
        public enum Poperty
        {
            GetEulerAngles,
            SetEulerAngles,
            GetForward,
            SetForward,
            GetLocalEulerAngles,
            SetLocalEulerAngles,
            GetLocalPosition,
            SetLocalPosition,
            //GetLocalRotation,
            //SetLocalRotation,
            GetLocalScale,
            SetLocalScale,
            GetLossyScale,
            GetPosition,
            SetPosition,
            GetRight,
            SetRight,
            GetUp,
            SetUp,
        }

        [SerializeField]
        protected Poperty property = Poperty.GetPosition;

        [SerializeField]
        protected TransformData transformData;

        [Tooltip("Used as value for either assigning to or from depending on the Get or Set chosen")]
        [SerializeField]
        protected Vector3Data vec3;

        public override void OnEnter()
        {
            switch (property)
            {
                case Poperty.GetEulerAngles:
                    vec3.Value = transformData.Value.eulerAngles;
                    break;
                case Poperty.SetEulerAngles:
                    transformData.Value.eulerAngles = vec3.Value;
                    break;
                case Poperty.GetForward:
                    vec3.Value = transformData.Value.forward;
                    break;
                case Poperty.SetForward:
                    transformData.Value.forward = vec3.Value;
                    break;
                case Poperty.GetLocalEulerAngles:
                    vec3.Value = transformData.Value.localEulerAngles;
                    break;
                case Poperty.SetLocalEulerAngles:
                    transformData.Value.localEulerAngles = vec3.Value;
                    break;
                case Poperty.GetLocalPosition:
                    vec3.Value = transformData.Value.localPosition;
                    break;
                case Poperty.SetLocalPosition:
                    transformData.Value.localPosition = vec3.Value;
                    break;
                case Poperty.GetLocalScale:
                    vec3.Value = transformData.Value.localScale;
                    break;
                case Poperty.SetLocalScale:
                    transformData.Value.localScale = vec3.Value;
                    break;
                case Poperty.GetLossyScale:
                    vec3.Value = transformData.Value.lossyScale;
                    break;
                case Poperty.GetPosition:
                    vec3.Value = transformData.Value.position;
                    break;
                case Poperty.SetPosition:
                    transformData.Value.position = vec3.Value;
                    break;
                case Poperty.GetRight:
                    vec3.Value = transformData.Value.right;
                    break;
                case Poperty.SetRight:
                    transformData.Value.right = vec3.Value;
                    break;
                case Poperty.GetUp:
                    vec3.Value = transformData.Value.up;
                    break;
                case Poperty.SetUp:
                    transformData.Value.up = vec3.Value;
                    break;
                default:
                    break;
            }

            Continue();
        }

        public override string GetSummary()
        {
            return property.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

    }
}