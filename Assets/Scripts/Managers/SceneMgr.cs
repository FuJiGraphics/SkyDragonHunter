using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SkyDragonHunter.Managers
{
    public class SceneMgr : MonoBehaviour
    {
        static string s_NextScene = "GameScene";
        static bool s_IsFirstStart = true;
        private bool m_IsStartedScene = false;

        [SerializeField] private TextMeshProUGUI textUI;
        [SerializeField] private Slider progressBar;
        [SerializeField] private string fontLabel = "Fonts";
        [SerializeField] private string prefabLabel = "Prefabs";
        [SerializeField] private string soLabel = "ScriptableObjects";
        [SerializeField] private string textureLabel = "Textures";
        [SerializeField] private string soundLabel = "Sounds";
        [SerializeField] private string animLabel = "Animations";
        [SerializeField] private string matLabel = "Materials";
        [SerializeField] private string shaderLabel = "Shaders";
        [SerializeField] private string sanctumLabel = "sanctum_pixel";
        [SerializeField] private string scenesLabel = "Scenes";

        public static void LoadScene(string sceneName)
        {
            s_NextScene = sceneName;
            SceneManager.LoadScene("LoadingScene");
        }

        private void Start()
        {
            if (!s_IsFirstStart)
            {
                StartCoroutine(this.LoadSceneProcess());
            }
        }
        
        public void StartScene(string sceneName)
        {
            if (m_IsStartedScene)
                return;

            s_NextScene = sceneName;
            m_IsStartedScene = true;
            StartCoroutine(this.LoadSceneProcess());
        }

        private IEnumerator Loading(string label, string title, string downText)
        {
            textUI.text = title;
            var fontSizeHandle = Addressables.GetDownloadSizeAsync(label);
            yield return fontSizeHandle;

            long fontTotalBytes = fontSizeHandle.Result;
            if (fontTotalBytes > 0)
            {
                var fontDownloadHandle = Addressables.DownloadDependenciesAsync(label);
                while (!fontDownloadHandle.IsDone)
                {
                    float percent = fontDownloadHandle.PercentComplete;
                    float downloadedKB = percent * fontTotalBytes / 1024f;
                    float totalKB = fontTotalBytes / 1024f;

                    textUI.text = downText + $" {downloadedKB:F1} KB / {totalKB:F1} KB";
                    progressBar.value = percent;
                    yield return null;
                }
                Addressables.Release(fontDownloadHandle);
            }
        }

        private IEnumerator LoadMemory<T>(string label)
            where T : UnityEngine.Object
        {
            var fontHandle = Addressables.LoadAssetAsync<T>(fontLabel);
            yield return fontHandle;

            if (fontHandle.Status == AsyncOperationStatus.Succeeded)
            {
                T loadedFont = fontHandle.Result;
            }
            else
            {
                textUI.text = "로드 실패!";
            }
        }

        private IEnumerator LoadSceneProcess()
        {
            textUI.text = "Initialize ...";
            yield return new WaitForSeconds(1);

            s_IsFirstStart = false;

            DataTableMgr.InitOnSceneLoaded(s_NextScene);

            // GameMgr.InitializeAddressablesIfNeeded();
            var initHandle = Addressables.InitializeAsync();
            if (initHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Addressables 초기화 실패!");
                textUI.text = $"Initialization Failed! : {initHandle.OperationException}";
                yield break;
            }

            yield return Loading(fontLabel, "Loading Fonts...", "Downloading Fonts...");
            yield return Loading(prefabLabel, "Loading Prefabs...", "Downloading Prefabs...");
            yield return Loading(soLabel, "Loading Scriptable Objects...", "Downloading Scriptable Objects...");
            yield return Loading(textureLabel, "Loading Textures...", "Downloading Textures...");
            yield return Loading(soundLabel, "Loading Sounds...", "Downloading Sounds...");
            yield return Loading(animLabel, "Loading Animations...", "Downloading Animations...");
            yield return Loading(matLabel, "Loading Materials...", "Downloading Materials...");
            yield return Loading(shaderLabel, "Loading Shaders...", "Downloading Shaders...");
            yield return Loading(sanctumLabel, "Loading Resources...", "Downloading Resources...");
            yield return Loading(scenesLabel, "Loading Scenes...", "Downloading Scenes...");

            // Load scene
            AsyncOperationHandle<SceneInstance> loadHandle =
                Addressables.LoadSceneAsync(s_NextScene, LoadSceneMode.Single, activateOnLoad: false);

            while (!loadHandle.IsDone && loadHandle.PercentComplete < 0.9f)
            {
                float percent = loadHandle.PercentComplete;
                textUI.text = $"Loading Scene {percent * 100f:F0}%";
                progressBar.value = percent;
                yield return null;
            }

            // Smooth finish to 100%
            float timer = 0f;
            while (timer < 1f)
            {
                timer += Time.unscaledDeltaTime;
                progressBar.value = Mathf.Lerp(0.9f, 1f, timer);
                textUI.text = $"Loading Scene {(progressBar.value * 100f):F0}%";
                yield return null;
            }

            progressBar.value = 1f;

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                textUI.text = "Complete!";
                yield return loadHandle.Result.ActivateAsync();
            }
            else
            {
                Debug.LogError($"씬 로드 실패: {loadHandle.OperationException}");
                textUI.text = $"Failed! : {loadHandle.OperationException}";
            }

            m_IsStartedScene = false;
        }
    }
}