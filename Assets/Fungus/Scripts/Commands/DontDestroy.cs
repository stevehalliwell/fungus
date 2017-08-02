using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Mark a gameobject as dont destroy on load
    /// </summary>
    [CommandInfo("GameObject",
                 "DontDestroyOnLoad",
                 "Mark a gameobject as dont destroy on load.")]
    [AddComponentMenu("")]
    public class DontDestroy : Command
    {
        [Tooltip("GameObject to mark as Don'tDestroyOnLoad")]
        [SerializeField]
        protected GameObjectData gameObjectVar;

        public override void OnEnter()
        {
            DontDestroyOnLoad(gameObjectVar.Value);

            Continue();
        }

        public override string GetSummary()
        {
            var g = gameObjectVar.Value; 

            return "DontDestroy" + (g!=null ? g.name : "");
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

    }
}