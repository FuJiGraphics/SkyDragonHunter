using SkyDragonHunter.Gameplay;
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
        private static GameObject s_UIDamageMeterPrefab;
        private static GameObject s_UIAlertDialogPrefab;
        private static GameObject s_UIAlertArtifactInfoPrefab;
        private static GameObject s_PrevGenDialogInstance;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // Public �޼���
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            //Debug.Log("DrawableMgr Init");
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log($"[DrawableMgr] �� �ε��: {scene.name}");
            //Debug.Log($"[DrawableMgr] UIDamageMeter Prefab ������");
            s_UIDamageMeterPrefab = ResourcesMgr.Load<GameObject>("Assets/Prefabs/UI/UIDamageMeter.prefab");
            s_UIAlertDialogPrefab = ResourcesMgr.Load<GameObject>("Assets/Prefabs/UI/UIAlertDialog.prefab");
            s_UIAlertArtifactInfoPrefab = ResourcesMgr.Load<GameObject>("Assets/Prefabs/UI/UIAlertArtifactInfo.prefab");
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            //Debug.Log($"[DrawableMgr] UIDamageMeter Prefab ������");
            s_UIDamageMeterPrefab = null;
            //Debug.Log($"[DrawableMgr] �� ��ε��: {scene.name}");
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
            var uiSetting = GameMgr.FindObject<UISettingPanel>("UISettingPanel");
            if (uiSetting != null && !uiSetting.IsDrawDamage)
                return;

            GameObject damageMeterUI = GameObject.Instantiate(s_UIDamageMeterPrefab);
            damageMeterUI.transform.position = position;
            damageMeterUI.GetComponent<UIDamageMeter>().SetText(str, Color.white);
            GameObject.Destroy(damageMeterUI, 1f);
        }

        public static void Text(Vector2 position, string str, Color color)
        {
            var uiSetting = GameMgr.FindObject<UISettingPanel>("UISettingPanel");
            if (uiSetting != null && !uiSetting.IsDrawDamage)
                return;

            GameObject damageMeterUI = GameObject.Instantiate(s_UIDamageMeterPrefab);
            damageMeterUI.transform.position = position;
            var meter = damageMeterUI.GetComponent<UIDamageMeter>();
            meter.SetText(str, color);
            GameObject.Destroy(damageMeterUI, 1f);
        }

        public static void TopText(Vector2 position, string str, Color color)
        {
            var uiSetting = GameMgr.FindObject<UISettingPanel>("UISettingPanel");
            if (uiSetting != null && !uiSetting.IsDrawDamage)
                return;

            GameObject damageMeterUI = GameObject.Instantiate(s_UIDamageMeterPrefab);
            damageMeterUI.transform.position = position;
            var meter = damageMeterUI.GetComponent<UIDamageMeter>();
            meter.SetTopText(str, color);
            GameObject.Destroy(damageMeterUI, 1f);
        }

        public static void DialogWithArtifactInfo(string title, ArtifactDummy artifact)
        {
            if (s_PrevGenDialogInstance != null)
                GameObject.Destroy(s_PrevGenDialogInstance);

            s_PrevGenDialogInstance = GameObject.Instantiate(s_UIAlertArtifactInfoPrefab);
            s_PrevGenDialogInstance?.SetActive(true);
            if (s_PrevGenDialogInstance.TryGetComponent<UIAlertArtifactInfo>(out var dialogComp))
            {
                dialogComp.Title = title;
                dialogComp.ShowInfo(artifact);
            }
            else
            {
                Debug.LogError($"[DrawableMgr]: {s_PrevGenDialogInstance}���� UIAlertDialog�� ã�� �� �����ϴ�.");
            }

            GameObject.Destroy(s_PrevGenDialogInstance, 8f);
        }

        // Private �޼���
        // Others

    } // Scope by class GameMgr
} // namespace Root
