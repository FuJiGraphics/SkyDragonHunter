using SkyDragonHunter.Entities;
using SkyDragonHunter.Tables.Generic;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Tables
{
    public enum BuffType
    {
        Buff,
        Debuff,
    }

    public enum BuffStatType
    {
        Damage,
        Health,
        Armor,
        Resilient,
        CriticalChance,
        CriticalMultiplier,
        BossMultiplier,
        SkillEffectMultiplier,
        AttackSpeed,
        CooldownResilient,
        Shield,
    }

    public class BuffData : DataTableData
    {
        public BuffType BuffApply { get; set; }         // 버프 효과의 적용 방법 0: 상승, 1: 감소
        public string BuffEffect { get; set; }          // 버프 이펙트 리소스 명
        public BuffStatType BuffStatType { get; set; }  // 적용 스탯 타입
        public float BuffMultiplier { get; set; }       
        public float BuffDuration { get; set; }         // 0일 경우 지속 시간 무한

        public float EffectiveMultiplier => BuffApply == BuffType.Debuff
            ? Mathf.Clamp01(1f - (BuffMultiplier / 100f))
            : 1f + (BuffMultiplier / 100f);
    }

    public class BuffTable : DataTable<BuffData>
    {

    } // Scope by class BuffTable

} // namespace Root