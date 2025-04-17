using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class AilmentStorage
    {
        public float endTime;
        public AilmentBase instance;
    }

    public class AilmentAffectable : MonoBehaviour
    {
        // 필드 (Fields)
        private Dictionary<AilmentType, AilmentStorage> m_AilmentExecutor;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_AilmentExecutor = new Dictionary<AilmentType, AilmentStorage>();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        // Public 메서드
        public void Execute(AilmentType type, float duration, GameObject caster)
        {
            AilmentBase ailmentInstance = null;
            if (!m_AilmentExecutor.ContainsKey(type))
            {
                ailmentInstance = AilmentMgr.GetAilmentInstance(type);
                ailmentInstance.Duration = duration;
                ailmentInstance.Caster = caster;
                ailmentInstance.Type = type;
                AilmentStorage storage = SetStorage(ailmentInstance);
                m_AilmentExecutor.Add(ailmentInstance.Type, storage);
                StartCoroutine(CoExecute(storage));
            }
            else if (m_AilmentExecutor[type].instance.IsStackable)
            {
                ailmentInstance = m_AilmentExecutor[type].instance;
                int nextCount = ailmentInstance.CurrentStackCount + 1;
                if (nextCount < ailmentInstance.MaxStackCount)
                {
                    ailmentInstance.CurrentStackCount = nextCount;
                }
            }
            else if (m_AilmentExecutor[type].instance.IsRefreshable)
            {
                ailmentInstance = m_AilmentExecutor[type].instance;
                ailmentInstance.Duration = duration;
                m_AilmentExecutor[ailmentInstance.Type].endTime = Time.time + duration;
            }
        }

        // Private 메서드
        private AilmentStorage SetStorage(AilmentBase ailmentInstance)
        {
            AilmentStorage storage = new AilmentStorage();
            storage.instance = ailmentInstance;
            storage.endTime = Time.time + ailmentInstance.Duration;
            return storage;
        }

        private IEnumerator CoExecute(AilmentStorage storage)
        {
            var currInstance = storage.instance;
            var currType = storage.instance.Type;
            currInstance.OnEnter(gameObject);

            while (Time.time < storage.endTime)
            {
                currInstance.OnStay();
                yield return new WaitForSeconds(storage.instance.Tick);

                if (storage.instance == null)
                    yield break;

                if (storage.instance.Caster == null)
                    yield break;
            }

            storage.instance.OnExit();

            yield return new WaitForSeconds(storage.instance.ImmuneTime);
            Destroy(storage.instance);
            m_AilmentExecutor.Remove(currType);
        }

        // Others

    } // Scope by class AilmentAffectable
} // namespace SkyDragonHunter