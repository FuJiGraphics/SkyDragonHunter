using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers
{
    [DefaultExecutionOrder(-99)] // 가장 먼저 실행되도록 함
    public static class DrawableMgr
    {
        // 필드 (Fields)
        public static GameObject s_UIDamageMeterPrefab;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            Debug.Log("DrawableMgr Init");
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"[DrawableMgr] 씬 로드됨: {scene.name}");
            Debug.Log($"[DrawableMgr] UIDamageMeter Prefab 생성중");
            s_UIDamageMeterPrefab = Resources.Load<GameObject>("Prefabs/UIDamageMeter");
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            Debug.Log($"[DrawableMgr] UIDamageMeter Prefab 정리중");
            s_UIDamageMeterPrefab = null;
            Debug.Log($"[DrawableMgr] 씬 언로드됨: {scene.name}");
        }

        public static void Text(Vector2 position, string str)
        {
            GameObject damageMeterUI = GameObject.Instantiate(s_UIDamageMeterPrefab);
            damageMeterUI.transform.position = position;
            damageMeterUI.GetComponent<UIDamageMeter>().SetText(str);
        }

        public static void Text(Vector2 position, string str, Color color)
        {
            GameObject damageMeterUI = GameObject.Instantiate(s_UIDamageMeterPrefab);
            damageMeterUI.transform.position = position;
            var meter = damageMeterUI.GetComponent<UIDamageMeter>();
            meter.SetText(str);
            meter.SetColor(color);
        }

        // Private 메서드
        // Others

    } // Scope by class GameMgr
} // namespace Root
