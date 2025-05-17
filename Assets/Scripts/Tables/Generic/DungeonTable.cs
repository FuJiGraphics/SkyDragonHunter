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
        public int MonsterWaveID { get; set; }
        public int BossID { get; set; }
        public BigNum MultiplierHP { get; set; }
        public BigNum MultiplierATK { get; set; }
        public int KillCount { get; set; }
        public int RewardItemID { get; set; }
        public BigNum RewardCounts { get; set; }
    }

    public class DungeonTable : DataTable<DungeonTableData>
    {
        public int GetDungeonCount(DungeonType dungeonType)
        {            


            return 0;
        }

        public DungeonTableData Get(DungeonType dungeonType, int stageIndex)
        {
            return Get(GetDungeonID(dungeonType, stageIndex));
        }

        public int GetDungeonID(DungeonType dungeonType, int stageIndex)
        {
            int defaultIndex = 3100000;
            int addant = 100000;
            int dungeonID = defaultIndex + addant * (int)dungeonType + stageIndex;
            
            return dungeonID;
        }

        public DungeonTableData GetCurrentDungeonData()
        {
            return Get(DungeonMgr.CurrentDungeonType, DungeonMgr.CurrentStageIndex);
        }
    }
} // namespace Root