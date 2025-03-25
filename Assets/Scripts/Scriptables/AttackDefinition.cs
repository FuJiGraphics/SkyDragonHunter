using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using UnityEngine;

namespace SkyDragonHunter.Scriptables {

    [CreateAssetMenu(fileName = "AttackDefinition.asset", menuName = "Weapon/AttackDefinition")]
    public class AttackDefinition : ScriptableObject, IAttacker
    {
        // 필드 (Fields)
        public GameObject weaponPrefab;  // 공격 스프라이트 오브젝트
        public float coolDown;           // 쿨다운
        public float range;              // 공격 범위
        public AlphaUnit minDamage;      // 최소 데미지
        public AlphaUnit maxDamage;      // 최고 데미지
        public float criticalChance;     // 크리티컬 확률
        public AlphaUnit criticalDamage; // 크리티컬 데미지

        protected GameObject m_ActivePrefabInstance;    // 현재 활성화된 프리팹 인스턴스
        protected GameObject m_Owner;                   // 공격하는 오브젝트
        protected Transform m_Dummy;                    // 부착 위치

        // 속성 (Properties)
        public void SetOwner(GameObject owner) => m_Owner = owner;
        public void SetDummy(Transform transform) => m_Dummy = transform;
        public void SetActivePrefabInstance(GameObject instance) => m_ActivePrefabInstance = instance;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (ScriptableObject 기본 메서드)
        // Public 메서드
        public virtual void Execute(GameObject attacker, GameObject defender)
        {
            throw new System.NotImplementedException();
        }

        protected Attack CreateAttack(CharacterStatus aStats, CharacterStatus dStats)
        {
            Attack attack = new Attack();
            attack.defender = dStats.gameObject;
            AlphaUnit damage = aStats.currentDamage;
            damage += DoubleRandom.Range(minDamage.Value, maxDamage.Value);
            if (criticalChance > UnityEngine.Random.value)
            {
                attack.critical = criticalDamage;
            }
            else
            {
                attack.critical = 0.0;
            }
            attack.damage = Math.Floor(damage.Value);
            if (dStats != null)
            {
                attack.damage -= dStats.currentArmor;
                if (attack.damage <= 0.0)
                    attack.damage = 0.0;
            }
            return attack;
        }

        // Private 메서드
        // Others

    } // Scope by class NewScript
} // namespace SkyDragonHunter