using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Save the current screen to a named png
    /// </summary>
    [CommandInfo("Camera",
                 "Take Screenshot",
                 "Save the current screen to a named png.")]
    [AddComponentMenu("")]
    public class TakeScreenshot : Command
    {
        [Tooltip("Filename to be saved under")]
        [SerializeField]
        protected StringData screenShotFileName = new StringData("screenshot");

        [Tooltip("SuperSized screenshot")]
        [SerializeField]
        protected IntegerData superSized = new IntegerData(1);

        public override void OnEnter()
        {
            Application.CaptureScreenshot(screenShotFileName.Value, superSized);

            Continue();
        }

        public override string GetSummary()
        {
            return "Screenshot";
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

    }
}