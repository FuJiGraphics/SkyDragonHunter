using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System;

namespace SkyDragonHunter.Gameplay {

    public class RangedAttackExecutor : MonoBehaviour
    {
        // 필드 (Fields)
        public bool isSkipDistanceCheck = false;
        public bool isSkipCooldownCheck = false;
        public Vector2 fireOffset = Vector2.zero;

        private RangedAttackSC m_Def;
        private ObjectPool<GameObject> m_ProjectilePool;
        private List<GameObject> m_ProjectileGenList;
        private float m_LastCooldown;
        private CharacterInventory m_Inventory;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void OnEnable()
        {
            if (m_ProjectileGenList != null)
            {
                foreach (var gen in m_ProjectileGenList)
                {
                    m_ProjectilePool.Release(gen);
                }
                m_ProjectileGenList = null;
            }
            m_ProjectilePool = new ObjectPool<GameObject>(
                createFunc: this.OnCreateObject,
                actionOnGet: this.OnGetObject,
                actionOnRelease: (proj) => proj.SetActive(false),
                actionOnDestroy: this.OnDestroyObject,
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 50
            );
            m_ProjectileGenList = new List<GameObject>();
            m_LastCooldown = Time.time;
            m_Inventory = GetComponent<CharacterInventory>();
        }

        public void OnDisable()
        {
            foreach (var gen in m_ProjectileGenList)
            {
                if (gen != null && gen.activeSelf)
                    m_ProjectilePool.Release(gen);
            }
            m_ProjectileGenList.Clear();
            m_ProjectilePool?.Clear();
        }

        public void Fire(RangedAttackSC def, GameObject attacker, GameObject defender)
        {
            m_Def = def;

            if (attacker == null)
                return;
            if (m_Def == null || m_Def.projectilePrefab == null)
                return;
            if (m_ProjectilePool == null || m_ProjectileGenList == null)
                return;

            var attackTargetProvider = attacker.GetComponent<IAttackTargetProvider>();
            if (attackTargetProvider == null)
                return;

            // 발사 위치 구하기
            Vector2 firePos = attacker.transform.position;
            GameObject firePreviewGo = m_Inventory?.CurrentEquipPreview;
            if (m_Def.fireDummyName.Length > 0 && firePreviewGo != null)
            {
                IWeaponAnchorProvider weaponAnchor = firePreviewGo.GetComponent<IWeaponAnchorProvider>();
                if (weaponAnchor != null)
                {
                    firePos = weaponAnchor.GetWeaponFirePoint().position;
                }
            }

            if (!isSkipDistanceCheck)
            {
                float distance = Vector2.Distance(defender.transform.position, firePos);
                if (distance > m_Def.range)
                    return;
            }

            ILookAtProvider lookAtComp = firePreviewGo?.GetComponentInParent<ILookAtProvider>();
            if (lookAtComp != null)
            {
                lookAtComp.LookAt(defender);
            }

            if (!isSkipCooldownCheck)
            {
                if (Time.time < m_LastCooldown)
                    return;

                m_LastCooldown = Time.time + m_Def.coolDown;
            }

            var instance = m_ProjectilePool.Get();
            m_ProjectileGenList.Add(instance);
            IProjectable projectile = instance.GetComponent<IProjectable>();

            if (projectile == null)
                return;

            if (defender != null)
            {
                CharacterStatus aStats = attacker.GetComponent<CharacterStatus>();
                CharacterStatus dStats = defender.GetComponent<CharacterStatus>();
                if (aStats == null || dStats == null)
                {
                    m_ProjectilePool.Release(instance);
                    return;
                }

                instance.GetComponent<HitTriggerProjectile>().targetTags = attackTargetProvider.AllowedTargetTags;
                Attack attack = m_Def.CreateAttack(aStats, dStats);

                Vector2 atkPos = firePos + fireOffset;
                Vector2 defPos = defender.transform.position;
                Vector2 tarDir = (defPos - atkPos).normalized;
                projectile.Fire(atkPos, tarDir, m_Def.projectileSpeed, attack, attacker, m_ProjectilePool);
            }
            else
            {
                m_ProjectilePool.Release(instance);
                return;
            }
        }

        // Private 메서드
        private GameObject OnCreateObject()
            => GameObject.Instantiate(m_Def.projectilePrefab);

        private void OnDestroyObject(GameObject obj)
        {
            GameObject.Destroy(obj);
        }

        private void OnGetObject(GameObject obj)
        {
            obj.SetActive(true);
            obj.transform.position = Vector2.zero;
        }

        // Others


    } // public class RangedAttackExecutor
} // namespace SkyDragonHunter.Gameplay