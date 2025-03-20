using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter {

    public class TestStartAddressables : MonoBehaviour
    {
        // 필드 (Fields)
        public string loadingSceneKey;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        public void OnStartLoadingScene()
        {
            Addressables.LoadSceneAsync(loadingSceneKey, LoadSceneMode.Single);
        }
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class TestStart

} // namespace Root