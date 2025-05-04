using NPOI.SS.Formula.Functions;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using System;
using System.Collections.Generic;

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
        public AdditionalStatData[] Random(int count, ArtifactGrade grade)
        {
            List<AdditionalStatData> result = new();

            var randPick = GetArtifactList(grade);

            count = Math.Min(count, randPick.Count);
            for (int i = 0; i < count; ++i)
            {
                int randIndex = UnityEngine.Random.Range(0, randPick.Count);
                result.Add(randPick[randIndex]);
                // randPick.RemoveAt(randIndex); 중복 방지 코드
            }

            return result.ToArray();
        }

        private List<AdditionalStatData> GetArtifactList(ArtifactGrade grade)
        {
            var allElements = base.ToArray();
            List<AdditionalStatData> result = new();

            foreach (var element in allElements)
            {
                switch (grade)
                {
                    case ArtifactGrade.Rare:
                        if (element.RareStatValue != 0)
                            result.Add(element);
                        break;
                    case ArtifactGrade.Epic:
                        if (element.EpicStatValue != 0)
                            result.Add(element);
                        break;
                    case ArtifactGrade.Unique:
                        if (element.UniqueStatValue != 0)
                            result.Add(element);
                        break;
                    case ArtifactGrade.Legend:
                        if (element.LegendStatValue != 0)
                            result.Add(element);
                        break;
                }
            }

            return result;
        }

    } // Scope by class AdditionalStatTable

} // namespace Root