using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Numerics;
using UnityEngine;

namespace SkyDragonHunter.Scriptables {

    [CreateAssetMenu(fileName = "CanonDefinition.asset", menuName = "Canons/CanonDefinition")]
    public class CanonDefinition : ScriptableObject
    {
        // �ʵ� (Fields)
        public int id;                      // ����ID
        public string canName;              // ���� �̸�
        public int canGrade;                // �븻=0 ����=1 ����ũ=2 ������=3 
        public string canEqATK;             // �⺻ ���� ���ݷ�
        public string canEqDEF;             // �⺻ ���� ����
        public string canHoldATK;           // �⺻ ���� ���ݷ�
        public string canHoldDEF;           // �⺻ ���� ����
        public string canLvUpCost;          // ���� �� ��� ���� ��
        public string canEqATKup;           // ���� ���ݷ� ��·�
        public string canEqDEFup;           // ���� ���� ��·�
        public string canHoldATKup;         // ���� ���ݷ� ��·�
        public string canHoldDEFup;         // ���� ���� ��·�
        public float canATKMultiplier;      // ������ �ѹ�
        public float canCooldown;           // ���� ��ٿ�
        public int canAilmentID;            // ���� �̻� ID
        public float canAilmentDuration;    // ���� �̻� ���� �ð�
        public int canUpgradeID;            // ���� �ռ� ��� ID

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (ScriptableObject �⺻ �޼���)
        // Public �޼���
        public Attack CreateAttack(CharacterStatus aStats, CharacterStatus dStats)
        {
            if (aStats == null || dStats == null)
            {
                Debug.LogWarning("[CanonDefinition]: CreateAttack ���� �� �� ���� ����");
                return new Attack();
            }

            Attack attack = new Attack();
            attack.defender = dStats.gameObject;
            BigNum damage = aStats.MaxDamage;
            float criticalChance = Mathf.Clamp01(aStats.CriticalChance);

            /* ccj)
             * double ������ ������ 1.5������ ��� ó�� �����ϰ��ϰ� 
             * double�� �ʰ��ϴ� ������ 1.5�� �Ҽ����� ������ �����ϴ� ���� ä��
             */
            if (criticalChance > UnityEngine.Random.value)
            {
                // TODO: AlphaUnit Convert
                //double criDamage = (double)damage * aStats.CriticalMultiplier;
                //if (criDamage < double.MaxValue)
                //{
                //    damage = (BigInteger)criDamage;
                //}
                //else
                //{
                //    damage *= (BigInteger)aStats.CriticalMultiplier;
                //}
                damage *= aStats.CriticalMultiplier;
                // ~TODO
                attack.isCritical = true;
            }
            if (dStats.gameObject.tag == "Boss")
            {
                // TODO: AlphaUnit Convert
                //double bossDamage = (double)damage * aStats.BossDamageMultiplier;
                //if (bossDamage < double.MaxValue)
                //{
                //    damage = (BigInteger)bossDamage;
                //}
                //else
                //{
                //    damage *= (BigInteger)aStats.BossDamageMultiplier;
                //}
                damage *= aStats.BossDamageMultiplier;
                // ~TODO
            }

            float cannonAtkMultiplier = 1f;
            if (AccountMgr.EquipCannonDummy != null)
            {
                var equipCanonGo = AccountMgr.EquipCannonDummy.GetCanonInstance();
                if (equipCanonGo.TryGetComponent<CanonBase>(out var cannonBase))
                {
                    cannonAtkMultiplier = cannonBase.CanonData.canATKMultiplier;
                }
            }
            attack.damage = damage * cannonAtkMultiplier;
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