using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using UnityEngine;
using UnityEngine.Pool;

namespace SkyDragonHunter.Gameplay
{
    public class HitTriggerProjectile : MonoBehaviour
    {
        // 필드 (Fields)
        public Attack attackInfo;
        public float lifeTime = 5f;
        public bool isSingleAttack = true;
        public AilmentType[] ailments;
        public string[] targetTags;
        public bool perfectAimAttack = false;

        private bool m_HasAttacked = false;
        private float m_lastTime = 0f;

        // 속성 (Properties)
        public GameObject OwnerShooter { get; set; }
        public ObjectPool<GameObject> OwnerPool { get; set; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드
        private void OnEnable()
        {
            m_HasAttacked = false;
            m_lastTime = Time.time + lifeTime;
        }

        private void Update()
        {
            UpdateLifeTime();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            this.OnTriggerEnter2D(collision.collider);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_HasAttacked && isSingleAttack)
                return;

            if (!CanAttack(collision.gameObject.tag))
                return;

            if (perfectAimAttack && attackInfo.defender != collision.gameObject)
                return;

            IAttackable target = collision.gameObject.GetComponent<IAttackable>();
            if (target == null)
                return;

            if (ailments != null && ailments.Length > 0)
            {
                ApplyStatusAliments(OwnerShooter, collision.gameObject);
            }
            target.OnAttack(OwnerShooter, attackInfo);
            m_HasAttacked = true;
            DestroyProjectile();
        }
        // Public 메서드
        // Private 메서드
        private bool CanAttack(string target)
        {
            bool canAttack = false;
            foreach (string tag in targetTags)
            {
                if (target == tag)
                {
                    canAttack = true;
                    break;
                }
            }
            return canAttack;
        }

        private void UpdateLifeTime()
        {
            if (Time.time >= m_lastTime)
            {
                m_lastTime = Time.time + lifeTime;
                DestroyProjectile();
            }
        }

        private void DestroyProjectile()
        {
            if (OwnerPool != null)
                OwnerPool.Release(gameObject);
            else
                GameObject.Destroy(this);
        }

        private void ApplyStatusAliments(GameObject attacker, GameObject defender)
        {
            foreach (var ailment in ailments)
            {
                if (defender.TryGetComponent<AilmentAffectable>(out var ailmentComp))
                {
                    ailmentComp.Execute(ailment, 3f, attacker);
                }
            }
        }

        // Others

    } // Scope by class HitTriggerProjectile
} // namespace Root