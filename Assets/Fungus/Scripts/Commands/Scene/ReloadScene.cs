using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Reload the current scene
    /// </summary>
    [CommandInfo("Scene",
                 "Clamp",
                 "Reload the current scene")]
    [AddComponentMenu("")]
    public class ReloadScene : Command
    {
        [Tooltip("Image to display while loading the scene")]
        [SerializeField]
        protected Texture2D loadingImage;

        public override void OnEnter()
        {
            SceneLoader.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, loadingImage);

            Continue();
        }

        public override string GetSummary()
        {
            return "Reload the current scene";
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }
}