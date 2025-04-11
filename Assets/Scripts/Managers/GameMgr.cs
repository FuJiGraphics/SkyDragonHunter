using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers
{
    [DefaultExecutionOrder(-100)] // 가장 먼저 실행되도록 함
    public static class GameMgr
    {
        // 필드 (Fields)
        private static Dictionary<string, List<GameObject>> m_LoadObjects;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            Debug.Log("GameMgr Init");
            Application.targetFrameRate = 60;
            m_LoadObjects = new Dictionary<string, List<GameObject>>();
            AccountMgr.Init();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!Application.isPlaying)
                return;
            Debug.Log($"[GameMgr] 씬 로드됨: {scene.name}");

            GameMgr.LoadedRegisterObjects();
            AccountMgr.LoadUserData();
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            if (!Application.isPlaying)
                return;

            Debug.Log($"[GameMgr] Load된 Object 정리 중");
            m_LoadObjects.Clear();
            AccountMgr.Release();
            Debug.Log($"[GameMgr] 씬 언로드됨: {scene.name}");
        }

        public static bool RegisterObject(string id, GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("Warning: object를 등록할 수 없습니다. (null)");
                return false;
            }

            if (m_LoadObjects.ContainsKey(id))
            {
                m_LoadObjects[id].Add(obj);
            }
            else
            {
                m_LoadObjects.Add(id, new List<GameObject>());
                m_LoadObjects[id].Add(obj);
            }
            return true;
        }

        public static bool UnregisterObject(string id)
        {
            if (!m_LoadObjects.ContainsKey(id))
            {
                return false;
            }
            m_LoadObjects.Remove(id);
            return true;
        }

        public static GameObject FindObject(string id)
        {
            GameObject findGo = null;
            if (m_LoadObjects.ContainsKey(id))
            {
                findGo = m_LoadObjects[id][0];
            }
            return findGo;
        }

        public static GameObject[] FindObjects(string id)
        {
            GameObject[] findGos = null;
            if (m_LoadObjects.ContainsKey(id))
            {
                findGos = m_LoadObjects[id].ToArray();
            }
            return findGos;
        }

        // Private 메서드
        private static void LoadedRegisterObjects()
        {
            RegisterObject[] components = Object.FindObjectsOfType<RegisterObject>(true);
            foreach (RegisterObject comp in components)
            {
                RegisterObject(comp.id, comp.gameObject);
                comp.onInitEvents.Invoke();
                Debug.Log($"게임 매니저에 등록됨: {comp.id}");
            }
        }

        // Others

    } // Scope by class GameMgr
} // namespace Root
