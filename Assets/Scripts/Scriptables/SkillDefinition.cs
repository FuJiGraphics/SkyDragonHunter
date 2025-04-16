using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using UnityEngine;

namespace SkyDragonHunter.Scriptables {

    [CreateAssetMenu(fileName = "SkillDefinition.asset", menuName = "Skills/SkillDefinition")]
    public class SkillDefinition : ScriptableObject
    {
        // �ʵ� (Fields)
        public int ID;                      // ��ų ���̵�
        public string skillName;            // ��ų�� �̸�
        public int skillType;               // ��ų�� Ÿ�� 0 : �ٰŸ�(����), 1 : �ٰŸ�(����), 2 : ���Ÿ�(����), 3 : ���Ÿ�(����), 4 : ����ü ���� ���Ÿ�(����), 5 : ����ü ���� ���Ÿ�(����) 6 : �۷ι�
        public string skillEffect;          // ��ų ����Ʈ ���ҽ��� �̸�
        public string projectileName;       // ����ü ���ҽ� �̸�
        public float projectileSpeed;       // ����ü �ӵ�
        public string explosionEffect;      // ����ü ���� ����Ʈ ���ҽ� �̸�
        public int skillArea;               // ��ų�� ����Ǿ��� �� �ֺ��� ��ü�� ������ �޴� ����
        public float skillMultiplier;       // ��ų�� ����޴� ���ݷ��� ����
        public int skillHitCount;           // ��ų�� ����Ǿ��� �� Ÿ�� Ƚ��
        public float skillHitDuration;      // ��ų�� ����Ǵ� ������ �ð�
        public int buffID;                  // ȿ�� ID
        public int buffTarget;              // ȿ�� ���� ��� (0: ȿ�� ����, 1: ��ų ���� ���, 2: ������, 3:�Ʊ� ��ü)
        public int ailmentID;               // �����̻��� ID
        public float ailmentDuration;       // �����̻��� ���� �ð�

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)3
        // �̺�Ʈ (Events)
        // ����Ƽ (ScriptableObject �⺻ �޼���)
        // Public �޼���
        public Attack CreateAttack(CharacterStatus aStats, CharacterStatus dStats)
        {
            Attack attack = new Attack();
            attack.attacker = aStats.gameObject;
            attack.defender = dStats.gameObject;

            double newMultiplier = skillMultiplier * aStats.SkillEffectMultiplier;
            AlphaUnit newDamage = aStats.MaxDamage.Value;
            if ((double)newDamage.Value > (double)float.MaxValue)
                newDamage *= Math2DHelper.SmartRound(newMultiplier);
            else
                newDamage *= newMultiplier;

            if (dStats.gameObject.tag == "Boss")
            {
                if ((double)newDamage.Value > (double)float.MaxValue)
                    newDamage *= Math2DHelper.SmartRound(aStats.BossDamageMultiplier);
                else
                    newDamage *= aStats.BossDamageMultiplier;
            }

            attack.damage = newDamage;
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