using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Lerp from a start to end point based on percentage.
    /// </summary>
    [CommandInfo("Vector3",
                 "Lerp",
                 "Lerp from a start to end point based on percentage.")]
    [AddComponentMenu("")]
    public class Vector3MoveTowards : Command
    {
        [SerializeField]
        protected Vector3Data a, b;

        [Tooltip("Maxiumum step to take this frame")]
        [SerializeField]
        protected FloatData maxDelta;

        //[Tooltip("Where the result of the function is stored.")]
        [SerializeField]
        protected Vector3Data outValue;

        [Tooltip("If true, will multiply the maxDelta by the current Time.DeltaTime")]
        [SerializeField]
        protected bool multiplyMaxDeltaByDeltaTime = true;

        public override void OnEnter()
        {
            outValue.Value = Vector3.MoveTowards(a, b, (multiplyMaxDeltaByDeltaTime ? maxDelta * Time.deltaTime : maxDelta));

            Continue();
        }

        public override string GetSummary()
        {
            if (outValue.vector3Ref == null)
            {
                return "Error: no output set";
            }
            
            return "Stored in " + outValue.vector3Ref.Key; ;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}