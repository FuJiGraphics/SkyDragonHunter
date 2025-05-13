using SkyDragonHunter.Managers;
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

        private void Awake()
        {
            currentFavorabilityLevel = 1;
        }

        private void Start()
        {
            currentFavorabilityLevel = SaveLoadMgr.GameData.savedShopItemData.favorabilityLevel;
            resetTime = SaveLoadMgr.GameData.savedShopItemData.GetRefreshedTime(ShopType.Reroll, ShopRefreshType.Common).Value;
            if(resetTime == DateTime.MinValue)
            {
                SetSlotData(currentFavorabilityLevel);
            }
            else if (resetTime.Hour + resetTimeHourlyCriterion > DateTime.UtcNow.Hour)
            {
                SetSlotData(currentFavorabilityLevel);
            }
            else
            {

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
                    if (AccountMgr.Diamond >= item.Price)
                    {
                        var itemCount = AccountMgr.ItemCount(item.itemType);
                        itemCount += item.maxCount;
                        AccountMgr.SetItemCount(item.itemType, itemCount);
                        item.currCount = 0;
                        AccountMgr.Diamond -= item.Price;
                        DrawableMgr.Dialog("안내", $"{item.ItemName} {item.maxCount}개 구매 성공");
                    }
                    else
                    {
                        DrawableMgr.Dialog("안내", "다이아가 부족합니다");
                    }
                                     
                });
                slot.AddLockButtonListener(() =>
                {
                    item.locked = !item.locked;
                });
                slotList.Add(slot.gameObject);
            }
        }

        private void SetSlotData(int favorabilityLv)
        {
            List<ItemSlotData> lockedSlots = new List<ItemSlotData>();
            List<int> lockedSlotIndexes = new List<int>();
            for (int i = 0; i < itemDataList.Count; ++i)
            {
                if (itemDataList[i].locked)
                {
                    var lockedSlot = new ItemSlotData();
                    lockedSlot.id = itemDataList[i].id;
                    lockedSlot.index = itemDataList[i].index;
                    lockedSlot.refreshType = itemDataList[i].refreshType;
                    lockedSlot.locked = true;
                    lockedSlot.
                }
            }

            itemDataList.Clear();

            for (int i = 0; i < maxItemCount; ++i)
            {
                var randId = DataTableMgr.RerollShopTable.GetWeightedRandomItemID(favorabilityLv);
                var rerollShopData = DataTableMgr.RerollShopTable.Get(randId);
                ItemSlotData slotData = new ItemSlotData();
                slotData.id = randId;
                slotData.index = i;
                slotData.shopType = ShopType.Reroll;
                slotData.refreshType = ShopRefreshType.Common;

                slotData.itemType = rerollShopData.GetItemType();
                slotData.maxCount = rerollShopData.ItemAmount;
                slotData.currCount = 1;
                slotData.currencyType = CurrencyType.Diamond;
                slotData.Price = rerollShopData.Price;
                itemDataList.Add(slotData);
            }
            resetTime = DateTime.UtcNow;
        }        

        private void OnClickRerollButton()
        {

        }
    } // Scope by class DiamondShopController

} // namespace Root