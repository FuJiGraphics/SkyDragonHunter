using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;

namespace SkyDragonHunter.Tables {

    public enum ArtifactAddStatType
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
        public ArtifactAddStatType StatType { get; set; }
        public BigNum RareStatValue { get; set; }
        public BigNum EpicStatValue { get; set; }
        public BigNum UniqueStatValue { get; set; }
        public BigNum LegendStatValue { get; set; }
    }

    public class ArtifactAdditionalStatTable : DataTable<AdditionalStatData>
    {

    } // Scope by class ArtifactAdditionalStatTable

} // namespace Root