using SkyDragonHunter.Managers;
using SkyDragonHunter.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter
{
    public static class SceneChangeMgr
    {
        public static event UnityAction beforeSceneUnloaded;
        private static Scene m_currentScene;

        static SceneChangeMgr()
        {
                      
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            m_currentScene = scene;
        }

        public static void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Debug.Log($"LoadedScene Called with string [{sceneName}], currentScene : [{m_currentScene.name}]");
            if (m_currentScene.name == "GameScene" || m_currentScene.name == "DungeonScene")
            {
                beforeSceneUnloaded?.Invoke();
                SaveLoadMgr.ResetLoaded();
            }
            SceneMgr.LoadScene(sceneName);
        }

        public static void LoadScene(int sceneBuildIndex, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Debug.Log($"LoadedScene Called with buildIndex [{sceneBuildIndex}/{(SceneIds)sceneBuildIndex}], currentScene : [{m_currentScene.name}]");
            if (m_currentScene.name == "GameScene" || m_currentScene.name == "DungeonScene")
            {
                beforeSceneUnloaded?.Invoke();
                SaveLoadMgr.ResetLoaded();
            }
            // SceneMgr.LoadScene(sceneName);
        }

    } // Scope by class SceneChangeMgr

} // namespace Root