using SkyDragonHunter.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers
{
    [DefaultExecutionOrder(-98)] // ���� ���� ����ǵ��� ��
    public static class EffectMgr
    {
        // �ʵ� (Fields)
        public static GameObject s_SampleExplosionEffect;
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
            if (scene.name == "GameScene" || scene.name == "DungeonScene")
            {
                Debug.Log($"[DrawableMgr] �� �ε��: {scene.name}");
                Debug.Log($"[DrawableMgr] UIDamageMeter Prefab ������");
                s_SampleExplosionEffect = Resources.Load<GameObject>("Prefabs/SampleExplosionEffect");
            }
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            if (scene.name == "GameScene" || scene.name == "DungeonScene")
            {
                Debug.Log($"[DrawableMgr] UIDamageMeter Prefab ������");
                s_SampleExplosionEffect = null;
                Debug.Log($"[DrawableMgr] �� ��ε��: {scene.name}");
            }
        }

        public static void Play(string effectName, Vector2 position, float duration)
        { 
            var effectGo = ResourcesMgr.Load<GameObject>(effectName);
            GameObject effect = GameObject.Instantiate(effectGo);
            effect.transform.position = position;
            GameObject.Destroy(effect, duration);
        }

        public static void Play(string effectName, Vector2 position, Vector2 scale, float duration)
        {
            var effectGo = ResourcesMgr.Load<GameObject>(effectName);
            GameObject effect = GameObject.Instantiate(effectGo);
            effect.transform.position = position;
            effect.transform.localScale = scale;
            GameObject.Destroy(effect, duration);
        }

        public static void Play(string effectName, GameObject target, float duration)
        {
            var effectGo = ResourcesMgr.Load<GameObject>(effectName);
            GameObject effect = GameObject.Instantiate(effectGo);

            effect.transform.SetParent(target.transform);
            effect.transform.localPosition = new Vector3(0f, 0f, -1f);
            effect.transform.localScale = effect.transform.localScale * 2f;

            GameObject.Destroy(effect, duration);
        }

        public static void SampleExplosionPlay(Vector2 position, float radius, float duration)
        {
            GameObject effect = GameObject.Instantiate(s_SampleExplosionEffect);
            effect.transform.position = position;
            effect.GetComponent<SampleExplosionEffect>().radius = radius;
            GameObject.Destroy(effect, duration);
        }

        // Private �޼���
        // Others

    } // Scope by class GameMgr
} // namespace Root
