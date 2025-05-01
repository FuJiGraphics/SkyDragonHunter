using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class DungeonTableData : DataTableData
    {
        public DungeonType Type { get; set; }
        public string Name { get; set; }
        public int Stage { get; set; }
        public int TicketID { get; set; }
        public int BossID { get; set; }
        public int KillCount { get; set; }
        public BigNum MultiplierHP { get; set; }
        public BigNum MultiplierATK { get; set; }
        public int[] RewardIDs { get; set; }
        public BigNum[] RewardCounts { get; set; }
    }

    public class DungeonTable : DataTable<DungeonTableData>
    {
        public int GetDungeonCount(DungeonType dungeonType)
        {
            


            return 0;
        }

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