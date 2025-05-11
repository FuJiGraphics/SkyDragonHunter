using SkyDragonHunter.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers
{
    [DefaultExecutionOrder(-98)] // 가장 먼저 실행되도록 함
    public static class EffectMgr
    {
        // 필드 (Fields)
        public static GameObject s_SampleExplosionEffect;
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
            if (scene.name == "GameScene" || scene.name == "DungeonScene")
            {
                Debug.Log($"[DrawableMgr] 씬 로드됨: {scene.name}");
                Debug.Log($"[DrawableMgr] UIDamageMeter Prefab 생성중");
                s_SampleExplosionEffect = Resources.Load<GameObject>("Prefabs/SampleExplosionEffect");
            }
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            if (scene.name == "GameScene" || scene.name == "DungeonScene")
            {
                Debug.Log($"[DrawableMgr] UIDamageMeter Prefab 정리중");
                s_SampleExplosionEffect = null;
                Debug.Log($"[DrawableMgr] 씬 언로드됨: {scene.name}");
            }
        }

        public static GameObject Play(string effectName, Vector2 position, float duration)
        { 
            var effectGo = ResourcesMgr.Load<GameObject>(effectName);
            GameObject effect = GameObject.Instantiate(effectGo);
            effect.transform.position = position;
            GameObject.Destroy(effect, duration);
            return effect;
        }

        public static GameObject Play(string effectName, Vector2 position, Vector2 scale, float duration)
        {
            var effectGo = ResourcesMgr.Load<GameObject>(effectName);
            GameObject effect = GameObject.Instantiate(effectGo);

            effect.transform.position = position;
            effect.transform.localScale = scale;

            GameObject.Destroy(effect, duration);
            return effect;
        }

        public static GameObject Play(string effectName, GameObject target, Vector2 offset, Vector2 scale, float duration)
        {
            var effectGo = ResourcesMgr.Load<GameObject>(effectName);
            GameObject effect = GameObject.Instantiate(effectGo);

            effect.transform.SetParent(target.transform);
            effect.transform.localPosition = new Vector3(offset.x, offset.y, 0f);
            effect.transform.localScale = effect.transform.localScale * scale;

            GameObject.Destroy(effect, duration);
            return effect;
        }

        public static void SampleExplosionPlay(Vector2 position, float radius, float duration)
        {
            GameObject effect = GameObject.Instantiate(s_SampleExplosionEffect);
            effect.transform.position = position;
            effect.GetComponent<SampleExplosionEffect>().radius = radius;
            GameObject.Destroy(effect, duration);
        }

        // Private 메서드
        // Others

    } // Scope by class GameMgr
} // namespace Root
