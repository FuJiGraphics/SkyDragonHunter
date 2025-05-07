using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {
    public class SkillOnHitExplosiveDamage : MonoBehaviour
        , ISkillLifecycleHandler
    {
        // 필드 (Fields)
        public float radius = 5f;
        private SkillBase m_SkillBase;
        private SkillDefinition m_SkillData;
        private IAttackTargetProvider m_TargetProvider;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        // Public 메서드
        public void OnSkillCast(GameObject caster) { }
        public void OnSkillEnd(GameObject caster) { }
        public void OnSkillHitAfter(GameObject caster)
        {
            EffectMgr.SampleExplosionPlay(transform.position, radius, 0.5f);
        }
        public void OnSkillHitBefore(GameObject caster) { }
        public void OnSkillHitEnter(GameObject defender)
        {
            Vector2 pos = transform.position;
            CharacterStatus aStat = m_SkillBase.Caster.GetComponent<CharacterStatus>();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius);
            foreach (Collider2D collider in colliders)
            {
                if (!m_TargetProvider.IsAllowedTarget(collider.tag))
                    continue;

                CharacterStatus bStat = collider.GetComponent<CharacterStatus>();
                if (aStat == null || bStat == null)
                    continue;

                Attack attack = m_SkillData.CreateAttack(aStat, bStat);
                IAttackable attackable = bStat.GetComponent<IAttackable>();
                if (attackable != null)
                {
                    attackable.OnAttack(m_SkillBase.Caster, attack);
                }
            }
        }
        public void OnSkillHitExit(GameObject defender) { }
        public void OnSkillHitStay(GameObject defender) { }

        // Private 메서드
        private void Init()
        {
            m_SkillBase = GetComponent<SkillBase>();
            m_SkillData = m_SkillBase.SkillData;
            m_TargetProvider = GetComponent<IAttackTargetProvider>();
        }

        // Others

    } // Scope by class SkillOnHitExplosiveDamage
} // namespace SkyDragonHunter