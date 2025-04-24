using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class AFKRewardData : DataTableData
    {
        public string AFKGold { get; set; }
        public string AFKExp { get; set; }
        public string AFKSupply { get; set; }
        public int AFKRewardTreasureTableID { get; set; }
    }

    public class AFKRewardTable : DataTable<AFKRewardData>
    {        
    } // Scope by class AFKRewardTable

} // namespace Root