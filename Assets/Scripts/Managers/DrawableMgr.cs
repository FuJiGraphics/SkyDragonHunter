using SkyDragonHunter.UI;
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
        public static GameObject s_UIAlertDialogPrefab;

        public static GameObject s_PrevGenDialogInstance;

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
            s_UIDamageMeterPrefab = ResourcesMgr.Load<GameObject>("Prefabs/UI/UIDamageMeter");
            s_UIAlertDialogPrefab = ResourcesMgr.Load<GameObject>("Prefabs/UI/UIAlertDialog");
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            Debug.Log($"[DrawableMgr] UIDamageMeter Prefab 정리중");
            s_UIDamageMeterPrefab = null;
            Debug.Log($"[DrawableMgr] 씬 언로드됨: {scene.name}");
        }

        public static void Dialog(string title, string text)
        {
            if (s_PrevGenDialogInstance != null)
                GameObject.Destroy(s_PrevGenDialogInstance);

            s_PrevGenDialogInstance = GameObject.Instantiate(s_UIAlertDialogPrefab);
            if (s_PrevGenDialogInstance.TryGetComponent<UIAlertDialog>(out var dialogComp))
            {
                dialogComp.Title = title;
                dialogComp.Text = text;
            }
            else
            {
                Debug.LogError($"[DrawableMgr]: {s_PrevGenDialogInstance}에서 UIAlertDialog를 찾을 수 없습니다.");
            }
        }

        public static void Dialog(string title, string text, float destroyTime)
        {
            Dialog(title, text);
            GameObject.Destroy(s_PrevGenDialogInstance, destroyTime);
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
