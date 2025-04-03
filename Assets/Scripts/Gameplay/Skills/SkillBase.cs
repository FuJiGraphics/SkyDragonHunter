using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using UnityEngine;

namespace SkyDragonHunter {
    public class SkillBase : MonoBehaviour
    {
        // 필드 (Fields)
        public SkillDefinition skillData;
        public float lifetime = 5f;

        // 속성 (Properties)
        public GameObject Caster { get; set; }
        public GameObject Receiver { get; set; }

        // 외부 종속성 필드 (External dependencies field)
        private ISkillLifecycleHandler[] m_Handlers;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_Handlers = GetComponents<ISkillLifecycleHandler>();
        }

        private void OnEnable()
        {
            Init();

            // TODO: 임시
            Destroy(gameObject, lifetime);
        }

        private void OnDisable()
        {
            Release();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnHitBefore();
            OnHitEnter(collision.gameObject);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            OnHitStay(collision.gameObject);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            OnHitExit(collision.gameObject);
            OnHitAfter();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            OnHitBefore();
            OnHitEnter(collider.gameObject);
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            OnHitStay(collider.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            OnHitExit(collider.gameObject);
            OnHitAfter();
        }

        // Public 메서드
        // Private 메서드
        private void Init()
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnSkillCast(Caster);
            }
        }

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

        private void OnHitStay(GameObject defender)
        {
            foreach (var handler in m_Handlers)
            {
                handler.OnSkillHitStay(defender);
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