using System.Collections;
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
        static string nextScene;

        [SerializeField] private TextMeshProUGUI textUI;
        [SerializeField] private Slider progressBar;

        public static void LoadScene(string sceneName)
        {
            nextScene = sceneName;
            SceneManager.LoadScene("LoadingScene");
        }

        private void Start()
        {
            StartCoroutine(this.LoadSceneProcess());
        }

        private IEnumerator LoadSceneProcess()
        {
            DataTableMgr.InitOnSceneLoaded(nextScene);
            GameMgr.InitializeAddressablesIfNeeded(); 
            yield return Addressables.InitializeAsync();

            // Get size
            var sizeHandle = Addressables.GetDownloadSizeAsync(nextScene);
            yield return sizeHandle;

            long totalBytes = sizeHandle.Result;
            if (totalBytes > 0)
            {
                var downloadHandle = Addressables.DownloadDependenciesAsync(nextScene);
                while (!downloadHandle.IsDone)
                {
                    float percent = downloadHandle.PercentComplete;
                    float downloadedKB = percent * totalBytes / 1024f;
                    float totalKB = totalBytes / 1024f;

                    textUI.text = $"Downloading {downloadedKB:F1} KB / {totalKB:F1} KB";
                    progressBar.value = percent;
                    yield return null;
                }
                Addressables.Release(downloadHandle);
            }

            // Load scene
            AsyncOperationHandle<SceneInstance> loadHandle =
                Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Single, activateOnLoad: false);

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
                textUI.text = "Failed!";
            }
        }
    }
}
