using SkyDragonHunter.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers
{
    [DefaultExecutionOrder(-99)] // ���� ���� ����ǵ��� ��
    public static class DrawableMgr
    {
        // �ʵ� (Fields)
        public static GameObject s_UIDamageMeterPrefab;
        public static GameObject s_UIAlertDialogPrefab;

        public static GameObject s_PrevGenDialogInstance;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // Public �޼���
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            Debug.Log("DrawableMgr Init");
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"[DrawableMgr] �� �ε��: {scene.name}");
            Debug.Log($"[DrawableMgr] UIDamageMeter Prefab ������");
            s_UIDamageMeterPrefab = ResourcesMgr.Load<GameObject>("Prefabs/UIDamageMeter");
            s_UIAlertDialogPrefab = ResourcesMgr.Load<GameObject>("Prefabs/UIAlertDialog");
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            Debug.Log($"[DrawableMgr] UIDamageMeter Prefab ������");
            s_UIDamageMeterPrefab = null;
            Debug.Log($"[DrawableMgr] �� ��ε��: {scene.name}");
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
                Debug.LogError($"[DrawableMgr]: {s_PrevGenDialogInstance}���� UIAlertDialog�� ã�� �� �����ϴ�.");
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

        // Private �޼���
        // Others

    } // Scope by class GameMgr
} // namespace Root
