using SkyDragonHunter.Managers;
using UnityEngine.Events;
using UnityEngine.Internal;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter {

    public static class SceneChangeMgr
    {
        public static event UnityAction<Scene> beforeSceneLoad;
        private static Scene currentScene;

        static SceneChangeMgr()
        {
            currentScene = default;
        }

        public static void SetCurrentScene(Scene scene)
        {
            currentScene = scene;
        }

        public static void LoadScene(string sceneName, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
        {
            if (beforeSceneLoad != null)
            {
                beforeSceneLoad(currentScene);
            }
            SaveLoadMgr.ResetLoaded();
            SceneManager.LoadScene(sceneName, mode);
        }

        public static void LoadScene(string sceneName)
        {
            if (beforeSceneLoad != null)
            {
                beforeSceneLoad(currentScene);
            }
            SaveLoadMgr.ResetLoaded();
            LoadSceneMode mode = LoadSceneMode.Single;
            SceneManager.LoadScene(sceneName, mode);
        }

        public static void LoadScene(int sceneBuildIndex, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
        {
            if (beforeSceneLoad != null)
            {
                beforeSceneLoad(currentScene);
            }
            SaveLoadMgr.ResetLoaded();
            SceneManager.LoadScene(sceneBuildIndex, mode);
        }

        public static void LoadScene(int sceneBuildIndex)
        {
            if (beforeSceneLoad != null)
            {
                beforeSceneLoad(currentScene);
            }
            SaveLoadMgr.ResetLoaded();
            LoadSceneMode mode = LoadSceneMode.Single;
            SceneManager.LoadScene(sceneBuildIndex, mode);
        }


    } // Scope by class SceneChangeMgr

} // namespace Root