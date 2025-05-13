using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad {    
   
    public class SavedShopItemList
    {
        public DateTime refreshedTime;
        public ShopType shopType;
        public ShopRefreshType refreshType;
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
            foreach(ShopType shopType in Enum.GetValues(typeof(ShopType)))
            {
                if (!shopItemDict.ContainsKey(shopType))
                    shopItemDict.Add(shopType, new());
                foreach(ShopRefreshType refreshType in Enum.GetValues(typeof(ShopRefreshType)))
                {
                    if (shopType != ShopType.Reroll)
                    {
                        if (!shopItemDict[shopType].ContainsKey(refreshType))
                        {
                            shopItemDict[shopType].Add(refreshType, new());
                            shopItemDict[shopType][refreshType].refreshedTime = DateTime.MinValue;
                            shopItemDict[shopType][refreshType].shopType = shopType;
                            shopItemDict[shopType][refreshType].refreshType = refreshType;
                        }
                    }
                }
            }
            shopItemDict[ShopType.Reroll].Add(ShopRefreshType.Common, new());
            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].refreshedTime = DateTime.MinValue;
            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].shopType = ShopType.Reroll;
            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].refreshType = ShopRefreshType.Common;
        } 

        public void UpdateSavedData()
        {
            var goldShopPanelGO = GameMgr.FindObject($"GoldShopPanel");
            var diamondShopPanelGO = GameMgr.FindObject($"DiamondShopPanel");
            var rerollShopPanelGO = GameMgr.FindObject($"RerollShopPanel");
            if(goldShopPanelGO == null || diamondShopPanelGO == null || rerollShopPanelGO == null)
            {
                Debug.LogError($"ShopItemData Update Failed, panelGO null");
                return;
            }
            var goldShopController = goldShopPanelGO.GetComponent<GoldShopController>();
            var diamondShopController = diamondShopPanelGO.GetComponent<DiamondShopController>();
            var rerollShopController = rerollShopPanelGO.GetComponent<RerollShopController>();
            if(goldShopController == null || diamondShopController == null || rerollShopController == null)
            {
                Debug.LogError($"ShopItemData Update Failed, shop controller null");
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
                Debug.LogError($"ShopItemData Apply Failed, panelGO null");
                return;
            }
            var goldShopController = goldShopPanelGO.GetComponent<GoldShopController>();
            var diamondShopController = diamondShopPanelGO.GetComponent<DiamondShopController>();
            var rerollShopController = rerollShopPanelGO.GetComponent<RerollShopController>();
            if (goldShopController == null || diamondShopController == null || rerollShopController == null)
            {
                Debug.LogError($"ShopItemData Apply Failed, shop controller null");
                return;
            }
        }

        public DateTime? GetRefreshedTime (ShopType category, ShopRefreshType refreshType)
        {
            if(!shopItemDict.ContainsKey(category))
            {
                Debug.LogError($"No data found with shop category {category}");
                return null;
            }
            if (!shopItemDict[category].ContainsKey(refreshType))
            {
                Debug.LogError($"No data found with refresh type {refreshType} in shop category {category}");
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