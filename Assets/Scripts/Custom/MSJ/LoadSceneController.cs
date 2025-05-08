using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class LoadSceneController : MonoBehaviour
    {
        // 필드 (Fields)
        public string sceneName;
        public Slider progressBar;
        private float progress;
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
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                progress = Mathf.Clamp01(operation.progress / 0.9f);
                progressBar.value = progress;

                if (progress >= 0.2f && progress <= 0.3f)
                {
                    yield return new WaitForSeconds(0.5f);
                }

                if (progress >= 0.5f && progress <= 0.6f)
                {
                    yield return new WaitForSeconds(0.7f);
                }

                if (progress >= 0.8f && progress <= 0.9f)
                {
                    yield return new WaitForSeconds(1f);
                }

                if (progress >= 0.9f)
                {
                    yield return new WaitForSeconds(2f);
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }


    } // Scope by class LoadSceneController

} // namespace Root