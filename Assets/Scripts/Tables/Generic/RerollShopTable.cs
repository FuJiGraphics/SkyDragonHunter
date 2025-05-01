using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;

namespace SkyDragonHunter.Tables {

    public class RerollShopData : DataTableData
    {
        public int ItemID { get; set; }
        public int ItemAmount { get; set; }
        public BigNum Price { get; set; }
        public float RerollRate { get; set; }
        public int AffinityLevel { get; set; }

        public ItemType GetItemType()
            => DataTableMgr.ItemTable.Get(ItemID).Type;
    }

    public class RerollShopTable : DataTable<RerollShopData>
    {

    } // Scope by class RerollShopTable

} // namespace Root