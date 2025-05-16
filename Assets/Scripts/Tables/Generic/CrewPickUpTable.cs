using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class CrewPickUpData : DataTableData
    {
        public int CrewID { get; set; }
        public float DrawRate { get; set; }
    }

    public class CrewPickUpTable : DataTable<CrewPickUpData>
    {
        
    
    } // Scope by class CrewPickUpTable

} // namespace Root