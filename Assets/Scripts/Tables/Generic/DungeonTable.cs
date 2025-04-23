using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class DungeonTableData : DataTableData
    {
        public string Name { get; set; }
        public DungeonType Type { get; set; }
        public int Stage { get; set; }
        public int[] RewardIds { get; set; }
        public int[] RewardCountMins { get; set; }
        public int[] RewardCountMaxs { get; set; }
        
    }

    public class DungeonTable : DataTable<DungeonTableData>
    {
        public DungeonTableData Get(DungeonType dungeonType, int stageIndex)
        {
            return Get(GetIDfromDungeonData(dungeonType, stageIndex));
        }

        public int GetIDfromDungeonData(DungeonType dungeonType, int stageIndex)
        {
            int defaultIndex = 100000;
            var dungeonTypeVal = ((int)dungeonType) * 10000;
            defaultIndex += dungeonTypeVal + stageIndex;
            return defaultIndex;
        }
    }
} // namespace Root