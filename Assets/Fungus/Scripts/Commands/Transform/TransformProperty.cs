using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    // <summary>
    /// Get or Set a property of a transform component
    /// </summary>
    [CommandInfo("Transform",
                 "Property",
                 "Get or Set a property of a transform component")]
    [AddComponentMenu("")]
    public class TransformProperty : Command
    {
        public enum GetSet
        {
            Get,
            Set,
        }
        public GetSet getOrSet = GetSet.Get;

        public enum Poperty
        {
            ChildCount,
            EulerAngles,
            Forward,
            HasChanged,
            HierarchyCapacity,
            HierarchyCount,
            LocalEulerAngles,
            LocalPosition,
            LocalScale,
            LossyScale,
            Parent,
            Position,
            Right,
            Root,
            Up,
            //no quat or mat4 yet
            //LocalRotation,
            //Rotation,
            //LocalToWorldMatrix,
            //WorldToLocalMatrix
        }
        [SerializeField]
        protected Poperty property = Poperty.Position;

        [SerializeField]
        protected TransformData transformData;
        
        [SerializeField]
        [VariableProperty(typeof(BooleanVariable),
                          typeof(IntegerVariable),
                          typeof(Vector3Variable),
                          typeof(TransformVariable))]
        protected Variable inOutVar;

        public override void OnEnter()
        {
            var iob = inOutVar as BooleanVariable;
            var ioi = inOutVar as IntegerVariable;
            var iov = inOutVar as Vector3Variable;
            var iot = inOutVar as TransformVariable;

            var t = transformData.Value;

            switch (getOrSet)
            {
                case GetSet.Get:
                    switch (property)
                    {
                        case Poperty.ChildCount:
                            ioi.Value = t.childCount;
                            break;
                        case Poperty.EulerAngles:
                            iov.Value = t.eulerAngles;
                            break;
                        case Poperty.Forward:
                            iov.Value = t.forward;
                            break;
                        case Poperty.HasChanged:
                            iob.Value = t.hasChanged;
                            break;
                        case Poperty.HierarchyCapacity:
                            ioi.Value = t.hierarchyCapacity;
                            break;
                        case Poperty.HierarchyCount:
                            ioi.Value = t.hierarchyCount;
                            break;
                        case Poperty.LocalEulerAngles:
                            iov.Value = t.localEulerAngles;
                            break;
                        case Poperty.LocalPosition:
                            iov.Value = t.localPosition;
                            break;
                        case Poperty.LocalScale:
                            iov.Value = t.localScale;
                            break;
                        case Poperty.LossyScale:
                            iov.Value = t.lossyScale;
                            break;
                        case Poperty.Parent:
                            iot.Value = t.parent;
                            break;
                        case Poperty.Position:
                            iov.Value = t.position;
                            break;
                        case Poperty.Right:
                            iov.Value = t.right;
                            break;
                        case Poperty.Root:
                            iot.Value = t.parent;
                            break;
                        case Poperty.Up:
                            iov.Value = t.up;
                            break;
                        default:
                            break;
                    }
                    break;
                case GetSet.Set:
                    switch (property)
                    {
                        case Poperty.ChildCount:
                            Debug.LogWarning("Cannot Set childCount, it is read only");
                            break;
                        case Poperty.EulerAngles:
                            t.eulerAngles = iov.Value;
                            break;
                        case Poperty.Forward:
                            t.forward = iov.Value;
                            break;
                        case Poperty.HasChanged:
                            t.hasChanged = iob.Value;
                            break;
                        case Poperty.HierarchyCapacity:
                            t.hierarchyCapacity = ioi.Value;
                            break;
                        case Poperty.HierarchyCount:
                            Debug.LogWarning("Cannot Set HierarchyCount, it is read only");
                            break;
                        case Poperty.LocalEulerAngles:
                            t.localEulerAngles = iov.Value;
                            break;
                        case Poperty.LocalPosition:
                            t.localPosition = iov.Value;
                            break;
                        case Poperty.LocalScale:
                            t.localScale = iov.Value;
                            break;
                        case Poperty.LossyScale:
                            Debug.LogWarning("Cannot Set LossyScale, it is read only");
                            break;
                        case Poperty.Parent:
                            t.parent = iot.Value;
                            break;
                        case Poperty.Position:
                            t.position = iov.Value;
                            break;
                        case Poperty.Right:
                            t.right = iov.Value;
                            break;
                        case Poperty.Root:
                            Debug.LogWarning("Cannot Set Root, it is read only");
                            break;
                        case Poperty.Up:
                            t.up = iov.Value;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }



            Continue();
        }

        public override string GetSummary()
        {
            return getOrSet.ToString() + " " + property.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

    }
}