using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    // 상점 종류를 나타내는 열거형 정의 (일반 / 일일 / 주간 / 월간)
    public enum ShopRefreshType { Common, Daily, Weekly, Monthly }

    // 골드 상점, 다이아 상점 중 어디 화폐인지 체크
    public enum CurrencyType
    {
        Coin, Diamond
    }

    [System.Serializable]
    public class ItemSlotData
    {
        
        public int id;
        public int index;
        public bool locked;
        
        public ShopRefreshType refreshType;
        public ShopType shopType;
        public ItemType itemType;
        public float pullRate;
        public int currCount;
        public int maxCount;
        public CurrencyType currencyType;
        public BigNum price = 0;

        public ItemSlotData Clone()
        {
            return new ItemSlotData
            {
                id = this.id,
                index = this.index,
                locked = this.locked,
                refreshType = this.refreshType,
                shopType = this.shopType,
                itemType = this.itemType,
                currCount = this.currCount,
                maxCount = this.maxCount,
                currencyType = this.currencyType,
                price = this.price,
            };
        }

        public bool Purchasable
        {
            get
            {
                if (shopType != ShopType.Reroll && refreshType == ShopRefreshType.Common)
                    return true;

                return currCount > 0;
            }
        }
        public string ItemName => GetData().Name;
        public Sprite ItemIcon => GetData().Icon;
        public Sprite CurrencyIcon => currencyType == CurrencyType.Coin ?
            DataTableMgr.ItemTable.Get(ItemType.Coin).Icon : DataTableMgr.ItemTable.Get(ItemType.Diamond).Icon;

        public ItemData GetData() => DataTableMgr.ItemTable.Get(itemType);
    }

    public class GoldShopController : MonoBehaviour
    {
        // Fields
        private bool initiated = false;

        [Header("UI 참조")]
        [SerializeField] private FavorailityMgr favorailityMgr;              // 16개의 아이템 슬롯이 배치된 Content 오브젝트
        [SerializeField] private Transform contentParent;              // 16개의 아이템 슬롯이 배치된 Content 오브젝트
       
        [SerializeField] private ShopRefreshType currentRefreshType;         // 현재 활성화된 상점 탭 종류

        [SerializeField] private UIShopItemSlot prefab;
        [SerializeField] private List<GameObject> slotList = new();
        public DateTime?[] resetTime;
        private static readonly int[] resetTimeHourlyCriteria =
        {
            99999999,
            24,
            168,
            720
        };
        private Dictionary<ShopRefreshType, List<ItemSlotData>> itemDataListDict = new();
        private const int maxItemCount = 16;

        [SerializeField] private Button closeButton;
        
        // Unity Methods
        private void OnEnable()
        {
            if(initiated)
                SetSlotForCategory(currentRefreshType);
        }

        private void Start()
        {
            closeButton.onClick.AddListener(OnClickCloseButton);
            currentRefreshType = ShopRefreshType.Common;
            resetTime = new DateTime?[Enum.GetValues(typeof(ShopRefreshType)).Length];
            for (int i = 0; i < resetTime.Length; ++i)
            {
                resetTime[i] = SaveLoadMgr.GameData.savedShopItemData.GetRefreshedTime(ShopType.Gold, (ShopRefreshType)i);
            }
            foreach (ShopRefreshType refreshType in Enum.GetValues(typeof(ShopRefreshType)))
            {
                if(resetTime[(int)refreshType] == null)
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
        
        // Public Methods
        public List<ItemSlotData> GetItemSlotDataList(ShopRefreshType refreshType)
        {
            return itemDataListDict[refreshType];
        }

        public List<SavedShopItem> GetSavedShopItemList(ShopRefreshType refreshType)
        {
            if(!itemDataListDict.ContainsKey(refreshType))
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
            if(resetTime == null)
            {
                return null;
            }

            return resetTime[(int)refreshType];
        }
        
        public TimeSpan GetElapsedTime(ShopRefreshType refreshType)
        {
            var elapsed = DateTime.UtcNow - resetTime[(int)refreshType];
            if(elapsed == null)
            {
                return TimeSpan.MaxValue;
            }
            return elapsed.Value;
        }

        // 외부 탭 버튼에서 호출되는 메서드
        public void OnClickTab(int categoryIndex)
        {
            SetSlotForCategory((ShopRefreshType)categoryIndex);
        }

        // Private Methods
        private void OnClickCloseButton()
        {
            gameObject.SetActive(false);
        }
        private void SetSlotForCategory(ShopRefreshType refreshType)
        {
            foreach(var slotGO in slotList)
            {
                Destroy(slotGO);
            }
            slotList.Clear();

            for(int i = 0; i < itemDataListDict[refreshType].Count; ++i)
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
                        if (AccountMgr.Coin >= item.price)
                        {
                            var itemCount = AccountMgr.ItemCount(item.itemType);
                            itemCount += 1;
                            AccountMgr.SetItemCount(item.itemType, itemCount);
                            AccountMgr.Coin -= item.price;
                            DrawableMgr.Dialog("안내", $"{item.ItemName} 구매 성공");
                        }
                        else
                        {
                            DrawableMgr.Dialog("안내", "골드가 부족합니다");
                        }
                    }
                    else
                    {
                        if (AccountMgr.Coin >= item.price && item.currCount > 0)
                        {
                            var itemCount = AccountMgr.ItemCount(item.itemType);
                            itemCount += 1;
                            item.currCount -= 1;
                            AccountMgr.SetItemCount(item.itemType, itemCount);
                            AccountMgr.Coin -= item.price;
                            DrawableMgr.Dialog("안내", $"{item.ItemName} 구매 성공");
                        }
                        else if (!(item.currCount > 0))
                        {
                            DrawableMgr.Dialog("안내", "아이템 재고가 없습니다");
                        }
                        else if (AccountMgr.Coin < item.price)
                        {
                            DrawableMgr.Dialog("안내", "골드가 부족합니다");
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
                var randId = DataTableMgr.GoldShopTable.GetWeightedRandomItemID(refreshType);
                var goldShopData = DataTableMgr.GoldShopTable.Get(randId);
                ItemSlotData slotData = new ItemSlotData();
                slotData.id = randId;
                slotData.index = i;
                slotData.shopType = ShopType.Gold;
                slotData.refreshType = refreshType;

                slotData.itemType = goldShopData.GetItemType();
                slotData.maxCount = goldShopData.ItemBuyLimit;
                slotData.currCount = slotData.maxCount;
                slotData.currencyType = CurrencyType.Coin;
                slotData.price = goldShopData.Price;
                itemDataListDict[refreshType].Add(slotData);
            }

            resetTime[(int)refreshType] = DateTime.UtcNow;
            SaveLoadMgr.CallSaveGameData();
        }

        private void SetCommonShopSlotData()
        {
            // List of Items in common shop stay same
            var commonItems = DataTableMgr.GoldShopTable.GetCategorizedItemList(ShopRefreshType.Common);

            int index = 0;
            foreach (var goldShopData in commonItems)
            {
                ItemSlotData slotData = new ItemSlotData();
                slotData.id = goldShopData.ID;
                slotData.index = index;
                slotData.shopType = ShopType.Gold;
                slotData.refreshType = ShopRefreshType.Common;

                slotData.itemType = goldShopData.GetItemType();
                slotData.maxCount = goldShopData.ItemBuyLimit;
                slotData.currCount = slotData.maxCount;
                slotData.currencyType = CurrencyType.Coin;
                slotData.price = goldShopData.Price;
                itemDataListDict[ShopRefreshType.Common].Add(slotData);
                index++;
            }

            resetTime[(int)ShopRefreshType.Common] = DateTime.UtcNow;
            SaveLoadMgr.CallSaveGameData();
        }
        // Others

    } // Scope by class DiamondShopController

} // namespace Root