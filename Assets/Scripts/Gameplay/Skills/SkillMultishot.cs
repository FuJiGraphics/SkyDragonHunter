using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace SkyDragonHunter.Gameplay
{
    public class SkillMultishot : MonoBehaviour
        , ISkillLifecycleHandler
    {
        // 필드 (Fields)
        [Header("Shoot Skill Settings")]
        [SerializeField] private int m_ShootCount;
        [SerializeField] private SkillBase m_ShootSkillPrefab;
        [SerializeField] private Vector2 m_FirePositionOffset;
        [SerializeField] private string m_TargetTag;
        [SerializeField] private float m_Distance;

        [Header("Shoot Effect Settings")]
        [SerializeField] private string m_ShootEffectName;
        [SerializeField] private Vector2 m_ShootEffectPositionOffset;
        [SerializeField] private Vector2 m_ShootEffectScaleOffset = Vector2.one;
        [SerializeField] private float m_ShootEffectDuration;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        private SkillBase m_SkillBase;
        private Coroutine m_Coroutine;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        private void OnDisable()
        {
            m_Coroutine = null;
            StopAllCoroutines();
        }

        // Public 메서드
        public void OnSkillCast(GameObject caster)
        {
            m_ShootCount = Math.Max(m_ShootCount, 1);


            if (m_Coroutine == null)
            {
                m_Coroutine = StartCoroutine(CoShoot());
            }
        }

        public void OnSkillEnd(GameObject caster) { }
        public void OnSkillHitEnter(GameObject defender) { }
        public void OnSkillHitExit(GameObject defender) { }
        public void OnSkillHitStay(GameObject defender) { }
        public void OnSkillHitBefore(GameObject caster) { }
        public void OnSkillHitAfter(GameObject caster) { }

        // Private 메서드
        private void Init()
        {
            StopAllCoroutines();
            m_SkillBase = GetComponent<SkillBase>();
        }

        private void FireSkill()
        {
            var target = GameMgr.FindFarthestTarget(m_TargetTag, transform.position, m_Distance);
            if (target == null)
                return;

            ApplyShootEffect();

            SkillBase skill = Instantiate(m_ShootSkillPrefab);
            skill.Init(m_SkillBase.Caster, target);

            if (skill.TryGetComponent<AttackTargetSelector>(out var selector))
            {
                selector.SetAllowedTarget(m_SkillBase.AttackTargetTags);
            }

            Vector3 firePos = skill.Caster.transform.position;
            skill.transform.position = firePos + new Vector3(m_FirePositionOffset.x, m_FirePositionOffset.y, 0f);

            if (skill.TryGetComponent<IDirectional>(out var dir))
            {
                Vector2 aPos = firePos;
                Vector2 dPos = skill.Receiver.transform.position;
                dir.SetDirection(dPos - aPos);
            }
        }

        private void ApplyShootEffect()
        {
            if (!string.IsNullOrEmpty(m_ShootEffectName))
            {
                Vector2 newPos = m_SkillBase.Caster.transform.position;
                newPos += m_ShootEffectPositionOffset;
               EffectMgr.Play(m_ShootEffectName, newPos, m_ShootEffectScaleOffset, m_ShootEffectDuration);
            }
        }

        private IEnumerator CoShoot()
        {
            for (int i = 0; i < m_ShootCount; ++i)
            {
                FireSkill();
                yield return new WaitForSeconds(m_SkillBase.SkillData.skillHitDuration);
            }
            m_Coroutine = null;
        }

        // Others

    } // Scope by class SkillOnHitDamage
} // namespace SkyDragonHunter