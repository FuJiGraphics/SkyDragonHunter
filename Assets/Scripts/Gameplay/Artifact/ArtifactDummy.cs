using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDraonHunter.Utility;
using System.Collections;
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
        public ArtifactGrade Grade => m_ArtifactData.Grade;
        public ConstantStat ConstantStat => m_CacheConstantStat;
        public AdditionalStat[] AdditionalStats => m_CacheAdditionalStats.ToArray();

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
            m_AdditionalStats = GetRandomAdditionalStats(randCount);
            m_CacheAdditionalStats = GetAdditionalStats();
        }

        public void RerollAdditionalStats()
            => m_AdditionalStats = GetRandomAdditionalStats(m_AdditionalStats.Length);
        // Private �޼���
        private AdditionalStatData[] GetRandomAdditionalStats(int count)
            => DataTableMgr.AdditionalStatTable.Random(count);

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
