using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using SkyDraonHunter.Utility;
using System.Collections.Generic;

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
        public Dictionary<ArtifactGrade, List<ArtifactData>> m_Cache = new();
        
        public ArtifactData Random(ArtifactGrade grade)
        {
            if (!m_Cache.ContainsKey(grade))
            {
                m_Cache.Add(grade, new());
                foreach (var data in m_dict)
                {
                    if (data.Value.Grade == grade)
                    {
                        m_Cache[grade].Add(data.Value);
                    }
                }
            }
            return RandomMgr.Random(m_Cache[grade]);
        }
    
    } // Scope by class ArtifactTable

} // namespace Root