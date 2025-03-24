using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Scriptables
{
    [CreateAssetMenu(fileName = "CanonSC.asset", menuName = "Weapon/CanonSC")]
    public class CanonSC : AttackDefinition
    {
        // 필드 (Fields)
        public GameObject projectilePrefab;
        public float projectileSpeed = 5f;
        public string[] targetTags;

        private ObjectPool<GameObject> m_ProjectilePool;
        private List<GameObject> m_GenList;
        private float m_LastCooldown;

        public void OnEnable()
        {
            this.Initialized();
        }

        private void OnDisable()
        {
            this.Release();
        }

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // Public 메서드
        public void Initialized()
        {
            if (m_GenList != null)
            {
                foreach (var gen in m_GenList)
                {
                    m_ProjectilePool.Release(gen);
                }
                m_GenList = null;
            }
            m_ProjectilePool = new ObjectPool<GameObject>(
                createFunc: this.OnCreateObject,
                actionOnGet: this.OnGetObject,
                actionOnRelease: (proj) => proj.SetActive(false),
                actionOnDestroy: (proj) => Destroy(proj.gameObject),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 50
            );
            m_GenList = new List<GameObject>();
            m_LastCooldown = Time.time;
        }

        public void Release()
        {
            foreach (var gen in m_GenList)
            {
                if (gen.activeSelf)
                {
                    m_ProjectilePool.Release(gen);
                }
            }
        }

        // Private 메서드
        private GameObject OnCreateObject()
            => Instantiate(projectilePrefab);

        private void OnGetObject(GameObject obj)
        {
            obj.SetActive(true);
            obj.transform.position = Vector2.zero;
        }

        // Others
        public override void Execute(GameObject attacker, GameObject defender)
        {
            if (Time.time < m_LastCooldown)
                return;
            if (attacker == null)
                return;
            if (projectilePrefab == null)
                return;
            if (m_ProjectilePool == null || m_GenList == null)
                return;

            m_LastCooldown = Time.time + coolDown;

            var instance = m_ProjectilePool.Get();
            IProjectable projectile = instance.GetComponent<IProjectable>();

            if (projectile == null) 
                return;

            Attack attack = new Attack();
            Vector2 targetVec = Vector2.zero;
            Vector2 targetDir = Vector2.zero;
            Vector3 firePos = dummy.transform.position + dummy.transform.forward;
            if (defender != null)
            {
                CharacterStatus aStats = attacker.GetComponent<CharacterStatus>();
                CharacterStatus dStats = defender.GetComponent<CharacterStatus>();
                if (aStats == null || dStats == null)
                {
                    m_ProjectilePool.Release(instance);
                }

                instance.GetComponent<HitTriggerProjectile>().targetTags = targetTags;
                attack = CreateAttack(aStats, dStats);

                targetVec = defender.transform.position - firePos;
                if (targetVec.sqrMagnitude > range * range)
                {
                    m_ProjectilePool.Release(instance);
                    return;
                }
                targetDir = (targetVec).normalized;
            }
            else
            {
                m_ProjectilePool.Release(instance);
                return;
            }

            projectile.Fire(firePos, targetDir, projectileSpeed, attack, owner, m_ProjectilePool);
        }

    } // Scope by class NewScript
} // namespace SkyDragonHunter