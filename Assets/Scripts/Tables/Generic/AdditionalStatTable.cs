using NPOI.SS.Formula.Functions;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using System;
using System.Collections.Generic;
using UnityEditor.Search;

namespace SkyDragonHunter.Tables {

    public enum AdditionalStatType
    {
        Damage,
        Health,
        Armor,
        Resilient,
        CriticalChance,
        CriticalMultiplier,
        BossMultiplier,
        SkillEffectMultiplier,
    }

    public class AdditionalStatData : DataTableData
    {
        public AdditionalStatType StatType { get; set; }
        public BigNum RareStatValue { get; set; }
        public BigNum EpicStatValue { get; set; }
        public BigNum UniqueStatValue { get; set; }
        public BigNum LegendStatValue { get; set; }
    }

    public class AdditionalStatTable : DataTable<AdditionalStatData>
    {
        public AdditionalStatData[] Random(int count)
        {
            List<AdditionalStatData> result = new();

            List<AdditionalStatData> randPick = new(base.ToArray());

            count = Math.Min(count, randPick.Count);
            for (int i = 0; i < count; ++i)
            {
                int randIndex = UnityEngine.Random.Range(0, randPick.Count);
                result.Add(randPick[randIndex]);
                randPick.RemoveAt(randIndex);
            }

            return result.ToArray();
        }

    } // Scope by class AdditionalStatTable

} // namespace Root