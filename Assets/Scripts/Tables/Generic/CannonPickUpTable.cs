using SkyDragonHunter.Tables.Generic;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class CannonPickUpData : DataTableData
    {
        public int CannonID { get; set; }
        public float DrawRate { get; set; }
        public int AffinityLvID { get; set; }
    }

    public class CannonPickUpTable : DataTable<CannonPickUpData>
    {      
        public List<CannonPickUpData> GetCannonPickUpDatasWithAffinID(int affinID)
        {
            List<CannonPickUpData> result = new List<CannonPickUpData>();
            foreach(var data in m_dict.Values)
            {
                if (data.AffinityLvID == affinID)
                    result.Add(data);
            }
            return result;
        }
    } // Scope by class CannonPickUpTable

} // namespace Root