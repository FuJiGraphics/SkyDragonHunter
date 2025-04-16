using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class AilmentData : DataTableData
    {
        // public int ID { get; set; }
        public int AilmentType { get; set; }
        public bool Refreshable { get; set; }
        public string Effect { get; set; }
        public float ImmuneTime { get; set; }
    }

    public class AilmentTable : DataTable<AilmentData>
    {
    } // Scope by class ArtifactTable

} // namespace Root