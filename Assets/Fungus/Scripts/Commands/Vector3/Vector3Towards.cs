using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Move or rotate from a start to end point based on step size.
    /// </summary>
    [CommandInfo("Vector3",
                 "Towards",
                 "Move or rotate from a start to end point based on step size.")]
    [AddComponentMenu("")]
    public class Vector3Towards : Command
    {
        public enum Mode
        {
            Move,
            Rotate
        }

        [SerializeField]
        protected Mode mode = Mode.Move;

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
            if(mode == Mode.Move)
                outValue.Value = Vector3.MoveTowards(a, b, (multiplyMaxDeltaByDeltaTime ? maxDelta * Time.deltaTime : maxDelta));
            else
                outValue.Value = Vector3.RotateTowards(a, b, (multiplyMaxDeltaByDeltaTime ? maxDelta * Time.deltaTime : maxDelta),0);


            Continue();
        }

        public override string GetSummary()
        {
            if (outValue.vector3Ref == null)
            {
                return "Error: no output set";
            }
            
            return mode.ToString() + " stored in " + outValue.vector3Ref.Key;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}