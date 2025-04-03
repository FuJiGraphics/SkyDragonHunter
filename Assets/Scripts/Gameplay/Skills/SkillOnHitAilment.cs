using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using UnityEngine;

namespace SkyDragonHunter {
    public class SkillOnHitAilment : MonoBehaviour
        , ISkillLifecycleHandler
    {
        // 필드 (Fields)
        public StatusAilmentDefinition[] ailments;

        private IAttackTargetProvider m_TargetProvider;
        private SkillBase m_SkillBase;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        // Public 메서드
        public void OnSkillCast(GameObject caster) {}
        public void OnSkillEnd(GameObject caster) {}

        public void OnSkillHitEnter(GameObject defender)
        {
            if (!m_TargetProvider.IsAllowedTarget(defender.tag))
                return;
            ApplyStatusAilment(defender);
        }

        public void OnSkillHitExit(GameObject defender) {}
        public void OnSkillHitStay(GameObject defender) { }
        public void OnSkillHitBefore(GameObject caster) { }
        public void OnSkillHitAfter(GameObject caster) { }

        // Private 메서드
        private void Init()
        {
            m_TargetProvider = GetComponent<IAttackTargetProvider>();
            m_SkillBase = GetComponent<SkillBase>();
        }

        private void ApplyStatusAilment(GameObject target)
        {
            foreach (var ailment in ailments)
            {
                ailment.Execute(m_SkillBase.Caster, target);
            }
        }

        // Others

    } // Scope by class SkillOnHitAilment
} // namespace SkyDragonHunter