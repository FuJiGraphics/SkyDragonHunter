using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using UnityEngine;
using UnityEngine.Pool;

namespace SkyDragonHunter {

    public class LinearProjectile : MonoBehaviour
        , IProjectable
    {
        // 필드 (Fields)
        public float speed = 1f;
        public Vector2 direction = Vector2.right;

        private HitTriggerProjectile m_HitTrigger;
        private bool m_HasFired = false;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_HitTrigger = GetComponent<HitTriggerProjectile>();
        }

        private void Update()
        {
            LinearMovement();
        }

        // Public 메서드
        public void Fire(Vector2 position, Vector2 direction, float projectileSpeed, Attack attack, GameObject owner, ObjectPool<GameObject> ownerPool = null)
        {
            this.transform.position = position;
            this.speed = projectileSpeed;
            this.direction = direction;
            if (m_HitTrigger != null)
            {
                m_HitTrigger.OwnerShooter = owner;
                m_HitTrigger.OwnerPool = ownerPool;
            }
            m_HasFired = true;
        }

        // Private 메서드
        private void LinearMovement()
        {
            if (!m_HasFired)
                return;

            Vector2 currPos = transform.position;
            Vector2 nextDir = direction * speed * Time.deltaTime;
            transform.position = currPos + nextDir;
        }

        // Others

    } // Scope by class LinearProjectile
} // namespace SkyDragonHunter