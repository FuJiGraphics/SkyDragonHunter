using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class SkillOnHitAilment : MonoBehaviour
        , ISkillLifecycleHandler
    {
        // 필드 (Fields)
        [SerializeField] private bool m_AllowMultiHit = false;
        public AilmentType[] ailments;

        private SkillBase m_SkillBase;
        private bool m_IsNotFirstEnter = false;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        // Public 메서드
        public void OnSkillCast(GameObject caster)
        {

        }
        public void OnSkillEnd(GameObject caster) {}

        public void OnSkillHitEnter(GameObject defender)
        {
            if (!m_AllowMultiHit && m_IsNotFirstEnter)
                return;

            m_IsNotFirstEnter = true;
            ApplyStatusAilment(defender);
        }

        public void OnSkillHitExit(GameObject defender) {}
        public void OnSkillHitStay(GameObject defender) { }
        public void OnSkillHitBefore(GameObject caster) { }
        public void OnSkillHitAfter(GameObject caster) { }

        // Private 메서드
        private void Init()
        {
            m_SkillBase = GetComponent<SkillBase>();
        }

        private void ApplyStatusAilment(GameObject target)
        {
            foreach (var type in ailments)
            {
                if (target.TryGetComponent<AilmentAffectable>(out var affactable))
                {
                    float duration = m_SkillBase.SkillData.ailmentDuration;
                    affactable.Execute(type, duration, m_SkillBase.Caster);
                }
            }
        }

        // Others

    } // Scope by class SkillOnHitAilment
} // namespace SkyDragonHunter