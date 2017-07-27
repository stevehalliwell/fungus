using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Calls Debug.DrawLine
    /// </summary>
    [CommandInfo("Debug",
                 "Draw",
                 "Draws a debug line between 2 given points")]
    [AddComponentMenu("")]
    public class DrawLine : Command
    {
        public enum Mode { Line, RayFromLine, Ray}


        [Tooltip("Mode to use, Line = DrawLine, RayFromLine = DrawRay with calculated direction, Ray = DrawRay")]
        [SerializeField]
        protected Mode mode = Mode.Line;

        [Tooltip("Start of the debug line")]
        [SerializeField]
        protected Vector3Data startPoint;

        [Tooltip("End of the debug line or if Ray mode, the direction of the ray.")]
        [SerializeField]
        protected Vector3Data endPointOrDir;

        [Tooltip("Color of the line/Ray")]
        [SerializeField]
        protected ColorData lineColor = new ColorData(Color.green);

        [Tooltip("Length of Ray")]
        [SerializeField]
        protected FloatData lengthOfRay = new FloatData(1);

        [Tooltip("Duration of the debug")]
        [SerializeField]
        protected FloatData showDebugDrawFor;
        
        [Tooltip("Does the debug draw over other things or does it get occuled by depth.")]
        [SerializeField]
        protected BooleanData depthTest = new BooleanData(false);

        public override void OnEnter()
        {
            var flowchart = GetFlowchart();

            switch (mode)
            {
                case Mode.Line:
                        Debug.DrawLine(startPoint.Value, endPointOrDir.Value, lineColor.Value, showDebugDrawFor, depthTest);
                    break;
                case Mode.RayFromLine:
                    Debug.DrawRay(startPoint.Value, (endPointOrDir.Value - startPoint.Value).normalized * lengthOfRay, lineColor.Value,showDebugDrawFor,depthTest);
                    break;
                case Mode.Ray:
                    Debug.DrawRay(startPoint.Value, endPointOrDir.Value * lengthOfRay, lineColor.Value, showDebugDrawFor, depthTest);
                    break;
                default:
                    break;
            }


            Continue();
        }

        public override string GetSummary()
        {
            return "Draws a debug line";
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
        
    }
}