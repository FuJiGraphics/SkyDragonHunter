using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;

namespace SkyDragonHunter.Tables {

    public class  CrewLevelTableData : DataTableData
    {
        public CrewGrade UnitGrade {  get; set; }
        public int Level { get; set; }
        public BigNum RequiredEXP { get; set; }
    }

    public class CrewLevelTable : DataTable<CrewLevelTableData>
    {
        public BigNum GetRequiredEXP(CrewGrade unitGrade, int level)
        {
            int tempID = 10000;
            tempID += (int)unitGrade * 1000;
            tempID += level;
            return Get(tempID).RequiredEXP;            
        }
    } // Scope by class CrewLevelTable

} // namespace Root