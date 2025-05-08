using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public enum SkillType
    {
        Damage,
        Affect,     // 액티브/패시브 등
        Undefined,
    }

    public class SkillBase : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private SkillType m_SkillType;
        [SerializeField] private SkillDefinition m_SkillData;
        [SerializeField] private float m_Lifetime = 5f;

        // 속성 (Properties)
        public SkillDefinition SkillData => m_SkillData;
        public float LifeTime => m_Lifetime;
        public GameObject Caster { get; set; }
        public GameObject Receiver { get; set; }
        public SkillType SkillType => m_SkillType;

        // 외부 종속성 필드 (External dependencies field)
        private ISkillLifecycleHandler[] m_Handlers;
        private ISkillEffectLifecycleHandler[] m_EffectHandlers;
        private IAttackTargetProvider m_AttackTargetSelector;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_Handlers = GetComponents<ISkillLifecycleHandler>();
            m_EffectHandlers = GetComponents<ISkillEffectLifecycleHandler>();
            m_AttackTargetSelector = GetComponent<IAttackTargetProvider>();
            if (m_AttackTargetSelector == null)
            {
                Debug.LogWarning("[SkillBase]: AttackTargetSelector를 찾을 수 없습니다.");
            }
        }

        private void OnEnable()
        {
            // TODO: 임시
            Destroy(gameObject, m_Lifetime);
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
            OnHitEnterEffect(Caster, collision.gameObject);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!m_AttackTargetSelector.IsAllowedTarget(collision.gameObject.tag))
                return;

            OnHitStay(collision.gameObject);
            OnHitStayEffect(Caster, collision.gameObject);
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
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (!m_AttackTargetSelector.IsAllowedTarget(collider.tag))
                return;

            OnHitStay(collider.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (!m_AttackTargetSelector.IsAllowedTarget(collider.tag))
                return;

            OnHitExit(collider.gameObject);
            OnHitAfter();
        }

        // Public 메서드
        public void Init(GameObject caster, GameObject receiver)
        {
            Caster = caster;
            Receiver = receiver;
            foreach (var handler in m_Handlers)
            {
                handler.OnSkillCast(Caster);
            }
            foreach (var handler in m_EffectHandlers)
            {
                handler.OnCastEffect(Caster);
            }
        }

        // Private 메서드
        private void OnHitBefore()
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnSkillHitBefore(Caster);
            }
        }

        private void OnHitEnter(GameObject defender)
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnSkillHitEnter(defender);
            }
        }

        private void OnHitEnterEffect(GameObject caster, GameObject receiver)
        {
            foreach (var handler in m_EffectHandlers)
            {
                handler.OnHitEnterEffect(caster, receiver);
            }
        }

        private void OnHitStay(GameObject defender)
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnSkillHitStay(defender);
            }
        }

        private void OnHitStayEffect(GameObject caster, GameObject receiver)
        {
            foreach (var handler in m_EffectHandlers)
            {
                handler.OnHitStayEffect(caster, receiver);
            }
        }

        private void OnHitExit(GameObject defender)
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnSkillHitExit(defender);
            }
        }

        private void OnHitAfter()
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnSkillHitAfter(Caster);
            }
        }

        private void Release()
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnSkillEnd(Caster);
            }
        }

        // Others

    } // Scope by class SkillBase
} // namespace SkyDragonHunter