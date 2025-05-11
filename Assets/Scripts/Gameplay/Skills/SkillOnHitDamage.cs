using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using System.Collections;
using UnityEngine;

namespace SkyDragonHunter {
    public class SkillOnHitDamage : MonoBehaviour
        , ISkillLifecycleHandler
    {
        // 필드 (Fields)
        private GameObject m_Defender = null;
        private bool m_IsAttackStart = false;

        // 속성 (Properties)
        public bool IsSingleAttack { get; private set; } = false;
        public bool IsAttackEnd { get; private set; } = false;
 
        // 외부 종속성 필드 (External dependencies field)
        private SkillBase m_SkillBase;
        private SkillDefinition m_SkillData;
        private Coroutine m_AttackCoroutine;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            // 지속적으로 공격하기 위해서 상대 위치로 포지션 잡음
            if (m_IsAttackStart && m_Defender != null) 
            {
                transform.position = m_Defender.transform.position;
            }
        }

        // Public 메서드
        public void OnSkillCast(GameObject caster)
        {
        }

        public void OnSkillEnd(GameObject caster)
        {
            StopAllCoroutines();
            m_AttackCoroutine = null;
        }

        public void OnSkillHitEnter(GameObject defender)
        {
            if (m_IsAttackStart)
                return;

            if (m_AttackCoroutine == null && defender != null)
            {
                m_AttackCoroutine = StartCoroutine(CoAttack(defender));
            }
        }

        public void OnSkillHitExit(GameObject defender) {}
        public void OnSkillHitStay(GameObject defender) {}
        public void OnSkillHitBefore(GameObject caster) {}
        public void OnSkillHitAfter(GameObject caster) {}

        // Private 메서드
        private void Init()
        {
            StopAllCoroutines();
            m_SkillBase = GetComponent<SkillBase>();
            m_SkillData = m_SkillBase.SkillData;
            m_AttackCoroutine = null;
            IsSingleAttack = (m_SkillData.skillHitCount == 1);
        }

        private IEnumerator CoAttack(GameObject defender)
        {
            CharacterStatus aStat = m_SkillBase.Caster.GetComponent<CharacterStatus>();
            CharacterStatus bStat = defender.GetComponent<CharacterStatus>();
            if (aStat == null || bStat == null)
            {
                m_AttackCoroutine = null;
                yield break;
            }

            m_Defender = defender;
            m_IsAttackStart = true;
            StopMovementStates();

            for (int i = 0; i < m_SkillData.skillHitCount; ++i)
            {
                if (m_Defender == null)
                    break;

                Attack attack = m_SkillData.CreateAttack(aStat, bStat);
                if (bStat.TryGetComponent<IAttackable>(out var attackable))
                {
                    attackable.OnAttack(m_SkillBase.Caster, attack);
                }

                yield return new WaitForSeconds(m_SkillData.skillHitDuration);
            }

            m_AttackCoroutine = null;
            IsAttackEnd = true;
        }

        private void StopMovementStates()
        {
            var sprite = GetComponentInChildren<SpriteRenderer>(true);
            if (sprite != null)
            {
                sprite.enabled = false;
            }

            if (TryGetComponent<SkillMovementLinear>(out var linear))
            {
                linear.enabled = false;
            }
        }

        // Others

    } // Scope by class SkillOnHitDamage
} // namespace SkyDragonHunter