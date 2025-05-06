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
    }

    public class BuffData : DataTableData
    {
        public BuffType BuffApply { get; set; }         // ���� ȿ���� ���� ��� 0: ���, 1: ����
        public string BuffEffect { get; set; }          // ���� ����Ʈ ���ҽ� ��
        public BuffStatType BuffStatType { get; set; }  // ���� ���� Ÿ��
        public float BuffMultiplier { get; set; }       
        public float BuffDuration { get; set; }         // 0�� ��� ���� �ð� ����

        public float EffectiveMultiplier => BuffApply == BuffType.Debuff
            ? Mathf.Clamp01(1f - (BuffMultiplier / 100f))
            : 1f + (BuffMultiplier / 100f);
    }

    public class BuffTable : DataTable<BuffData>
    {

    } // Scope by class BuffTable

} // namespace Root