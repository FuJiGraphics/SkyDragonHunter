using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using UnityEngine;

namespace SkyDragonHunter.Scriptables {

    public class SkillDefinition : ScriptableObject
    {
        // 필드 (Fields)
        public int id;
        public string skillName;
        public double damage;   // 데미지
        public float coolDown;  // 쿨다운

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (ScriptableObject 기본 메서드)
        // Public 메서드
        public Attack CreateAttack(CharacterStatus aStats, CharacterStatus dStats)
        {
            Attack attack = new Attack();
            attack.attacker = aStats.gameObject;
            attack.defender = dStats.gameObject;
            double newDamage = damage * aStats.SkillEffectMultiplier;
            if (dStats.gameObject.tag == "Boss")
            {
                newDamage *= aStats.BossDamageMultiplier;
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

    } // Scope by class SkillDefinition
} // namespace SkyDragonHunter