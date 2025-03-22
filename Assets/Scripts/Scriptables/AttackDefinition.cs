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
        public GameObject prefab;        // 공격 스프라이트 오브젝트
        public GameObject owner;         // 공격하는 오브젝트
        public Transform dummy;          // 부착 위치
        public float coolDown;           // 쿨다운
        public float range;              // 공격 범위
        public AlphaUnit minDamage;      // 최소 데미지
        public AlphaUnit maxDamage;      // 최고 데미지
        public float criticalChance;     // 크리티컬 확률
        public AlphaUnit criticalDamage; // 크리티컬 데미지

        // 속성 (Properties)
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
            AlphaUnit damage = aStats.damage;
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
                attack.damage -= dStats.armor;
            }
            return attack;
        }

        // Private 메서드
        // Others

    } // Scope by class NewScript
} // namespace SkyDragonHunter