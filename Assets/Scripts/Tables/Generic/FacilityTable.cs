using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class FacilityTableData : DataTableData
    {
        public string FacilityName { get; set; }
        public int FacilityLevel { get; set; }
        public int ItemID { get; set; }
        public float ItemMadeTime { get; set; }
        public int ItemYield { get; set; }
        public int KeepItemAmount { get; set; }
        public BigNum UpgradeGold { get; set; }
        public int[] UpgradeItemID { get; set; }
        public BigNum[] UpgradeItemCount { get; set; }
        public int UpgradeTime { get; set; }
        public int UpgradeFacilityID { get; set; }

        public bool Upgradable => UpgradeFacilityID != 0;
        public FacilityType Type
        {
            get
            {
                if (ID > 310000000 && ID < 320000000)
                {
                    return FacilityType.Kitchen;
                }
                else if (ID > 320000000 && ID < 330000000)
                {
                    return FacilityType.GearFactory;
                }
                else if (ID < 340000000)
                {
                    return FacilityType.ToolFactory;
                }
                else
                {
                    return FacilityType.MagicWorkshop;
                }
            }
        }
    }

    public class FacilityTable : DataTable<FacilityTableData>
    {
        
    } // Scope by class FacilityTable

} // namespace Root