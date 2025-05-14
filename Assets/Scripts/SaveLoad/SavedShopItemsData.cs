using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad {
    
    public enum ShopRefreshType
    {
        Common,
        Daily,
        Weekly,
        Monthly
    }

    public class SavedShopItemList
    {
        public DateTime refreshedTime;
        public List<SavedShopItem> itemList;        
    }

    public class SavedShopItem
    {
        public ItemType itemType;
        public int currentCount;
        public int maxCount;
        public CurrencyType currencyType;
        public BigNum price;
        public bool isLocked;
        public bool isPurchased;
    }

    public class SavedShopItemsData
    {
        //public List<>
        public Dictionary<ShopType, Dictionary<ShopRefreshType, SavedShopItemList>> shopItemDict;

        //private Dictionary<ShopCategory, Dictionary<ShopRefreshType, Dictionary<int, SavedShopItem>>> shopItemDict;
        //private Dictionary<ShopCategory, Dictionary<ShopRefreshType, DateTime>> refreshedTimeDict;
        //private int[] itemsCount =
        //{
        //    16 , 16 , 6
        //};

        public void InitData()
        {
            shopItemDict = new ();
            foreach(ShopType category in Enum.GetValues(typeof(ShopType)))
            {
                if (!shopItemDict.ContainsKey(category))
                    shopItemDict.Add(category, new());
                foreach(ShopRefreshType refreshType in Enum.GetValues(typeof(ShopRefreshType)))
                {
                    if (!shopItemDict[category].ContainsKey(refreshType))
                        shopItemDict[category].Add(refreshType, new ());
                    shopItemDict[category][refreshType].refreshedTime = DateTime.MinValue;
                }
            }

            //shopItemDict = new ();
            //refreshedTimeDict = new ();
            //
            //for (int i = 0; i < Enum.GetValues(typeof(ShopCategory)).Length; ++i)
            //{
            //    for (int j = 0; j < Enum.GetValues(typeof(ShopRefreshType)).Length; ++j)
            //    {
            //        DateTime tempDateTime = default;
            //        refreshedTime.Add (tempDateTime);
            //        if (!refreshedTimeDict.ContainsKey((ShopCategory)i))
            //            refreshedTimeDict.Add((ShopCategory)i, new());
            //        if (!refreshedTimeDict[(ShopCategory)i].ContainsKey((ShopRefreshType)j))
            //            refreshedTimeDict[(ShopCategory)i].Add((ShopRefreshType)j, tempDateTime);
            //
            //        for(int k = 0; k < itemsCount[i]; ++k)
            //        {
            //            var newSavedShopItem = new SavedShopItem();
            //            newSavedShopItem.category = (ShopCategory)i;
            //            newSavedShopItem.refreshType = (ShopRefreshType)j;
            //            newSavedShopItem.listIndex = k;
            //            newSavedShopItem.itemType = ItemType.Coin;
            //            newSavedShopItem.currentCount = 1;
            //            newSavedShopItem.maxCount = 1;
            //            newSavedShopItem.currencyType = CurrencyType.Coin;
            //            newSavedShopItem.price = 0;
            //            newSavedShopItem.isLocked = false;
            //            newSavedShopItem.isPurchased = false;
            //
            //            shopItems.Add(newSavedShopItem);
            //            if (!shopItemDict.ContainsKey((ShopCategory)i))
            //                shopItemDict.Add((ShopCategory)i, new());
            //            if (!shopItemDict[(ShopCategory)i].ContainsKey((ShopRefreshType)j))
            //                shopItemDict[(ShopCategory)i].Add((ShopRefreshType)j, new());
            //            if (!shopItemDict[(ShopCategory)i][(ShopRefreshType)j].ContainsKey(k))
            //                shopItemDict[(ShopCategory)i][(ShopRefreshType)j].Add(k, newSavedShopItem);
            //        }
            //    }
            //}
        } 

        public void UpdateSavedData()
        {
            var goldShopPanelGO = GameMgr.FindObject($"GoldShopPanel");
            var diamondShopPanelGO = GameMgr.FindObject($"DiamondShopPanel");
            var rerollShopPanelGO = GameMgr.FindObject($"RerollShopPanel");
            if(goldShopPanelGO == null || diamondShopPanelGO == null || rerollShopPanelGO == null)
            {
                // Debug.LogError($"ShopItemData Update Failed, panelGO null");
                return;
            }
            var goldShopController = goldShopPanelGO.GetComponent<GoldShopController>();
            var diamondShopController = diamondShopPanelGO.GetComponent<DiamondShopController>();
            var rerollShopController = rerollShopPanelGO.GetComponent<RerollShopController>();
            if(goldShopController == null || diamondShopController == null || rerollShopController == null)
            {
                // Debug.LogError($"ShopItemData Update Failed, shop controller null");
                return;
            }


        }

        public void ApplySavedData()
        {
            var goldShopPanelGO = GameMgr.FindObject($"GoldShopPanel");
            var diamondShopPanelGO = GameMgr.FindObject($"DiamondShopPanel");
            var rerollShopPanelGO = GameMgr.FindObject($"RerollShopPanel");
            if (goldShopPanelGO == null || diamondShopPanelGO == null || rerollShopPanelGO == null)
            {
                // Debug.LogError($"ShopItemData Apply Failed, panelGO null");
                return;
            }
            var goldShopController = goldShopPanelGO.GetComponent<GoldShopController>();
            var diamondShopController = diamondShopPanelGO.GetComponent<DiamondShopController>();
            var rerollShopController = rerollShopPanelGO.GetComponent<RerollShopController>();
            if (goldShopController == null || diamondShopController == null || rerollShopController == null)
            {
                // Debug.LogError($"ShopItemData Apply Failed, shop controller null");
                return;
            }
        }

        public DateTime? GetRefreshedTime (ShopType category, ShopRefreshType refreshType)
        {
            if(!shopItemDict.ContainsKey(category))
            {
                // Debug.LogError($"No data found with shop category {category}");
                return null;
            }
            if (!shopItemDict[category].ContainsKey(refreshType))
            {
                // Debug.LogError($"No data found with refresh type {refreshType} in shop category {category}");
                return null;
            }

            return shopItemDict[category][refreshType].refreshedTime;
        }

        //public SavedShopItem GetSavedShopItem(ShopCategory category, ShopRefreshType refreshType, int index)
        //{
        //    if (!shopItemDict.ContainsKey(category))
        //    {
        //        Debug.LogError($"Cannot Get Saved Shop Item, no category [{category}]");
        //        return null;
        //    }
        //    if (!shopItemDict[category].ContainsKey(refreshType))
        //    {
        //        Debug.LogError($"Cannot Get Saved Shop Item, no refresh type [{refreshType}] in category [{category}]");
        //        return null;
        //    }
        //    if (!shopItemDict[category][refreshType].ContainsKey(index))
        //    {
        //        Debug.LogError($"Cannot Get Saved Shop Item, no index [{index}] in refresh type [{refreshType}] in category [{category}]");
        //    }
        //
        //    return shopItemDict[category][refreshType][index];
        //}

    } // Scope by class SavedShopItemsData

} // namespace Root