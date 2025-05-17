using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables
{

    public class RepairPickUpData : DataTableData
    { 
        public int ItemID { get; set; }
        public float RepDrawRate { get; set; }
        public int AffinityLvID { get; set; }
    }

    public class RepairPickUpTable : DataTable<RepairPickUpData>
    {
        public List<RepairPickUpData> GetRepairPickUpDatasWithAffinID(int affinID)
        {
            List<RepairPickUpData> result = new List<RepairPickUpData>();
            foreach (var data in m_dict.Values)
            {
                if (data.AffinityLvID == affinID)
                    result.Add(data);
            }
            return result;
        }

    } // Scope by class RepairerPickUpTable

} // namespace Root