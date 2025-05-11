using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Utility;
using SkyDraonHunter.Utility;
using System;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace SkyDragonHunter.Scriptables {

    public enum SkillCastType
    {
        MeleeSingle,
        MeleeMulti,
        RangedSingle,
        RangedMulti,
        CastingSingle,
        CastingMulti,
        Global,
    }

    public enum BuffTarget
    {
        None,
        Receiver,
        Caster,
        All,
    }

    [CreateAssetMenu(fileName = "SkillDefinition.asset", menuName = "Skills/SkillDefinition")]
    public class SkillDefinition : ScriptableObject
    {
        // �ʵ� (Fields)
        public int ID;                      // ��ų ���̵�
        public string skillActiveName;      // ��Ƽ�� ��ų�� �̸�
        public string skillActiveIcon;      // ��Ƽ�� ��ų ������ ���ҽ� �̸�
        public string skillActiveDesc;      // ��Ƽ�� ��ų ����
        public string skillPassiveName;     // �нú� ��ų�� �̸�
        public string skillPassiveIcon;     // �нú� ������ ���ҽ� �̸�
        public string skillPassiveDesc;     // �нú� ��ų ����
        public SkillCastType skillType;     // ��ų�� Ÿ�� 0 : �ٰŸ�(����), 1 : �ٰŸ�(����), 2 : ���Ÿ�(����), 3 : ���Ÿ�(����), 4 : ����ü ���� ���Ÿ�(����), 5 : ����ü ���� ���Ÿ�(����) 6 : �۷ι�
        public string skillEffect;          // ��ų ����Ʈ ���ҽ��� �̸�
        public string projectileName;       // ����ü ���ҽ� �̸�
        public float projectileSpeed;       // ����ü �ӵ�
        public string explosionEffect;      // ����ü ���� ����Ʈ ���ҽ� �̸�
        public int skillArea;               // ��ų�� ����Ǿ��� �� �ֺ��� ��ü�� ������ �޴� ����
        public float skillMultiplier;       // ��ų�� ����޴� ���ݷ��� ����
        public int skillHitCount;           // ��ų�� ����Ǿ��� �� Ÿ�� Ƚ��
        public float skillHitDuration;      // ��ų�� ����Ǵ� ������ �ð�
        public string buffID;               // ȿ�� ID
        public BuffTarget buffTarget;       // ȿ�� ���� ��� (0: ȿ�� ����, 1: ��ų ���� ���, 2: ������, 3:�Ʊ� ��ü)
        public int ailmentID;               // �����̻��� ID
        public float ailmentDuration;       // �����̻��� ���� �ð�

        // �Ӽ� (Properties)
        public BuffData[] BuffData => buffID.Length <= 0 ?
            null : buffID.Split('/')
                         .Select(s => DataTableMgr.BuffTable.Get(int.Parse(s, CultureInfo.InvariantCulture)))
                         .ToArray();

        public float BuffMaxDuration => buffID.Length <= 0 ?
            1f : buffID.Split('/')
                       .Select(s => DataTableMgr.BuffTable.Get(int.Parse(s, CultureInfo.InvariantCulture)).BuffDuration)
                       .Max();

        public Sprite ActiveSkillIcon => ResourcesMgr.Load<Sprite>(skillActiveIcon);
        public Sprite PassiveSkillIcon => ResourcesMgr.Load<Sprite>(skillPassiveIcon);

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
            BigNum newDamage = aStats.MaxDamage;
            // TODO: AlphaUnit Convert
            //if ((double)newDamage > (double)float.MaxValue)
            //    newDamage *= Math2DHelper.SmartRound(newMultiplier);
            //else
            //    newDamage *= newMultiplier;
            newDamage *= newMultiplier;
            // ~TODO


            if (dStats.gameObject.tag == "Boss")
            {
                // TODO: AlphaUnit Convert
                //if ((double)newDamage > (double)float.MaxValue)
                //    newDamage *= Math2DHelper.SmartRound(aStats.BossDamageMultiplier);
                //else
                //    newDamage *= aStats.BossDamageMultiplier;
                newDamage *= aStats.BossDamageMultiplier;
                // ~TODO
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