using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter {
    public class SkillOnHitDamage : MonoBehaviour
        , ISkillLifecycleHandler
    {
        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        private SkillBase m_SkillBase;
        private SkillDefinition m_SkillData;
        private GameObject m_Caster;

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
            CharacterStatus aStat = m_Caster.GetComponent<CharacterStatus>();
            CharacterStatus bStat = m_Caster.GetComponent<CharacterStatus>();
            if (aStat == null || bStat == null)
                return;

            Attack attack = m_SkillData.CreateAttack(aStat, bStat);
            IAttackable attackable = bStat.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.OnAttack(m_Caster, attack);
            }
        }

        public void OnSkillHitExit(GameObject defender) {}
        public void OnSkillHitStay(GameObject defender) {}
        public void OnSkillHitBefore(GameObject caster) {}
        public void OnSkillHitAfter(GameObject caster) {}

        // Private 메서드
        private void Init()
        {
            m_SkillBase = GetComponent<SkillBase>();
            m_SkillData = m_SkillBase.skillData;
            m_Caster = m_SkillBase.Caster;
        }

        // Others

    } // Scope by class SkillOnHitDamage
} // namespace SkyDragonHunter