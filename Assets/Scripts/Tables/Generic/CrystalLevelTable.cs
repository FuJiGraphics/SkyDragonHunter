using SkyDragonHunter.Tables.Generic;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Tables
{
    public class CrystalLevelData : DataTableData
    {
        public int Level { get; set; }
        public string NeedEXP { get; set; }
        public string AtkUP { get; set; }
        public string HPUP { get; set; }
        public int NextLvID { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"ID:{ID} Level:{Level} NeedExp: {NeedEXP} AtkUp: {AtkUP} HPUP: {HPUP}");
            return sb.ToString();
        }
    }

    public class CrystalLevelTable : DataTable<CrystalLevelData>
    {

    } // Scope by class CrystalLevelTable

} // namespace Root