using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class SkillOnCastEffect : MonoBehaviour
        , ISkillEffectLifecycleHandler
    {
        // 필드 (Fields)
        [SerializeField] private Vector2 m_Position = Vector2.zero;
        [SerializeField] private Vector2 m_Scale = Vector2.one;

        private SkillBase m_SkillBase;
        private List<GameObject> m_CacheEffectInstancies;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_SkillBase = GetComponent<SkillBase>();
            m_CacheEffectInstancies = new List<GameObject>();
        }

        private void OnDisable()
        {
            foreach (var effectInstance in m_CacheEffectInstancies)
            {
                if (effectInstance != null && effectInstance.activeSelf)
                {
                    Destroy(effectInstance);
                }
            }
        }

        // Public 메서드
        public void OnCastEffect(GameObject caster)
        {
            CastEffectToCaster(caster);
            CastEffectToAllCrews();
        }

        public void OnHitEnterEffect(GameObject caster, GameObject receiver) { }
        public void OnHitStayEffect(GameObject caster, GameObject receiver) { }

        // Private 메서드
        private void CastEffectToCaster(GameObject caster)
        {
            if (m_SkillBase.SkillData.buffTarget != Scriptables.BuffTarget.Caster)
                return;

            string effectName = m_SkillBase.SkillData.skillEffect;
            if (!string.IsNullOrEmpty(effectName))
            {
                float duration = 1f;
                if (m_SkillBase.SkillType == SkillType.Affect)
                    duration = m_SkillBase.SkillData.BuffMaxDuration;
                var effectInstance = EffectMgr.Play(effectName, caster, m_Position, m_Scale, duration);
                m_CacheEffectInstancies.Add(effectInstance);
            }
        }

        private void CastEffectToAllCrews()
        {
            if (m_SkillBase.SkillData.buffTarget != Scriptables.BuffTarget.All)
                return;

            var crewInstancies = GameMgr.FindObjects("Crew");

            foreach (var crew in crewInstancies)
            {
                if (crew == null)
                    continue;

                string effectName = m_SkillBase.SkillData.skillEffect;
                if (!string.IsNullOrEmpty(effectName))
                {
                    float duration = 1f;
                    if (m_SkillBase.SkillType == SkillType.Affect)
                        duration = m_SkillBase.SkillData.BuffMaxDuration;
                    var effectInstance = EffectMgr.Play(effectName, crew, m_Position, m_Scale, duration);
                    m_CacheEffectInstancies.Add(effectInstance);
                }
            }
        }

        // Others

    } // Scope by class SkillOnCastEffect
} // namespace SkyDragonHunter