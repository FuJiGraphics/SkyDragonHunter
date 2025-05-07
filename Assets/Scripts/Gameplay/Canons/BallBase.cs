using NPOI.SS.Formula.Functions;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class BallBase : MonoBehaviour
    {
        // 필드 (Fields)
        // 속성 (Properties)
        public GameObject Caster { get; set; }
        public GameObject Receiver { get; set; }
        public CanonDefinition CanonData { get; set; }

        // 외부 종속성 필드 (External dependencies field)
        private IBallLifecycleHandler[] m_HitHandlers;
        private IBallEffectLifecycleHandler[] m_EffectHandlers;
        private IAttackTargetProvider m_AttackTargetSelector;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_HitHandlers = GetComponents<IBallLifecycleHandler>();
            m_EffectHandlers = GetComponents<IBallEffectLifecycleHandler>();
            m_AttackTargetSelector = GetComponent<IAttackTargetProvider>();
            if (m_AttackTargetSelector == null)
            {
                Debug.LogWarning("[CanonBase]: AttackTargetSelector를 찾을 수 없습니다.");
            }
        }

        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            Release();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!m_AttackTargetSelector.IsAllowedTarget(collision.gameObject.tag))
                return;

            OnHitBefore();
            OnHitEnter(collision.gameObject);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!m_AttackTargetSelector.IsAllowedTarget(collision.gameObject.tag))
                return;

            OnHitStay(collision.gameObject);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!m_AttackTargetSelector.IsAllowedTarget(collision.gameObject.tag))
                return;

            OnHitExit(collision.gameObject);
            OnHitAfter();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!m_AttackTargetSelector.IsAllowedTarget(collider.tag))
                return;

            OnHitBefore();
            OnHitEnter(collider.gameObject);
            OnHitEnterEffect(Caster, collider.gameObject);
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (!m_AttackTargetSelector.IsAllowedTarget(collider.tag))
                return;

            OnHitStay(collider.gameObject);
            OnHitStayEffect(Caster, collider.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (!m_AttackTargetSelector.IsAllowedTarget(collider.tag))
                return;

            OnHitExit(collider.gameObject);
            OnHitAfter();
        }

        // Public 메서드
        // Private 메서드
        private void Init()
        {
            if (m_HitHandlers == null)
                return;

            foreach (var handler in m_HitHandlers)
            {
                handler.OnStart(Caster);
            }
            foreach (var handler in m_EffectHandlers)
            {
                handler.OnCastEffect(Caster);
            }
        }

        private void OnHitBefore()
        {
            if (m_HitHandlers == null)
                return;

            foreach (var handler in m_HitHandlers)
            {
                handler.OnHitBefore(Caster);
            }
        }

        private void OnHitEnter(GameObject defender)
        {
            if (m_HitHandlers == null)
                return;

            foreach (var handler in m_HitHandlers)
            {
                handler.OnHitEnter(defender);
            }
        }

        private void OnHitEnterEffect(GameObject attacker, GameObject defender)
        {
            if (m_EffectHandlers == null)
                return;

            foreach (var handler in m_EffectHandlers)
            {
                handler.OnHitEnterEffect(attacker, defender);
            }
        }

        private void OnHitStay(GameObject defender)
        {
            if (m_HitHandlers == null)
                return;

            foreach (var handler in m_HitHandlers)
            {
                handler.OnHitStay(defender);
            }
        }

        private void OnHitStayEffect(GameObject attacker, GameObject defender)
        {
            if (m_EffectHandlers == null)
                return;

            foreach (var handler in m_EffectHandlers)
            {
                handler.OnHitStayEffect(attacker, defender);
            }
        }

        private void OnHitExit(GameObject defender)
        {
            if (m_HitHandlers == null)
                return;

            foreach (var handler in m_HitHandlers)
            {
                handler.OnHitExit(defender);
            }
        }

        private void OnHitAfter()
        {
            if (m_HitHandlers == null)
                return;

            foreach (var handler in m_HitHandlers)
            {
                handler.OnHitAfter(Caster);
            }
        }

        private void Release()
        {
            if (m_HitHandlers == null)
                return;

            foreach (var handler in m_HitHandlers)
            {
                handler.OnEnd(Caster);
            }
        }

        private bool IsNullProperties()
            => (Caster == null || Receiver == null || CanonData == null);

        // Others

    } // Scope by class BallBase
} // namespace SkyDragonHunter.Gameplay