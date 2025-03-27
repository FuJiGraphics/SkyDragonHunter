using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
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
    [CreateAssetMenu(fileName = "AttackRangeSC.asset", menuName = "Weapon/AttackRangeSC")]
    public class RangedAttackSC : AttackDefinition
    {
        // 필드 (Fields)
        public GameObject projectilePrefab;
        public float projectileSpeed = 5f;
        public string[] targetTags;
        public bool lookAtTarget;
        [Tooltip("발사 위치에 대한 더미 오브젝트의 이름")]
        [SerializeField] private string m_FireDummyName;

        private ObjectPool<GameObject> m_ProjectilePool;
        private List<GameObject> m_GenList;
        private float m_LastCooldown;
        private GameObject m_FireDummy;
        private bool m_IsFirstExcute = true;

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
            m_IsFirstExcute = true;
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
            m_IsFirstExcute = false;
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
            SetOwner(attacker);
            FirstExcute();

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
            Vector3 firePos = Vector3.zero;

            if (m_FireDummy != null)
            {
                firePos = m_FireDummy.transform.position;
                if (lookAtTarget && m_ActivePrefabInstance != null)
                {
                    Math2DHelper.LookAt(m_ActivePrefabInstance.transform, m_FireDummy.transform, defender.transform);
                }
            }
            else
            {
                firePos = m_ActivePrefabInstance.transform.position;
                if (lookAtTarget && m_ActivePrefabInstance != null)
                {
                    Math2DHelper.LookAt(m_ActivePrefabInstance.transform, m_ActivePrefabInstance.transform, defender.transform);
                }
            }

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
                if (Vector3.Distance(defender.transform.position, firePos) > range)
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

            projectile.Fire(firePos, targetDir, projectileSpeed, attack, m_Owner, m_ProjectilePool);
        }

        private void FirstExcute()
        {
            if (!m_IsFirstExcute)
                return;

            m_IsFirstExcute = false;
            if (m_FireDummyName.Length > 0 && m_ActivePrefabInstance != null)
            {
                m_FireDummy = m_ActivePrefabInstance.transform.Find(m_FireDummyName).gameObject;
                if (m_FireDummy == null)
                {
                    Debug.LogWarning($"Warnning: {weaponPrefab.name}/{m_FireDummyName} 더미 오브젝트를 찾을 수 없습니다.");
                }
            }
        }

    } // Scope by class NewScript
} // namespace SkyDragonHunter