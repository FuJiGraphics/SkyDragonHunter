using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;

namespace SkyDragonHunter.Tables {

    public enum ArtifactGrade
    {
        Rare,
        Epic,
        Unique,
        Legend,
    }

    public enum ArtifactHoldStatType
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

    public class ArtifactData : DataTableData
    {
        public ArtifactGrade Grade { get; set; }
        public ArtifactHoldStatType StatType { get; set; }
        public BigNum StatValue { get; set; }
    }

    public class ArtifactTable : DataTable<ArtifactData>
    {        

    } // Scope by class ArtifactTable

} // namespace Root