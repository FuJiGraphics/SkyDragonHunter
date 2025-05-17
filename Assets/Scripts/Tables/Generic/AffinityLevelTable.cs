using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public enum AffinityLevelType
    {
        Cannon,
        Repair
    }

    public class AffinityLevelData : DataTableData
    {
        public int AffinityLv { get; set; }
        public int LvUpExp { get; set; }
        public int LvUpRewardID { get; set; }
        public int NextAffinityLvID { get; set; }

        public bool IsMaxLevel => NextAffinityLvID == 0;
    }

    public class AffinityLevelTable : DataTable<AffinityLevelData>
    {
        private const int defaultID =   231000000;
        private const int addant = 10000000;

        public int GetAffinityLevelID(AffinityLevelType affinType, int level)
        {
            int result = defaultID + (int)affinType * addant + level;
            return result;
        }

        public AffinityLevelData GetAffinityLevelData(AffinityLevelType affinType, int level)
        {
            int id = GetAffinityLevelID(affinType, level);
            return m_dict[id];
        }
    } // Scope by class AffinityLevelTable

} // namespace Root