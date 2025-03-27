using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.UI;
using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {
    public class AilmentAffectable : MonoBehaviour
        , IAilmentAffectable
    {
        // 필드 (Fields)
        private Coroutine m_BurnCoroutine;
        private WaitForSeconds m_BurningStride;

        private bool m_IsFrozen;
        private float m_LastFrozenImmunityDuration = 0f;
        private Queue<float> m_CashingAnimatorSpeeds;

        private bool m_IsSlow;
        private float m_LastSlowImmunityDuration = 0f;

        // 테스트 코드
        private SpriteRenderer m_CashingSprite; // TODO: 테스트용
        private Color m_CashingSpriteColor; // TODO: 테스트용

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        private DamageReceiver m_DamageReceiver;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        // Public 메서드
        // Private 메서드
        private void Init()
        {
            m_DamageReceiver = GetComponent<DamageReceiver>();
            m_BurnCoroutine = null;
            m_BurningStride = new WaitForSeconds(1f);
        }

        private IEnumerator CoBurning(BurnStatusAilment status)
        {
            float endTime = Time.time + status.duration;
            while (Time.time < endTime)
            {
                m_DamageReceiver.TakeDamage(status.attacker, status.damagePerSeconds);
                DrawableMgr.Text(transform.position, status.damagePerSeconds.ToString(), Color.green);
                yield return m_BurningStride;
            }
            m_BurnCoroutine = null;
            m_CashingSprite.color = m_CashingSpriteColor;
        }

        private void TestRunAffectEffect(Color color)
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            m_CashingSprite = sprite;
            if (sprite != null)
            {
                m_CashingSpriteColor = sprite.color;
                sprite.color = color;
            }
        }

        private IEnumerator CoFreeze(float duration)
        {
            // 비활성화할 대상들 가져오기
            MonoBehaviour[] componentsToDisable = GetComponents<MonoBehaviour>();

            // 이 스크립트 제외하고 비활성화
            foreach (var comp in componentsToDisable)
            {
                if (comp != this)
                    comp.enabled = false;
            }

            m_CashingAnimatorSpeeds = new Queue<float>();
            Animator[] animators = GetComponentsInChildren<Animator>();
            foreach (var animator in animators)
            {
                if (animator != null)
                {
                    animator.speed = 0f;
                    m_CashingAnimatorSpeeds.Enqueue(animator.speed);
                }
            }

            yield return new WaitForSeconds(duration);

            foreach (var comp in componentsToDisable)
            {
                if (comp != this)
                    comp.enabled = true;
            }

            foreach (var animator in animators)
            {
                if (animator != null)
                {
                    animator.speed = m_CashingAnimatorSpeeds.Dequeue();
                }
            }

            m_CashingAnimatorSpeeds = null;
            m_IsFrozen = false;
            m_LastFrozenImmunityDuration = 0f;
            m_CashingSprite.color = m_CashingSpriteColor;
        }

        private IEnumerator CoSlow(float duration, float slowMultiplier)
        {
            var slowables = GetComponentsInChildren<ISlowable>();
            foreach (var slowable in slowables)
            {
                slowable.OnSlowBegin(slowMultiplier);
            }
            yield return new WaitForSeconds(duration);
            foreach (var slowable in slowables)
            {
                slowable.OnSlowEnd(slowMultiplier);
            }
            m_IsSlow = false;
            m_LastSlowImmunityDuration = 0f;
            m_CashingSprite.color = m_CashingSpriteColor;
        }

        // Others
        public void OnBurn(BurnStatusAilment status)
        {
            if (m_BurnCoroutine != null)
                StopCoroutine(m_BurnCoroutine);

            DrawableMgr.Text(transform.position, "Burn!!!!!", Color.red);
            TestRunAffectEffect(Color.red); // TODO: 테스트용
            m_BurnCoroutine = StartCoroutine(CoBurning(status));
        }

        public void OnFreeze(FreezeStatusAilment status)
        {
            if (m_IsFrozen || Time.time < m_LastFrozenImmunityDuration)
                return;

            m_IsFrozen = true;
            m_LastFrozenImmunityDuration = Time.time + status.duration + status.immunityMultiplier;

            DrawableMgr.Text(transform.position, "Freeze!!!!!", Color.blue);
            TestRunAffectEffect(Color.blue); // TODO: 테스트용
            StartCoroutine(CoFreeze(status.duration));
        }

        public void OnSlow(SlowStatusAilment status)
        {
            if (m_IsSlow || Time.time < m_LastFrozenImmunityDuration)
                return;

            m_IsFrozen = true;
            m_LastFrozenImmunityDuration = Time.time + status.duration + status.immunityMultiplier;

            DrawableMgr.Text(transform.position, "Slow!!!!!", Color.yellow);
            TestRunAffectEffect(Color.yellow); // TODO: 테스트용
            StartCoroutine(CoSlow(status.duration, status.slowMultiplier));
        }


    } // Scope by class AilmentAffectable
} // namespace SkyDragonHunter