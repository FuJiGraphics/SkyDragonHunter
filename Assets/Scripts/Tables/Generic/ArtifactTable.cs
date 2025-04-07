using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class  ArtifactData : DataTableData
    {
        // public int ID { get; set; }
        public string Name { get; set; }
        public Rank ArtifactRank { get; set; }
        public StatType OwnedBonusType { get; set; }
        public double InitialOwnedValue { get; set; }
        public double IncreasingOwnedValue { get; set; }
    }

    public class ArtifactTable : DataTable<ArtifactData>
    {        
    } // Scope by class ArtifactTable

} // namespace Root