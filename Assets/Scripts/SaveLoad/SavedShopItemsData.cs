using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.UI;
using System.Collections.Generic;

namespace SkyDragonHunter.SaveLoad {

    public enum ShopCategory
    {
        Gold,
        Diamond,
        Reroll
    }

    public enum ShopRefreshType
    {
        Common,
        Daily,
        Weekly,
        Monthly
    }

    public class SavedShopItem
    {
        public ShopCategory category;
        public ShopRefreshType refreshType;
        public ItemType itemType;
        public int currentCount;
        public int maxCount;
        public CurrencyType currencyType;
        public BigNum price;
    }

    public class SavedShopItemsData
    {
        public List<SavedShopItem> shopItems;
        public Dictionary<ShopCategory, Dictionary<ShopRefreshType, SavedShopItem>> shopItemDict;

        


    } // Scope by class SavedShopItemsData

} // namespace Root