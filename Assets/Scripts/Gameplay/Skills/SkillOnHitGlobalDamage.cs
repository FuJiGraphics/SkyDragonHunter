using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class SkillOnHitGlobalDamage : MonoBehaviour
        , ISkillLifecycleHandler
    {
        // 필드 (Fields)
        private GameObject m_Defender = null;

        // 속성 (Properties)
        public bool IsSingleAttack { get; private set; } = false;
        public bool IsAttackEnd { get; private set; } = false;

        // 외부 종속성 필드 (External dependencies field)
        private SkillBase m_SkillBase;
        private SkillDefinition m_SkillData;
        private List<Coroutine> m_AttackCoroutines;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        // Public 메서드
        public void OnSkillCast(GameObject caster)
        {
            AdjustColliderToCamera();
        }

        public void OnSkillEnd(GameObject caster)
        {
            StopAllCoroutines();
            m_AttackCoroutines.Clear();
        }

        public void OnSkillHitEnter(GameObject defender)
        {
            var coroutine = StartCoroutine(CoAttack(defender));
            m_AttackCoroutines.Add(coroutine);
        }

        public void OnSkillHitExit(GameObject defender) { }
        public void OnSkillHitStay(GameObject defender) { }
        public void OnSkillHitBefore(GameObject caster) { }
        public void OnSkillHitAfter(GameObject caster) { }

        // Private 메서드
        private void Init()
        {
            StopAllCoroutines();
            m_SkillBase = GetComponent<SkillBase>();
            m_SkillData = m_SkillBase.SkillData;
            m_AttackCoroutines = new();
            IsSingleAttack = (m_SkillData.skillHitCount == 1);
        }

        private IEnumerator CoAttack(GameObject defender)
        {
            CharacterStatus aStat = m_SkillBase.Caster.GetComponent<CharacterStatus>();
            CharacterStatus bStat = defender.GetComponent<CharacterStatus>();
            if (aStat == null || bStat == null)
            {
                yield break;
            }

            m_Defender = defender;
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

        private void AdjustColliderToCamera()
        {
            Camera cam = Camera.main;
            if (cam == null)
                return;

            BoxCollider2D box = GetComponent<BoxCollider2D>();
            if (box == null)
                return;

            float height = 2f * cam.orthographicSize;
            float width = height * cam.aspect;

            Vector3 cameraCenter = cam.transform.position;
            transform.position = new Vector3(cameraCenter.x, cameraCenter.y, transform.position.z);

            box.size = new Vector2(width, height);
        }

        // Others

    } // Scope by class SkillOnHitGlobalDamage
} // namespace SkyDragonHunter