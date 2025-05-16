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

        void Start() => StartCoroutine(LoadSceneProcess());

        /* ---------- 공통 다운로드 ---------- */
        IEnumerator Loading(string label, string text)
        {
            textUI.text = text;

            var sizeHandle = Addressables.GetDownloadSizeAsync(label);
            yield return sizeHandle;

            long totalBytes = sizeHandle.Result;
            if (totalBytes <= 0) yield break;

            var downloadHandle = Addressables.DownloadDependenciesAsync(label, true);
            while (!downloadHandle.IsDone)
            {
                float pct = downloadHandle.PercentComplete;
                progressBar.value = pct;
                textUI.text = $"{text} {(pct * 100f):F0}%";
                yield return null;
            }
            Addressables.Release(downloadHandle);
        }

        /* ---------- 메모리 선-로딩 ---------- */
        IEnumerator LoadMemory<T>(string label) where T : Object
        {
            var handle = Addressables.LoadAssetsAsync<T>(label, null);
            yield return handle;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                textUI.text = "로드 실패!";

            Addressables.Release(handle);
        }

        /* ---------- 메인 프로세스 ---------- */
        IEnumerator LoadSceneProcess()
        {
            DataTableMgr.InitOnSceneLoaded(nextScene);

            GameMgr.InitializeAddressablesIfNeeded();
            if (!Addressables.InitializationOperation.IsDone)
                yield return Addressables.InitializationOperation;

            /* 의존성 다운로드 */
            yield return Loading(scenesLabel, "씬 다운로드 중");
            yield return Loading(fontLabel, "리소스 다운로드 중...");
            yield return LoadMemory<TMP_FontAsset>(fontLabel);
            yield return Loading(prefabLabel, "프리팹 다운로드 중...");
            yield return Loading(soLabel, "스크립터블 오브젝트 다운로드 중...");
            yield return Loading(textureLabel, "텍스처 다운로드 중...");
            yield return Loading(soundLabel, "사운드 다운로드 중...");
            yield return Loading(animLabel, "애니메이션 다운로드 중...");
            yield return Loading(matLabel, "머티리얼 다운로드 중...");
            yield return Loading(shaderLabel, "셰이더 다운로드 중...");
            yield return Loading(sanctumLabel, "리소스 다운로드 중...");

            /* 씬 로드 */
            var loadHandle = Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Single,
                                                         activateOnLoad: false);

            while (!loadHandle.IsDone)
            {
                float pct = loadHandle.PercentComplete;
                progressBar.value = pct;
                textUI.text = $"Loading Scene {(pct * 100f):F0}%";
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
