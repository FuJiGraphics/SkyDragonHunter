using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SkyDragonHunter.Managers
{
    public class CoroutineHost : MonoBehaviour
    {
        private static CoroutineHost _instance;

        public static CoroutineHost Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CoroutineHost>();
                    if (_instance == null)
                    {
                        var go = new GameObject("[CoroutineHost]");
                        _instance = go.AddComponent<CoroutineHost>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }
    }

    public class SceneMgr : MonoBehaviour
    {
        static string nextScene = "GameScene";
        static string currentScene;
        static bool isFirstStart = true;

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

        private float m_StartDelay = 1f;

        public static void LoadScene(string sceneName)
        {
            nextScene = sceneName;
            currentScene = SceneManager.GetActiveScene().name;
            CoroutineHost.Instance.StartCoroutine(LoadLoadingSceneAsync());
        }

        public void StartScene(string sceneName)
        {
            nextScene = sceneName;
            currentScene = SceneManager.GetActiveScene().name;
            isFirstStart = false;
            StartCoroutine(DelayedStart());
        }

        private static IEnumerator LoadLoadingSceneAsync()
        {
            AsyncOperation handle = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
            while (!handle.isDone)
            {
                yield return null;
            }
            if (currentScene != "LoadingScene")
            {
                SceneManager.UnloadSceneAsync(currentScene);
            }
        }

        private void Start()
        {
            if (!isFirstStart)
            {
                StartCoroutine(DelayedStart());
            }
        }

        private IEnumerator DelayedStart()
        {
            isFirstStart = false;
            yield return null;

            textUI.text = "Preparing to Load...";
            progressBar.value = 0;

            yield return new WaitForSecondsRealtime(0.5f);

            var initHandle = Addressables.InitializeAsync();
            while (!initHandle.IsDone)
            {
                textUI.text = $"Initializing... {(initHandle.PercentComplete * 100f):F0}%";
                progressBar.value = initHandle.PercentComplete;
                yield return null;
            }

            yield return new WaitForSecondsRealtime(m_StartDelay);
            StartCoroutine(LoadSceneProcess());
        }

        private IEnumerator Loading(string label, string title, string downText)
        {
            textUI.text = title;
            var sizeHandle = Addressables.GetDownloadSizeAsync(label);
            yield return sizeHandle;

            long totalBytes = sizeHandle.Result;
            if (totalBytes > 0)
            {
                var downloadHandle = Addressables.DownloadDependenciesAsync(label);
                while (!downloadHandle.IsDone)
                {
                    float percent = downloadHandle.PercentComplete;
                    float downloadedKB = percent * totalBytes / 1024f;
                    float totalKB = totalBytes / 1024f;

                    textUI.text = downText + $" {downloadedKB:F1} KB / {totalKB:F1} KB";
                    progressBar.value = percent;
                    yield return null;
                }
                Addressables.Release(downloadHandle);
            }
        }

        private IEnumerator LoadAllLabelsInParallel()
        {
            var labels = new[]
            {
                fontLabel, prefabLabel, soLabel, textureLabel,
                soundLabel, animLabel, matLabel, shaderLabel,
                sanctumLabel, scenesLabel
            };

            List<AsyncOperationHandle> downloadHandles = new();

            foreach (string label in labels)
            {
                var sizeHandle = Addressables.GetDownloadSizeAsync(label);
                yield return sizeHandle;

                if (sizeHandle.Result > 0)
                {
                    var handle = Addressables.DownloadDependenciesAsync(label);
                    downloadHandles.Add(handle);
                }
            }

            while (!downloadHandles.All(h => h.IsDone))
            {
                float total = downloadHandles.Count;
                float progress = downloadHandles.Sum(h => h.PercentComplete) / total;
                progressBar.value = progress;
                textUI.text = $"Downloading... {(progress * 100f):F0}%";
                yield return null;
            }

            foreach (var handle in downloadHandles)
            {
                Addressables.Release(handle);
            }
        }

        private IEnumerator LoadSceneProcess()
        {
            DataTableMgr.InitOnSceneLoaded(nextScene);

            yield return LoadAllLabelsInParallel();
            yield return LoadTargetScene();
        }

        private IEnumerator LoadMemory<T>(string label) where T : UnityEngine.Object
        {
            var handle = Addressables.LoadAssetAsync<T>(label);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                T loaded = handle.Result;
                // You can store/use loaded if needed
            }
            else
            {
                textUI.text = "로드 실패!";
            }
        }

        private IEnumerator LoadTargetScene()
        {
            var loadHandle = Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Single, false);

            float lastProgress = -1f;
            while (!loadHandle.IsDone && loadHandle.PercentComplete < 0.9f)
            {
                float progress = loadHandle.PercentComplete;
                if (Mathf.Abs(progress - lastProgress) > 0.01f)
                {
                    progressBar.value = progress;
                    textUI.text = $"Loading Scene {(progress * 100f):F0}%";
                    lastProgress = progress;
                }
                yield return null;
            }

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
                textUI.text = "Failed!";
            }
        }
    }
}
