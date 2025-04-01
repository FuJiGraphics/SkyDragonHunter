using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class CrystalLevelData : DataTableData
    {
        public float NeedEXP { get; set; }
        public float AtkUP {  get; set; }
        public float HPUP { get; set; }
        public int NextLvID { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }




    public class CrystalLevelTable : DataTable<CrystalLevelData>
    {
    }

} // namespace Root