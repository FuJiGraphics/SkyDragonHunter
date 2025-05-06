using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDraonHunter.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public struct ConstantStat
    {
        public BigNum StatValue { get; private set; }
        public ArtifactHoldStatType StatType { get; private set; }

        public ConstantStat(BigNum value, ArtifactHoldStatType type)
        {
            StatValue = value;
            StatType = type;
        }

        public CommonStats ToCommonStats()
        {
            CommonStats result = new CommonStats();
            result.ResetAllZero();
            switch (StatType)
            {
                case ArtifactHoldStatType.Damage:
                    result.SetMaxDamage(StatValue);
                    break;
                case ArtifactHoldStatType.Health:
                    result.SetMaxHealth(StatValue);
                    break;
                case ArtifactHoldStatType.Armor:
                    result.SetMaxArmor(StatValue);
                    break;
                case ArtifactHoldStatType.Resilient:
                    result.SetMaxResilient(StatValue);
                    break;
                case ArtifactHoldStatType.CriticalChance:
                    result.SetCriticalChance((float)StatValue);
                    break;
                case ArtifactHoldStatType.CriticalMultiplier:
                    result.SetCriticalMultiplier((float)StatValue);
                    break;
                case ArtifactHoldStatType.BossMultiplier:
                    result.SetBossDamageMultiplier((float)StatValue);
                    break;
                case ArtifactHoldStatType.SkillEffectMultiplier:
                    result.SetSkillEffectMultiplier((float)StatValue);
                    break;
            }
            return result;
        }
        public override string ToString()
        {
            string result = "";
            switch (StatType)
            {
                case ArtifactHoldStatType.Damage:
                    result = "���ݷ� +";
                    result += StatValue.ToUnit();
                    break;
                case ArtifactHoldStatType.Health:
                    result = "ü�� +";
                    result += StatValue.ToUnit();
                    break;
                case ArtifactHoldStatType.Armor:
                    result = "���� +";
                    result += StatValue.ToUnit();
                    break;
                case ArtifactHoldStatType.Resilient:
                    result = "ȸ���� +";
                    result += StatValue.ToUnit();
                    break;
                case ArtifactHoldStatType.CriticalChance:
                    result = "ġ��Ÿ Ȯ�� +";
                    result += StatValue.ToString();
                    break;
                case ArtifactHoldStatType.CriticalMultiplier:
                    result = "ġ��Ÿ ���� +";
                    result += StatValue.ToString();
                    break;
                case ArtifactHoldStatType.BossMultiplier:
                    result = "���������� ���� +";
                    result += StatValue.ToString();
                    break;
                case ArtifactHoldStatType.SkillEffectMultiplier:
                    result = "��ų ȿ�� ���� +";
                    result += StatValue.ToString();
                    break;
            }
            return result;
        }
    }

    public struct AdditionalStat
    {
        public BigNum StatValue { get; private set; }
        public AdditionalStatType StatType { get; private set; }

        public AdditionalStat(BigNum value, AdditionalStatType type)
        {
            StatValue = value;
            StatType = type;
        }

        public CommonStats ToCommonStats()
        {
            CommonStats result = new CommonStats();
            result.ResetAllZero();
            switch (StatType)
            {
                case AdditionalStatType.Damage:
                    result.SetMaxDamage(StatValue);
                    break;
                case AdditionalStatType.Health:
                    result.SetMaxHealth(StatValue);
                    break;
                case AdditionalStatType.Armor:
                    result.SetMaxArmor(StatValue);
                    break;
                case AdditionalStatType.Resilient:
                    result.SetMaxResilient(StatValue);
                    break;
                case AdditionalStatType.CriticalChance:
                    result.SetCriticalChance((float)StatValue);
                    break;
                case AdditionalStatType.CriticalMultiplier:
                    result.SetCriticalMultiplier((float)StatValue);
                    break;
                case AdditionalStatType.BossMultiplier:
                    result.SetBossDamageMultiplier((float)StatValue);
                    break;
                case AdditionalStatType.SkillEffectMultiplier:
                    result.SetSkillEffectMultiplier((float)StatValue);
                    break;
            }
            return result;
        }

        public override string ToString()
        {
            string result = "";
            switch (StatType)
            {
                case AdditionalStatType.Damage:
                    result = "���ݷ� +";
                    result += StatValue.ToUnit();
                    break;
                case AdditionalStatType.Health:
                    result = "ü�� +";
                    result += StatValue.ToUnit();
                    break;
                case AdditionalStatType.Armor:
                    result = "���� +";
                    result += StatValue.ToUnit();
                    break;
                case AdditionalStatType.Resilient:
                    result = "ȸ���� +";
                    result += StatValue.ToUnit();
                    break;
                case AdditionalStatType.CriticalChance:
                    result = "ġ��Ÿ Ȯ�� +";
                    result += StatValue.ToString();
                    break;
                case AdditionalStatType.CriticalMultiplier:
                    result = "ġ��Ÿ ���� +";
                    result += StatValue.ToString();
                    break;
                case AdditionalStatType.BossMultiplier:
                    result = "���������� ���� +";
                    result += StatValue.ToString();
                    break;
                case AdditionalStatType.SkillEffectMultiplier:
                    result = "��ų ȿ�� ���� +";
                    result += StatValue.ToString();
                    break;
            }
            return result;
        }
    }

    public class ArtifactDummy
    {
        // �ʵ� (Fields)
        private readonly ArtifactData m_ArtifactData;
        private readonly int m_PickRandomMinCount = 3;
        private readonly int m_PickRandomMaxCount = 4;
        
        private AdditionalStatData[] m_AdditionalStats;
        private ConstantStat m_CacheConstantStat;
        private List<AdditionalStat> m_CacheAdditionalStats;

        // �Ӽ� (Properties)
        public string Name => m_ArtifactData.Name;
        public string Desc => m_ArtifactData.Desc;
        public Sprite Icon => m_ArtifactData.Icon;
        public ArtifactGrade Grade => m_ArtifactData.Grade;
        public ArtifactGrade NextGrade => Enum.IsDefined(typeof(ArtifactGrade), (int)m_ArtifactData.Grade + 1) 
            ? (ArtifactGrade)((int)m_ArtifactData.Grade + 1) : m_ArtifactData.Grade;
        public ConstantStat ConstantStat => m_CacheConstantStat;
        public AdditionalStat[] AdditionalStats => m_CacheAdditionalStats.ToArray();
        public CommonStats CommonStatValue
        {
            get
            {
                CommonStats result = new CommonStats();
                result.ResetAllZero();

                result += ConstantStat.ToCommonStats();
                foreach (var stat in AdditionalStats)
                {
                    result += stat.ToCommonStats();
                }

                return result;
            }
        }

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        public ArtifactDummy(ArtifactGrade grade)
        {
            m_ArtifactData = DataTableMgr.ArtifactTable.Random(grade);
            if (m_ArtifactData == null)
            {
                Debug.LogError("[ArtifactDummy]: Error! ArtifactData�� ã�� �� �����ϴ�.");
            }

            // ���� ���� 
            m_CacheConstantStat = new ConstantStat(m_ArtifactData.StatValue, m_ArtifactData.StatType);

            // �߰� ����, ���� ���� ���ϱ� => 3 �Ǵ� 4
            int randCount = RandomMgr.RandomWithWeights<int>(
                (m_PickRandomMinCount, 0.75f), 
                (m_PickRandomMaxCount, 0.25f));
            m_AdditionalStats = GetRandomAdditionalStats(randCount, grade);
            m_CacheAdditionalStats = GetAdditionalStats();
        }

        public void RerollAdditionalStats()
        {
            m_AdditionalStats = GetRandomAdditionalStats(m_AdditionalStats.Length, Grade);
            m_CacheAdditionalStats = GetAdditionalStats();
        }

        // Private �޼���
        private AdditionalStatData[] GetRandomAdditionalStats(int count, ArtifactGrade grade)
            => DataTableMgr.AdditionalStatTable.Random(count, grade);

        private List<AdditionalStat> GetAdditionalStats()
        {
            List<AdditionalStat> result = new List<AdditionalStat>();
            foreach (var stat in m_AdditionalStats)
            {
                switch (Grade)
                {
                    case ArtifactGrade.Rare:
                        result.Add(new AdditionalStat(stat.RareStatValue, stat.StatType));
                        break;
                    case ArtifactGrade.Epic:
                        result.Add(new AdditionalStat(stat.EpicStatValue, stat.StatType));
                        break;
                    case ArtifactGrade.Unique:
                        result.Add(new AdditionalStat(stat.UniqueStatValue, stat.StatType));
                        break;
                    case ArtifactGrade.Legend:
                        result.Add(new AdditionalStat(stat.LegendStatValue, stat.StatType));
                        break;
                }
            }
            return result;
        }
        // Others

    } // Scope by class AilmentOnTaunt
} // namespace Root
