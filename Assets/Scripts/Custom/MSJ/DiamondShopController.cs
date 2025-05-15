using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{    
    
    public class DiamondShopController : MonoBehaviour
    {

        // Fields
        private bool initiated = false;
        private ShopRefreshType currentRefreshType;         // 현재 활성화된 상점 탭 종류

        [Header("UI 참조")]
        [SerializeField] private FavorailityMgr favorailityMgr;              // 16개의 아이템 슬롯이 배치된 Content 오브젝트
        [SerializeField] private Transform contentParent;              // 16개의 아이템 슬롯이 배치된 Content 오브젝트

        [SerializeField] private UIShopItemSlot prefab;
        [SerializeField] private List<GameObject> slotList = new();
        public DateTime?[] resetTime;
        private const int maxItemCount = 16;
        private static readonly int[] resetTimeHourlyCriteria =
        {
            99999999,
            24,
            168,
            720
        };
        private Dictionary<ShopRefreshType, List<ItemSlotData>> itemDataListDict = new();

        // Unity Methods
        private void Start()
        {
            currentRefreshType = ShopRefreshType.Common;
            resetTime = new DateTime?[Enum.GetValues(typeof(ShopRefreshType)).Length];
            for (int i = 0; i < resetTime.Length; ++i)
            {
                resetTime[i] = SaveLoadMgr.GameData.savedShopItemData.GetRefreshedTime(ShopType.Diamond, (ShopRefreshType)i);
            }
            foreach (ShopRefreshType refreshType in Enum.GetValues(typeof(ShopRefreshType)))
            {
                if (resetTime[(int)refreshType] == null)
                {
                    SetItemSlotData(refreshType);
                    Debug.LogError($"previous reset time was null");
                }
                else if (resetTime[(int)refreshType] == DateTime.MinValue)
                {
                    SetItemSlotData(refreshType);
                }
                else if (GetElapsedTime(refreshType) > TimeSpan.FromHours(resetTimeHourlyCriteria[(int)refreshType]))
                {
                    SetItemSlotData(refreshType);
                }
                else
                {
                    LoadSlotData(refreshType);
                }
            }
            initiated = true;
            SetSlotForCategory(currentRefreshType);
        }

        private void OnEnable()
        {
            if (initiated)
                SetSlotForCategory(currentRefreshType);
        }

        // Public Methods
        // 외부 탭 버튼에서 호출되는 메서드
        public void OnClickTab(int categoryIndex)
        {
            SetSlotForCategory((ShopRefreshType)categoryIndex);
        }

        public List<ItemSlotData> GetItemSlotDataList(ShopRefreshType refreshType)
        {
            return itemDataListDict[refreshType];
        }

        public List<SavedShopItem> GetSavedShopItem(ShopRefreshType refreshType)
        {
            var result = new List<SavedShopItem>();
            foreach(var slotData in itemDataListDict[refreshType])
            {
                var saveItem = new SavedShopItem();
                saveItem.id = slotData.id;
                saveItem.itemType = slotData.itemType;
                saveItem.currentCount = slotData.currCount;
                saveItem.maxCount = slotData.maxCount;
                saveItem.price = slotData.price;
                saveItem.isLocked = slotData.locked;
                result.Add(saveItem);
            }
            return result;
        }

        public List<SavedShopItem> GetSavedShopItemList(ShopRefreshType refreshType)
        {
            if (!itemDataListDict.ContainsKey(refreshType))
            {
                itemDataListDict.Add(refreshType, new());
            }
            var result = new List<SavedShopItem>();
            foreach (var slot in itemDataListDict[refreshType])
            {
                var newSavedItem = new SavedShopItem();
                newSavedItem.id = slot.id;
                newSavedItem.isLocked = slot.locked;
                newSavedItem.itemType = slot.itemType;
                newSavedItem.currentCount = slot.currCount;
                newSavedItem.maxCount = slot.maxCount;
                newSavedItem.price = slot.price;
                newSavedItem.currencyType = slot.currencyType;
                result.Add(newSavedItem);
            }
            return result;
        }

        public DateTime? GetRefreshedTime(ShopRefreshType refreshType)
        {
            if (resetTime == null)
            {
                return null;
            }
            return resetTime[(int)refreshType];
        }

        public TimeSpan GetElapsedTime(ShopRefreshType refreshType)
        {
            var elapsed = DateTime.UtcNow - resetTime[(int)refreshType];
            if (elapsed == null)
            {
                return TimeSpan.MaxValue;
            }
            return elapsed.Value;
        }

        // Private Methods
        private void SetSlotForCategory(ShopRefreshType refreshType)
        {
            foreach (var slotGO in slotList)
            {
                Destroy(slotGO);
            }
            slotList.Clear();

            for (int i = 0; i < itemDataListDict[refreshType].Count; ++i)
            {
                var slot = Instantiate(prefab, contentParent);
                slot.SetSlot(itemDataListDict[refreshType][i]);

                var capturedType = refreshType;
                var capturedIndex = i;

                slot.AddListener(() =>
                {
                    var item = itemDataListDict[capturedType][capturedIndex];
                    if (capturedType == ShopRefreshType.Common)
                    {
                        if (AccountMgr.Diamond >= item.price)
                        {
                            var itemCount = AccountMgr.ItemCount(item.itemType);
                            itemCount += 1;
                            AccountMgr.SetItemCount(item.itemType, itemCount);
                            AccountMgr.Diamond -= item.price;
                            DrawableMgr.Dialog("안내", $"{item.ItemName} 구매 성공");
                        }
                        else
                        {
                            DrawableMgr.Dialog("안내", "다이아가 부족합니다");
                        }
                    }
                    else
                    {
                        if (AccountMgr.Diamond >= item.price && item.currCount > 0)
                        {
                            var itemCount = AccountMgr.ItemCount(item.itemType);
                            itemCount += 1;
                            item.currCount -= 1;
                            AccountMgr.SetItemCount(item.itemType, itemCount);
                            AccountMgr.Diamond -= item.price;
                            DrawableMgr.Dialog("안내", $"{item.ItemName} 구매 성공");
                        }
                        else if (!(item.currCount > 0))
                        {
                            DrawableMgr.Dialog("안내", "아이템 재고가 없습니다");
                        }
                        else if (AccountMgr.Diamond < item.price)
                        {
                            DrawableMgr.Dialog("안내", "다이아가 부족합니다");
                        }
                    }
                });
                slotList.Add(slot.gameObject);
            }
            currentRefreshType = refreshType;
        }

        private void LoadSlotData(ShopRefreshType refreshType)
        {
            if (itemDataListDict.ContainsKey(refreshType))
                itemDataListDict[refreshType].Clear();
            else
                itemDataListDict.Add(refreshType, new());

            itemDataListDict[refreshType] = SaveLoadMgr.GameData.savedShopItemData.GetItemSlotDataList(ShopType.Gold, refreshType);
        }

        private void SetItemSlotData(ShopRefreshType refreshType)
        {
            if (itemDataListDict.ContainsKey(refreshType))
                itemDataListDict[refreshType].Clear();
            else
                itemDataListDict.Add(refreshType, new());

            if (refreshType == ShopRefreshType.Common)
            {
                SetCommonShopSlotData();
                return;
            }

            for (int i = 0; i < maxItemCount; ++i)
            {
                var randId = DataTableMgr.DiamondShopTable.GetWeightedRandomItemID(refreshType);
                var diaShopData = DataTableMgr.DiamondShopTable.Get(randId);
                ItemSlotData slotData = new ItemSlotData();
                slotData.id = randId;
                slotData.index = i;
                slotData.shopType = ShopType.Diamond;
                slotData.refreshType = refreshType;

                slotData.itemType = diaShopData.GetItemType();
                slotData.maxCount = diaShopData.ItemBuyLimit;
                slotData.currCount = slotData.maxCount;
                slotData.currencyType = CurrencyType.Diamond;
                slotData.price = diaShopData.Price;
                itemDataListDict[refreshType].Add(slotData);
            }

            resetTime[(int)refreshType] = DateTime.UtcNow;
            SaveLoadMgr.CallSaveGameData();
        }

        private void SetCommonShopSlotData()
        {
            // List of Items in common shop stay same
            var commonItems = DataTableMgr.DiamondShopTable.GetCategorizedItemList(ShopRefreshType.Common);

            int index = 0;
            foreach (var diaShopData in commonItems)
            {
                ItemSlotData slotData = new ItemSlotData();
                slotData.id = diaShopData.ID;
                slotData.index = index;
                slotData.shopType = ShopType.Diamond;
                slotData.refreshType = ShopRefreshType.Common;

                slotData.itemType = diaShopData.GetItemType();
                slotData.maxCount = diaShopData.ItemBuyLimit;
                slotData.currCount = slotData.maxCount;
                slotData.currencyType = CurrencyType.Diamond;
                slotData.price = diaShopData.Price;
                itemDataListDict[ShopRefreshType.Common].Add(slotData);
                index++;
            }

            resetTime[(int)ShopRefreshType.Common] = DateTime.UtcNow;
            SaveLoadMgr.CallSaveGameData();
        }
    } // Scope by class DiamondShopController

} // namespace Root