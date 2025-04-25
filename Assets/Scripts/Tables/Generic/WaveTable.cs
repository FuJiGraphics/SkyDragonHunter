using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class WaveTableData : DataTableData
    {
        public int[] MonsterIDs { get; set; }
        public int[] MonsterCounts { get; set; }
    }

    public class WaveTable : DataTable<WaveTableData>
    {
        
    } // Scope by class WaveTable

} // namespace Root