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
            nextScene = sceneName;
            SceneManager.LoadScene("LoadingScene");
        }

        private void Start()
        {
            StartCoroutine(this.LoadSceneProcess());
        }

        private IEnumerator Loading(string label, string text)
        {
            textUI.text = text;
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

                    textUI.text = text + $" {downloadedKB:F1} KB / {totalKB:F1} KB";
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
            DataTableMgr.InitOnSceneLoaded(nextScene);
            // GameMgr.InitializeAddressablesIfNeeded();
            yield return Addressables.InitializeAsync();
            yield return Loading(fontLabel, "리소스 다운로드 중...");
            yield return Loading(prefabLabel, "프리팹 다운로드 중...");
            yield return Loading(soLabel, "스크립터블 오브젝트 다운로드 중...");
            yield return Loading(textureLabel, "텍스처 다운로드 중...");
            yield return Loading(soundLabel, "사운드 다운로드 중...");
            yield return Loading(animLabel, "애니메이션 다운로드 중...");
            yield return Loading(matLabel, "머티리얼 다운로드 중...");
            yield return Loading(shaderLabel, "셰이더 다운로드 중...");
            yield return Loading(sanctumLabel, "리소스 다운로드 중...");
            yield return Loading(scenesLabel, "씬 다운로드 중...");

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
                Debug.LogError($"씬 로드 실패: {loadHandle.OperationException}");
                textUI.text = "Failed!";
            }
        }
    }
}