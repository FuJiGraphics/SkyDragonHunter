using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
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

        public bool IsMaxLevel => UpgradeFacilityID == 0;

        public ItemType ProductType
        {
            get
            {
                return DataTableMgr.ItemTable.Get(ItemID).Type;
            }
        }

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

        public ItemType[] RequiredItemTypes
        {
            get
            {
                ItemType[] result = new ItemType[UpgradeItemID.Length];
                for (int i = 0; i < UpgradeItemID.Length; ++i)
                {
                    result[i] = DataTableMgr.ItemTable.Get(UpgradeItemID[i]).Type;
                }
                return result;
            }
        }
    }

    public class FacilityTable : DataTable<FacilityTableData>
    {
        private const int defaultID = 310000000;
        private const int typeAddant = 10000000;

        public FacilityTableData GetFacilityData(FacilityType type, int level)
        {
            int tempId = defaultID + typeAddant * (int)type + level;

            if(m_dict.ContainsKey(tempId))
            {
                return m_dict[tempId];
            }

            Debug.LogError($"Could not find Facility Data with type/level : [{type}/{level}], id [{tempId}]");
            return null;
        }
    } // Scope by class FacilityTable
} // namespace Root