using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fungus
{
    /// <summary>
    /// The block will execute when the desired SceneManager delegate is triggered.
    /// </summary>
    [EventHandlerInfo("Scene",
                      "LevelLoaded",
                      "The block will execute when the desired SceneManager delegate is triggered.")]
    [AddComponentMenu("")]
    public class LevelLoad : EventHandler
    {

        [System.Flags]
        public enum LevelLoadedMessageFlags
        {
            OnActiveSceneChanged = 1 << 0,
            OnSceneLoaded = 1 << 1,
            OnSceneUnLoaded = 1 << 2,
        }

        [Tooltip("Which of the SceneManager delegates to trigger on.")]
        [SerializeField]
        [EnumFlag]
        protected LevelLoadedMessageFlags FireOn = LevelLoadedMessageFlags.OnSceneLoaded;

        [Tooltip("Name of scene to filter event by, if empty any scene passes.")]
        [SerializeField]
        protected string SceneName;


        private void Start()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private void SceneManager_sceneUnloaded(Scene arg0)
        {
            ProcessSceneName(LevelLoadedMessageFlags.OnSceneUnLoaded, arg0.name);
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            ProcessSceneName(LevelLoadedMessageFlags.OnSceneLoaded, arg0.name);
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            ProcessSceneName(LevelLoadedMessageFlags.OnActiveSceneChanged, arg0.name);
        }

        private void ProcessSceneName(LevelLoadedMessageFlags mode, string sceneName)
        {
            if ((FireOn & mode) != 0 && SceneName == sceneName)
                ExecuteBlock();
        }

        public override string GetSummary()
        {
            //TODO
            return "None";
        }
    }
}
