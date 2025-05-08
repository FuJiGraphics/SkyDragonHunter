using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class SkillOnCastEffect : MonoBehaviour
        , ISkillEffectLifecycleHandler
    {
        // 필드 (Fields)
        [SerializeField] private bool m_MatchBoxColliderSize = false;

        private SkillBase m_SkillBase;
        private GameObject m_CacheEffectInstance;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_SkillBase = GetComponent<SkillBase>();
        }

        private void OnDisable()
        {
            Destroy(m_CacheEffectInstance);
        }

        // Public 메서드
        public void OnCastEffect(GameObject caster)
        {
            string effectName = m_SkillBase.SkillData.skillEffect;
            if (!string.IsNullOrEmpty(effectName))
            {
                float duration = 1f;
                if (m_SkillBase.SkillType == SkillType.Affect)
                    duration = m_SkillBase.SkillData.BuffMaxDuration;
                if (m_MatchBoxColliderSize)
                    m_CacheEffectInstance = EffectMgr.Play(effectName, caster, duration);
                else
                    m_CacheEffectInstance = EffectMgr.Play(effectName, caster.transform.position, duration);
            }
        }

        public void OnHitEnterEffect(GameObject caster, GameObject receiver) { }
        public void OnHitStayEffect(GameObject caster, GameObject receiver) { }

        // Private 메서드
        // Others

    } // Scope by class SkillOnCastEffect
} // namespace SkyDragonHunter