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
            s_UIDamageMeterPrefab = Resources.Load<GameObject>("Prefabs/UIDamageMeter");
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            Debug.Log($"[DrawableMgr] UIDamageMeter Prefab ������");
            s_UIDamageMeterPrefab = null;
            Debug.Log($"[DrawableMgr] �� ��ε��: {scene.name}");
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
