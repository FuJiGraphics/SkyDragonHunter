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
        // �ʵ� (Fields)
        public int id;
        public string skillName;
        public double damage;   // ������
        public float coolDown;  // ��ٿ�

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (ScriptableObject �⺻ �޼���)
        // Public �޼���
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