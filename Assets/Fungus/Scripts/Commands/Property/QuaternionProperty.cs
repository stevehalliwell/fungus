using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0 typeo
//1 prop enum
//2 lower class name
//3 get generated
//4 set generated
//5 used vars
//6 variableProperty Type of

namespace Fungus
{
    // <summary>
    /// Get or Set a property of a Quaternion component
    /// </summary>
    [CommandInfo("Quaternion",
                 "Property",
                 "Get or Set a property of a Quaternion component")]
    [AddComponentMenu("")]
    public class QuaternionProperty : BaseVariableProperty
    {
        //generated property
        public enum Property
        {
            X,
            Y,
            Z,
            W,
            EulerAngles,
        }


        [SerializeField]
        protected Property property;

        [SerializeField]
        protected QuaternionData quaternionData;

        [SerializeField]
        [VariableProperty(typeof(FloatVariable),
typeof(Vector3Variable))]

        protected Variable inOutVar;

        public override void OnEnter()
        {
            var iof = inOutVar as FloatVariable;
            var iov = inOutVar as Vector3Variable;


            var target = quaternionData.Value;

            switch (getOrSet)
            {
                case GetSet.Get:
                    switch (property)
                    {
                        case Property.X:
                            iof.Value = target.x;
                            break;
                        case Property.Y:
                            iof.Value = target.y;
                            break;
                        case Property.Z:
                            iof.Value = target.z;
                            break;
                        case Property.W:
                            iof.Value = target.w;
                            break;
                        case Property.EulerAngles:
                            iov.Value = target.eulerAngles;
                            break;
                        default:
                            Debug.Log("Unsupported get or set attempted");
                            break;
                    }

                    break;
                case GetSet.Set:
                    switch (property)
                    {
                        case Property.X:
                            target.x = iof.Value;
                            break;
                        case Property.Y:
                            target.y = iof.Value;
                            break;
                        case Property.Z:
                            target.z = iof.Value;
                            break;
                        case Property.W:
                            target.w = iof.Value;
                            break;
                        case Property.EulerAngles:
                            target.eulerAngles = iov.Value;
                            break;
                        default:
                            Debug.Log("Unsupported get or set attempted");
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
            if (inOutVar == null)
            {
                return "Error: no variable set to push or pull data to or from";
            }

            return getOrSet.ToString() + " " + property.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable variable)
        {
            if (quaternionData.quaternionRef == variable || inOutVar == variable)
                return true;

            return false;
        }

    }
}