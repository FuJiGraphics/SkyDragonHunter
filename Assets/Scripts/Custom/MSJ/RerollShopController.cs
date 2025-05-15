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
    public class RerollShopController : MonoBehaviour
    {
        // Fields
        private bool initiated = false;
        private int currentFavorabilityLevel;

        [Header("UI 참조")]
        [SerializeField] private FavorailityMgr favorailityMgr;              // 16개의 아이템 슬롯이 배치된 Content 오브젝트
        [SerializeField] private Transform contentParent;              // 16개의 아이템 슬롯이 배치된 Content 오브젝트

        [SerializeField] private UIShopItemSlot prefab;
        [SerializeField] private List<GameObject> slotList = new();
        [SerializeField] private Button rerollButton;

        public DateTime resetTime;
        private const int maxItemCount = 6;
        private static readonly int resetTimeHourlyCriterion = 1;
        
        private List<ItemSlotData> itemDataList = new();

        [SerializeField] private Button closeButton;

        // Unity Methods
        private void Awake()
        {
            currentFavorabilityLevel = 1;
        }

        private void Start()
        {
            closeButton.onClick.AddListener(OnClickCloseButton);
            currentFavorabilityLevel = SaveLoadMgr.GameData.savedShopItemData.favorabilityLevel;
            resetTime = SaveLoadMgr.GameData.savedShopItemData.GetRefreshedTime(ShopType.Reroll, ShopRefreshType.Common).Value;
            if(resetTime == DateTime.MinValue)
            {
                SetSlotData(currentFavorabilityLevel);
            }
            else if (GetElapsedTime() > TimeSpan.FromHours(resetTimeHourlyCriterion))
            {
                SetSlotData(currentFavorabilityLevel);
            }
            else
            {
                LoadSlotData();
            }

            rerollButton.onClick.AddListener(OnClickRerollButton);

            initiated = true;
            SetSlot();
        }

        private void OnEnable()
        {
            if (initiated)
                SetSlot();
        }

        // Public Methods
        public List<ItemSlotData> GetItemSlotDataList(ShopRefreshType refreshType)
        {
            return itemDataList;
        }

        public List<SavedShopItem> GetSavedShopItemList()
        {
            var result = new List<SavedShopItem>();
            foreach (var slot in itemDataList)
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
        public DateTime? GetRefreshedTime()
        {
            return resetTime;
        }
        public TimeSpan GetElapsedTime()
        {
            var elapsed = DateTime.UtcNow - resetTime;
           
            return elapsed;
        }

        // Private Methods
        private void OnClickCloseButton()
        {
            gameObject.SetActive(false);
        }
        private void SetSlot()
        {
            foreach (var slotGO in slotList)
            {
                Destroy(slotGO);
            }
            slotList.Clear();

            for (int i = 0; i < itemDataList.Count; ++i)
            {
                var slot = Instantiate(prefab, contentParent);
                slot.SetSlot(itemDataList[i]);

                var capturedIndex = i;
                var item = itemDataList[capturedIndex];

                slot.AddListener(() =>
                {
                    if (AccountMgr.Diamond >= item.price)
                    {
                        var itemCount = AccountMgr.ItemCount(item.itemType);
                        itemCount += item.maxCount;
                        AccountMgr.SetItemCount(item.itemType, itemCount);
                        item.currCount = 0;
                        AccountMgr.Diamond -= item.price;
                        item.locked = false;
                        DrawableMgr.Dialog("안내", $"{item.ItemName} {item.maxCount}개 구매 성공");
                    }
                    else
                    {
                        DrawableMgr.Dialog("안내", "다이아가 부족합니다");
                    }                                     
                });
                slot.AddLockButtonListener(() =>
                {
                    if (item.currCount > 0)
                    {
                        item.locked = !item.locked;
                    }
                    else
                    {
                        item.locked = false;
                    }
                });
                slotList.Add(slot.gameObject);
            }
        }

        private void SetSlotData(int favorabilityLv)
        {
            ItemSlotData[] lockedSlots = new ItemSlotData[maxItemCount];
            
            for (int i = 0; i < itemDataList.Count; ++i)
            {
                lockedSlots[i] = null;
                if (itemDataList[i].locked)
                {
                    lockedSlots[i] = new ItemSlotData();
                    lockedSlots[i].id = itemDataList[i].id;
                    lockedSlots[i].index = itemDataList[i].index;
                    lockedSlots[i].refreshType = itemDataList[i].refreshType;
                    lockedSlots[i].locked = true;
                    lockedSlots[i].shopType = itemDataList[i].shopType;
                    lockedSlots[i].itemType = itemDataList[i].itemType;
                    lockedSlots[i].pullRate = itemDataList[i].pullRate;
                    lockedSlots[i].currCount = itemDataList[i].currCount;
                    lockedSlots[i].maxCount = itemDataList[i].maxCount;
                    lockedSlots[i].currencyType = itemDataList[i].currencyType;
                }
            }

            itemDataList.Clear();

            for (int i = 0; i < maxItemCount; ++i)
            {
                if (lockedSlots[i] == null)
                {
                    var randId = DataTableMgr.RerollShopTable.GetWeightedRandomItemID(favorabilityLv);
                    var rerollShopData = DataTableMgr.RerollShopTable.Get(randId);
                    ItemSlotData slotData = new ItemSlotData();
                    slotData.id = randId;
                    slotData.index = i;
                    slotData.shopType = ShopType.Reroll;
                    slotData.refreshType = ShopRefreshType.Common;
                    slotData.locked = false;
                    slotData.itemType = rerollShopData.GetItemType();
                    slotData.maxCount = rerollShopData.ItemAmount;
                    slotData.currCount = 1;
                    slotData.currencyType = CurrencyType.Diamond;
                    slotData.price = rerollShopData.Price;
                    itemDataList.Add(slotData);
                }
                else
                {
                    ItemSlotData slotData = new ItemSlotData();
                    slotData.id = lockedSlots[i].id;
                    slotData.index = i;
                    slotData.shopType = ShopType.Reroll;
                    slotData.refreshType = ShopRefreshType.Common;
                    slotData.locked = true;
                    slotData.itemType = lockedSlots[i].itemType;
                    slotData.maxCount = lockedSlots[i].maxCount;
                    slotData.currCount = lockedSlots[i].currCount;
                    slotData.currencyType = CurrencyType.Diamond;
                    slotData.price = lockedSlots[i].price;
                    itemDataList.Add(slotData);
                }
            }
            resetTime = DateTime.UtcNow;
            SaveLoadMgr.CallSaveGameData();
        }

        private void LoadSlotData()
        {
            itemDataList = SaveLoadMgr.GameData.savedShopItemData.GetItemSlotDataList(ShopType.Reroll);
        }

        private void OnClickRerollButton()
        {
            SetSlotData(currentFavorabilityLevel);
            SetSlot();
        }
    } // Scope by class DiamondShopController

} // namespace Root