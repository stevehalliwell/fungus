/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEngine;


namespace Fungus
{
    // <summary>
    /// Get or Set a property of a ControllerColliderHit component
    /// </summary>
    [CommandInfo("ControllerColliderHit",
                 "Property",
                 "Get or Set a property of a ControllerColliderHit component")]
    [AddComponentMenu("")]
    public class ControllerColliderHitProperty : BaseVariableProperty
    {
		//generated property
        public enum Property 
        { 
            Collider, 
            Rigidbody, 
            GameObject, 
            Transform, 
            Point, 
            Normal, 
            MoveDirection, 
            MoveLength, 
        }

		
        [SerializeField]
        protected Property property;
		
        [SerializeField]
        [VariableProperty(typeof(ControllerColliderHitVariable))]
        protected ControllerColliderHitVariable controllerColliderHitVar;

        [SerializeField]
        [VariableProperty(typeof(ColliderVariable),
                          typeof(RigidbodyVariable),
                          typeof(GameObjectVariable),
                          typeof(TransformVariable),
                          typeof(Vector3Variable),
                          typeof(FloatVariable))]
        protected Variable inOutVar;

        public override void OnEnter()
        {
            var ioc = inOutVar as ColliderVariable;
            var iorb = inOutVar as RigidbodyVariable;
            var iogo = inOutVar as GameObjectVariable;
            var iot = inOutVar as TransformVariable;
            var iov = inOutVar as Vector3Variable;
            var iof = inOutVar as FloatVariable;


            var target = controllerColliderHitVar.Value;

            switch (getOrSet)
            {
                case GetSet.Get:
                    switch (property)
                    {
                        case Property.Collider:
                            ioc.Value = target.collider;
                            break;
                        case Property.Rigidbody:
                            iorb.Value = target.rigidbody;
                            break;
                        case Property.GameObject:
                            iogo.Value = target.gameObject;
                            break;
                        case Property.Transform:
                            iot.Value = target.transform;
                            break;
                        case Property.Point:
                            iov.Value = target.point;
                            break;
                        case Property.Normal:
                            iov.Value = target.normal;
                            break;
                        case Property.MoveDirection:
                            iov.Value = target.moveDirection;
                            break;
                        case Property.MoveLength:
                            iof.Value = target.moveLength;
                            break;
                        default:
                            Debug.Log("Unsupported get or set attempted");
                            break;
                    }

                    break;
                case GetSet.Set:
                    switch (property)
                    {
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
            if (controllerColliderHitVar == null)
            {
                return "Error: no controllerColliderHitVar set";
            }
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
            if (controllerColliderHitVar == variable || inOutVar == variable)
                return true;

            return false;
        }

    }
}