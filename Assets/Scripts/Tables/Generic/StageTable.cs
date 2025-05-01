using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;

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
        public BigNum MonsterMultiplierHP { get; set; }
        public BigNum MonsterMultiplierATK { get; set; }
        public BigNum MonsterGOLD { get; set; }
        public BigNum MonsterEXP { get; set; }
        public TreasureDropGrade_Temp TreasureDropGrade { get; set; }
        public float WaveLength { get; set; }
        public int ChallengeBossID { get; set; }
        public BigNum BossMultiplierHP { get; set; }
        public BigNum BossMultiplierATK { get; set; }
        public string ClearRewardGold { get; set; }
        public string ClearRewardEXP { get; set; }
        public int ClearRewardDiamond { get; set; }
        public TreasureDropGrade_Temp ClearRewardTreasureGrade { get; set; }
        public int ClearStageID { get; set; }
        public int AFKRewardTableID { get; set; }
        public int FailStageID { get; set; }
        public string ForeGround { get; set; }
        public string MidGround {  get; set; }
        public string BackGround { get; set; }
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