using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using UnityEngine;
using UnityEngine.Pool;

namespace SkyDragonHunter.Gameplay
{
    public class HitTriggerProjectile : MonoBehaviour
    {
        // �ʵ� (Fields)
        public Attack attackInfo;
        public float lifeTime = 5f;
        public bool isSingleAttack = true;
        public string[] targetTags;

        private bool m_HasAttacked = false;
        private float m_lastTime = 0f;

        // �Ӽ� (Properties)
        public GameObject OwnerShooter { get; set; }
        public ObjectPool<GameObject> OwnerPool { get; set; }

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���
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

            m_HasAttacked = true;

            if (!CanAttack(collision.gameObject.tag))
                return;

            IAttackable target = collision.gameObject.GetComponent<IAttackable>();
            if (target == null)
                return;

            target.OnAttack(OwnerShooter, attackInfo);
            DestroyProjectile();
        }
        // Public �޼���
        // Private �޼���
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

        // Others

    } // Scope by class HitTriggerProjectile
} // namespace Root