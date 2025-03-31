using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers
{
    [DefaultExecutionOrder(-100)] // 가장 먼저 실행되도록 함
    public static class GameMgr
    {
        // 필드 (Fields)
        private static Dictionary<string, GameObject> m_LoadObjects = new Dictionary<string, GameObject>();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            Debug.Log("GameMgr Init");
            AccountMgr.Init();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"[GameMgr] 씬 로드됨: {scene.name}");
            m_LoadObjects = new Dictionary<string, GameObject>();
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            Debug.Log($"[GameMgr] Load된 Object 정리 중");
            m_LoadObjects = null;
            AccountMgr.Release();
            Debug.Log($"[GameMgr] 씬 언로드됨: {scene.name}");
        }

        public static bool RegisterObject(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("Warning: object를 등록할 수 없습니다. (null)");
                return false;
            }
            var uuidComp = obj.GetComponent<ObjectUUID>();
            if (uuidComp == null)
            {
                Debug.LogWarning($"Warning: {obj.name}의 ObjectUUID 컴포넌트를 찾을 수 없습니다.");
                return false;
            }
            if (m_LoadObjects.ContainsKey(uuidComp.uuid))
            {
                Debug.LogWarning($"Warning: 중복된 uuid가 존재합니다.{uuidComp.uuid}");
                return false;
            }

            m_LoadObjects.Add(uuidComp.uuid, obj);
            return true;
        }

        public static bool UnregisterObject(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("Warning: object를 해제할 수 없습니다. (null)");
                return false;
            }
            var uuidComp = obj.GetComponent<ObjectUUID>();
            if (uuidComp == null)
            {
                Debug.LogWarning($"Warning: {obj.name}의 ObjectUUID 컴포넌트를 찾을 수 없습니다.");
                return false;
            }
            if (!m_LoadObjects.ContainsKey(uuidComp.uuid))
            {
                return false;
            }

            m_LoadObjects.Remove(uuidComp.uuid);
            return true;
        }

        public static GameObject FindObject(string uuid)
        {
            GameObject findGo = null;
            if (m_LoadObjects.ContainsKey(uuid))
            {
                findGo = m_LoadObjects[uuid];
            }
            return findGo;
        }

        // Private 메서드
        // Others

    } // Scope by class GameMgr
} // namespace Root
