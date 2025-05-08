using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace SkyDragonHunter {

    public class LoadSceneControllerAddressables : MonoBehaviour
    {
        // 필드 (Fields)
        public string sceneName;
        public Slider progressBar;
        private float fakeProgress;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            StartCoroutine(OnLoadNextScene());
        }
    
        // Public 메서드
        // Private 메서드
        // Others
        IEnumerator OnLoadNextScene()
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!handle.IsDone)
            {
                //progressBar.value = Mathf.Clamp01(handle.PercentComplete);

                // 페이크 딜레이
                fakeProgress = Mathf.MoveTowards(fakeProgress, handle.PercentComplete, Time.deltaTime * 0.5f);
                progressBar.value = Mathf.Clamp01(fakeProgress);

                yield return null; // 다음 프레임까지 대기
                // 페이크 딜레이
            }
            yield return new WaitForSeconds(2f); // 페이크 딜레이 원래는 null
            progressBar.value = 1f; // 로딩 완료 // 페이크 딜레이용
        }


    } // Scope by class LoadSceneController

} // namespace Root