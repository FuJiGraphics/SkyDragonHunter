using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public enum TreasureDropGrade_Temp
    {
        Normal,
        Elite,
        Rare,
        Unique,
        Epic,
        Legend,
    }

    public class StageData : DataTableData
    {
        public int Mission { get; set; }
        public int Zone { get; set; }
        public int WaveTableID { get; set; }
        public float WaveDistance { get; set; }
        public float MonsterMultiplierHP { get; set; }
        public float MonsterMultiplierATK { get; set; }
        public string MonsterGOLD { get; set; }
        public string MonsterEXP { get; set; }
        public TreasureDropGrade_Temp TreasureDropGrade { get; set; }
        public float TargetDistance { get; set; }
        public int ChallengeBossID { get; set; }
        public float BossMultiplierHP { get; set; }
        public float BossMultiplierATK { get; set; }
        public string ClearRewardGold { get; set; }
        public string ClearRewardEXP { get; set; }
        public int ClearRewardDiamond { get; set; }
        public TreasureDropGrade_Temp ClearRewardTreasureGrade { get; set; }
        public int ClearStageID { get; set; }
        public int FailStageID { get; set; }
        public int AFKRewardTableID { get; set; }
        public string GroundImg { get; set; }
        public string BackgroundImg1 {  get; set; }
        public string BackgroundImg2 { get; set; }
    }

    public class StageTable : DataTable<StageData>
    {
        public StageData Get(int mission, int zone)
        {
            int id = 1000000;
            id += mission * 100 + zone;
            return Get(id);
        }
    } // Scope by class StageTable

} // namespace Root