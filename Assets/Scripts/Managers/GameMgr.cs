using System.Collections.Generic;
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
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!Application.isPlaying)
                return;
            Debug.Log($"[GameMgr] 씬 로드됨: {scene.name}");
            Application.targetFrameRate = 60;
            m_LoadObjects = new Dictionary<string, List<GameObject>>();
            AccountMgr.Init();
            GameMgr.LoadedRegisterObjects();
            AccountMgr.LoadUserData(scene.name);
            AccountMgr.LateInit();
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
            if (m_LoadObjects.TryGetValue(id, out var list) && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public static T FindObject<T>(string id) where T : Component
        {
            var go = FindObject(id);
            return go != null ? go.GetComponent<T>() : null;
        }

        public static GameObject[] FindObjects(string id)
        {
            if (m_LoadObjects.TryGetValue(id, out var list))
            {
                return list.ToArray();
            }
            return null;
        }

        public static T[] FindObjects<T>(string id) where T : Component
        {
            var gos = FindObjects(id);
            if (gos == null)
                return null;

            List<T> result = new List<T>();
            foreach (var go in gos)
            {
                var comp = go.GetComponent<T>();
                if (comp != null)
                    result.Add(comp);
            }

            return result.ToArray();
        }

        public static T[] FindObjects<T>()
        {
            List<T> result = new List<T>();
            foreach (var go in m_LoadObjects)
            {
                var list = go.Value;

                foreach (var findGo in list)
                {
                    if (findGo.TryGetComponent<T>(out var comp))
                    {
                        result.Add(comp);
                    }
                }
            }
            return result.ToArray();
        }

        // Private 메서드
        private static void LoadedRegisterObjects()
        {
            RegisterObject[] components = Object.FindObjectsOfType<RegisterObject>(true);
            foreach (RegisterObject comp in components)
            {
                RegisterObject(comp.id, comp.gameObject);
                Debug.Log($"게임 매니저에 등록됨: {comp.id}");
            }
            foreach (RegisterObject comp in components)
            {
                comp.onInitEvents.Invoke();
                Debug.Log($"초기화 이벤트 호출: {comp.id}");
            }
        }

        // Others

    } // Scope by class GameMgr
} // namespace Root
