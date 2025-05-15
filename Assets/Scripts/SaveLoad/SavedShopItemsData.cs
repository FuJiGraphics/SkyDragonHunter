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
        public int id;
        public ItemType itemType;
        public int currentCount;
        public int maxCount;
        public CurrencyType currencyType;
        public BigNum price;
        public bool isLocked;

        public SavedShopItem Clone()
        {
            return new SavedShopItem
            {
                id = this.id,
                itemType = this.itemType,
                currentCount = this.currentCount,
                maxCount = this.maxCount,
                currencyType = this.currencyType,
                price = this.price,
                isLocked = this.isLocked,
            };
        }
    }

    public class SavedShopItemsData
    {
        // TODO: LJH
        public bool isFirstPickGiven;
        // ~TODO
        public int favorabilityLevel;
        public Dictionary<ShopType, Dictionary<ShopRefreshType, SavedShopItemList>> shopItemDict;
        
        public void InitData()
        {
            isFirstPickGiven = false;
            favorabilityLevel = 1;
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
                            shopItemDict[shopType][refreshType].itemList = new();
                        }
                    }
                }
            }
            shopItemDict[ShopType.Reroll].Add(ShopRefreshType.Common, new());
            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].refreshedTime = DateTime.MinValue;
            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].shopType = ShopType.Reroll;
            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].refreshType = ShopRefreshType.Common;
            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].itemList = new();
        }

        public void UpdateSavedData()
        {
            var goldShopPanelGO = GameMgr.FindObject($"GoldShopPanel");
            var diamondShopPanelGO = GameMgr.FindObject($"DiamondShopPanel");
            var rerollShopPanelGO = GameMgr.FindObject($"RerollShopPanel");
            if(goldShopPanelGO == null || diamondShopPanelGO == null || rerollShopPanelGO == null)
            {
                Debug.Log($"ShopItemData Update Failed, panelGO null");
                return;
            }
            var goldShopController = goldShopPanelGO.GetComponent<GoldShopController>();
            var diamondShopController = diamondShopPanelGO.GetComponent<DiamondShopController>();
            var rerollShopController = rerollShopPanelGO.GetComponent<RerollShopController>();
            if(goldShopController == null || diamondShopController == null || rerollShopController == null)
            {
                Debug.Log($"ShopItemData Update Failed, shop controller null");
                return;
            }

            foreach (ShopRefreshType refreshType in Enum.GetValues(typeof(ShopRefreshType)))
            {
                if (goldShopController.GetRefreshedTime(refreshType) != null)
                {
                    shopItemDict[ShopType.Gold][refreshType].refreshedTime = goldShopController.GetRefreshedTime(refreshType).Value;
                }
                else
                {
                    shopItemDict[ShopType.Gold][refreshType].refreshedTime = DateTime.MinValue;
                }
                shopItemDict[ShopType.Gold][refreshType].shopType = ShopType.Gold;
                shopItemDict[ShopType.Gold][refreshType].refreshType = refreshType;
                shopItemDict[ShopType.Gold][refreshType].itemList = goldShopController.GetSavedShopItemList(refreshType);


                if (diamondShopController.GetRefreshedTime(refreshType) != null)
                {                    
                    shopItemDict[ShopType.Diamond][refreshType].refreshedTime = diamondShopController.GetRefreshedTime(refreshType).Value;
                }
                else
                {
                    shopItemDict[ShopType.Diamond][refreshType].refreshedTime = DateTime.MinValue;
                }
                shopItemDict[ShopType.Diamond][refreshType].shopType = ShopType.Diamond;
                shopItemDict[ShopType.Diamond][refreshType].refreshType = refreshType;
                shopItemDict[ShopType.Diamond][refreshType].itemList = diamondShopController.GetSavedShopItemList(refreshType);
            }

            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].refreshedTime = rerollShopController.GetRefreshedTime().Value;
            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].shopType = ShopType.Reroll;
            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].refreshType = ShopRefreshType.Common;
            shopItemDict[ShopType.Reroll][ShopRefreshType.Common].itemList = rerollShopController.GetSavedShopItemList();
        }

        public void ApplySavedData()
        {
            var goldShopPanelGO = GameMgr.FindObject($"GoldShopPanel");
            var diamondShopPanelGO = GameMgr.FindObject($"DiamondShopPanel");
            var rerollShopPanelGO = GameMgr.FindObject($"RerollShopPanel");
            if (goldShopPanelGO == null || diamondShopPanelGO == null || rerollShopPanelGO == null)
            {
                Debug.Log($"ShopItemData Apply Failed, panelGO null");
                return;
            }
            var goldShopController = goldShopPanelGO.GetComponent<GoldShopController>();
            var diamondShopController = diamondShopPanelGO.GetComponent<DiamondShopController>();
            var rerollShopController = rerollShopPanelGO.GetComponent<RerollShopController>();
            if (goldShopController == null || diamondShopController == null || rerollShopController == null)
            {
                Debug.Log($"ShopItemData Apply Failed, shop controller null");
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

        public List<SavedShopItem> GetSavedShopItemList(ShopType shopType, ShopRefreshType refreshType = ShopRefreshType.Common)
        {
            return shopItemDict[shopType][refreshType].itemList;
        }

        public List<ItemSlotData> GetItemSlotDataList(ShopType shopType, ShopRefreshType refreshType = ShopRefreshType.Common)
        {
            var result = new List<ItemSlotData>();
            foreach (var savedItem in shopItemDict[shopType][refreshType].itemList)
            {
                var newSlotData = new ItemSlotData();
                newSlotData.id = savedItem.id;
                newSlotData.locked = savedItem.isLocked;
                newSlotData.refreshType = refreshType;
                newSlotData.shopType = shopType;
                newSlotData.itemType = savedItem.itemType;
                newSlotData.currCount = savedItem.currentCount;
                newSlotData.maxCount = savedItem.maxCount;
                newSlotData.currencyType = savedItem.currencyType;
                newSlotData.price = savedItem.price;
                result.Add(newSlotData);
            }
            return result;
        }
    } // Scope by class SavedShopItemsData

} // namespace Root