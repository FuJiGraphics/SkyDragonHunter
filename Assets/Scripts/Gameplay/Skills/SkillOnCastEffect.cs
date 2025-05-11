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
        [SerializeField] private float m_EffectDuration = 1f;
        [SerializeField] private Vector2 m_Position = Vector2.zero;
        [SerializeField] private Vector2 m_Scale = Vector2.one;

        private SkillBase m_SkillBase;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_SkillBase = GetComponent<SkillBase>();
        }

        // Public 메서드
        public void OnCastEffect(GameObject caster)
        {
            CastEffectToCaster(caster);
            CastEffectToAllCrews();
            CastEffectToGlobal();
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
                var effectInstance = EffectMgr.Play(effectName, caster, m_Position, m_Scale, m_EffectDuration);
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
                    var effectInstance = EffectMgr.Play(effectName, crew, m_Position, m_Scale, m_EffectDuration);
                }
            }
        }

        private void CastEffectToGlobal()
        {
            if (m_SkillBase.SkillData.buffTarget != Scriptables.BuffTarget.None)
                return;

            string effectName = m_SkillBase.SkillData.skillEffect;
            if (!string.IsNullOrEmpty(effectName))
            {
                var effectInstance = EffectMgr.Play(effectName, m_Position, m_Scale, m_EffectDuration);
            }
        }

        // Others

    } // Scope by class SkillOnCastEffect
} // namespace SkyDragonHunter