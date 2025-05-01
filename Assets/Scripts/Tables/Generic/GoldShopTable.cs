using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using SkyDragonHunter.UI;

namespace SkyDragonHunter.Tables {

    public class GoldShopData : DataTableData
    {
        public int ItemID { get; set; }                     // 아이템 데이터 아이디
        public int ItemAmount { get; set; }                 // 아이템 수량
        public BigNum Price { get; set; }                   // 아이템 가격
        public GoldShopCategory BuyLimitType { get; set; }  // 아이템 구매 수량
        public int ItemBuyLimitCount { get; set; }          // 아이템 구매 제한 수량
        public float GenWeight { get; set; }

        public ItemType GetItemType() 
            => DataTableMgr.ItemTable.Get(ItemID).Type;
    }

    public class GoldShopTable : DataTable<GoldShopData>
    {

    } // Scope by class GoldShopTable

} // namespace Root