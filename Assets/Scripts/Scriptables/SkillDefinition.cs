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
        // 필드 (Fields)
        public int ID;                      // 스킬 아이디
        public string skillActiveName;      // 액티브 스킬의 이름
        public string skillActiveIcon;      // 액티브 스킬 아이콘 리소스 이름
        public string skillActiveDesc;      // 액티브 스킬 설명
        public string skillPassiveName;     // 패시브 스킬의 이름
        public string skillPassiveIcon;     // 패시브 아이콘 리소스 이름
        public string skillPassiveDesc;     // 패시브 스킬 설명
        public SkillCastType skillType;     // 스킬의 타입 0 : 근거리(단일), 1 : 근거리(범위), 2 : 원거리(단일), 3 : 원거리(범위), 4 : 투사체 없는 원거리(단일), 5 : 투사체 없는 원거리(범위) 6 : 글로벌
        public string skillEffect;          // 스킬 이펙트 리소스의 이름
        public string projectileName;       // 투사체 리소스 이름
        public float projectileSpeed;       // 투사체 속도
        public string explosionEffect;      // 투사체 폭발 이펙트 리소스 이름
        public int skillArea;               // 스킬이 적용되었을 때 주변의 객체가 영향을 받는 범위
        public float skillMultiplier;       // 스킬이 적용받는 공격력의 비율
        public int skillHitCount;           // 스킬이 적용되었을 때 타격 횟수
        public float skillHitDuration;      // 스킬이 적용되는 간격의 시간
        public string buffID;               // 효과 ID
        public BuffTarget buffTarget;       // 효과 적용 대상 (0: 효과 없음, 1: 스킬 적용 대상, 2: 시전자, 3:아군 전체)
        public int ailmentID;               // 상태이상의 ID
        public float ailmentDuration;       // 상태이상의 지속 시간

        // 속성 (Properties)
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

        // 외부 종속성 필드 (External dependencies field)3
        // 이벤트 (Events)
        // 유니티 (ScriptableObject 기본 메서드)
        // Public 메서드
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