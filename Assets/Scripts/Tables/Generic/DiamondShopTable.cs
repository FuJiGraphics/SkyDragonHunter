using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using SkyDragonHunter.UI;

namespace SkyDragonHunter.Tables {

    public class DiamondShopData : DataTableData
    {
        public int ItemID { get; set; }                     // ������ ������ ���̵�
        public int ItemAmount { get; set; }                 // ������ ����
        public BigNum Price { get; set; }                   // ������ ����
        public ShopCategory BuyLimitType { get; set; }  // ������ ���� ����
        public int ItemBuyLimitCount { get; set; }          // ������ ���� ���� ����
        public float GenWeight { get; set; }

        public ItemType GetItemType()
            => DataTableMgr.ItemTable.Get(ItemID).Type;
    }

    public class DiamondShopTable : DataTable<DiamondShopData>
    {

    } // Scope by class DiamondShopTable

} // namespace Root