using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using UnityEngine;

namespace SkyDragonHunter.Scriptables {

    public abstract class AttackDefinition : ItemDefinition
    {
        // 필드 (Fields)
        public float coolDown;                      // 쿨다운
        public float range;                         // 공격 범위
        public double damage;                       // 데미지
        public float criticalChance = 0.0f;         // 크리티컬 확률
        public float criticalMultiplier = 1.0f;     // 크리티컬 피해 배율
        public float bossDamageMultiplier = 1.0f;   // 보스 피해량 증가 배율

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (ScriptableObject 기본 메서드)
        // Public 메서드
        public Attack CreateAttack(CharacterStatus aStats, CharacterStatus dStats)
        {
            Attack attack = new Attack();
            attack.defender = dStats.gameObject;
            double newDamage = damage + aStats.MaxDamage.Value;
            float newCriticalChance = Mathf.Clamp01(criticalChance + aStats.CriticalChance);
            float newCriticalMultiplier = criticalMultiplier + aStats.CriticalMultiplier;
            if (newCriticalChance > UnityEngine.Random.value)
            {
                newDamage *= newCriticalMultiplier;
                attack.isCritical = true;
            }
            if (dStats.gameObject.tag == "Boss")
            {
                newDamage *= bossDamageMultiplier;
            }
            attack.damage = Math.Floor(newDamage);
            if (dStats != null)
            {
                attack.damage -= dStats.Armor;
                if (attack.damage <= 1.0)
                    attack.damage = 1.0;
            }
            return attack;
        }

        // Others

    } // Scope by class AttackDefinition
} // namespace SkyDragonHunter