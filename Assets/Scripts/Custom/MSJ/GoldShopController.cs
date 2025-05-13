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
        // TODO: LJH
        public int id;
        public int index;
        public ShopRefreshType refreshType;
        // ~TODO
        [SerializeField] public ShopType shopType;
        [SerializeField] public ItemType itemType;
        [SerializeField] public float pullRate;
        [SerializeField] public int currCount;
        [SerializeField] public int maxCount;
        [SerializeField] public CurrencyType currencyType;

        public bool Purchasable
        {
            get
            {
                if (shopType != ShopType.Reroll && refreshType == ShopRefreshType.Common)
                    return true;

                return currCount > 0;
            }
        }
        public BigNum Price { get; set; }
        public string ItemName => GetData().Name;
        public Sprite ItemIcon => GetData().Icon;
        public Sprite CurrencyIcon => currencyType == CurrencyType.Coin ?
            DataTableMgr.ItemTable.Get(ItemType.Coin).Icon : DataTableMgr.ItemTable.Get(ItemType.Diamond).Icon;

        public ItemData GetData() => DataTableMgr.ItemTable.Get(itemType);
    }

    public class GoldShopController : MonoBehaviour
    {
        public ShopType shopType;

        [Header("UI 참조")]
        [SerializeField] private FavorailityMgr favorailityMgr;              // 16개의 아이템 슬롯이 배치된 Content 오브젝트
        [SerializeField] private Transform contentParent;              // 16개의 아이템 슬롯이 배치된 Content 오브젝트
        [SerializeField] private List<ItemSlotData> goldShopItemPool;       // 전체 아이템 풀 (출현 확률 포함)

        [SerializeField] private ShopRefreshType currentCategory;         // 현재 활성화된 상점 탭 종류

        [SerializeField] private ScrollRect scrollRect; // (추가) 스크롤뷰

        private List<ShopSlotHandler> slotHandlers = new();            // UI 슬롯 핸들러 리스트

        // 각 카테고리별로 아이템 상태(아이템 + 현재 수량)를 저장하는 리스트
        private Dictionary<ShopRefreshType, List<ShopSlotState>> categoryItems = new();

        // TODO: LJH
        [SerializeField] private UIShopItemSlot prefab;
        [SerializeField] private List<GameObject> slotList = new();
        public DateTime?[] resetTime;
        private static readonly int[] resetTimeHourlyCriteria =
        {
            int.MaxValue,
            24,
            168,
            720
        };
        private Dictionary<ShopRefreshType, List<ItemSlotData>> itemDataListDict = new();
        private const int maxItemCount = 16;
        // ~TODO

                // 각 카테고리별 자동 갱신을 위한 코루틴 추적용
        private Dictionary<ShopRefreshType, Coroutine> resetRoutines = new();

        [Header("탭별 자동 갱신 시간 (초 단위)")]
        [SerializeField] private float dailyResetTime = 86400f;        // 일일: 24시간
        [SerializeField] private float weeklyResetTime = 604800f;      // 주간: 7일
        [SerializeField] private float monthlyResetTime = 2592000f;    // 월간: 30일

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // 시작 시 슬롯 핸들러 수집 및 카테고리 초기화
        // 상점 창이 켜질 때 현재 카테고리 기준으로 아이템 표시
                
        private void OnEnable()
        {
            // TODO: LJH
            //RefreshCategory(currentCategory);
            // ~TODO
        }

        // TODO: LJH
        private void Start()
        {            
            resetTime = new DateTime?[Enum.GetValues(typeof(ShopRefreshType)).Length];
            for (int i = 0; i < resetTime.Length; ++i)
            {
                resetTime[i] = SaveLoadMgr.GameData.savedShopItemData.GetRefreshedTime(ShopType.Gold, (ShopRefreshType)i);
            }
            foreach (ShopRefreshType category in Enum.GetValues(typeof(ShopRefreshType)))
            {
                if(resetTime[(int)category] == null)
                {
                    SetSlotDataOfRefreshType(category);
                    Debug.LogError($"previous reset time was null");
                }
                else if (resetTime[(int)category] == DateTime.MinValue)
                {
                    SetSlotDataOfRefreshType(category);
                }
                else if (resetTime[(int)category].Value.Hour + resetTimeHourlyCriteria[(int)category] > DateTime.UtcNow.Hour)
                {
                    SetSlotDataOfRefreshType(category);
                }
                else
                {
                    
                }
            }
        }
        // ~TODO

        // Public 메서드
        public void Init()
        {
            InitGoldShopItemPool();            

            // Content 하위의 자식 오브젝트에서 슬롯 핸들러 수집
            for (int i = 0; i < contentParent.childCount; i++)
            {
                var handler = contentParent.GetChild(i).GetComponent<ShopSlotHandler>();
                if (handler != null)
                    slotHandlers.Add(handler);
            }

            // 각 카테고리에 대해 빈 리스트 초기화
            foreach (ShopRefreshType cat in System.Enum.GetValues(typeof(ShopRefreshType)))
                categoryItems[cat] = new List<ShopSlotState>();
        }

        private void InitGoldShopItemPool()
        {
            if (goldShopItemPool != null && goldShopItemPool.Count > 0)
                return;

            if (goldShopItemPool == null)
                goldShopItemPool = new();

            var tableData = DataTableMgr.GoldShopTable.ToArray();
            foreach (var data in tableData)
            {
                ItemSlotData slot = new ItemSlotData();
                slot.itemType = data.GetItemType();
                slot.currCount = data.ItemAmount;
                slot.maxCount = data.ItemBuyLimit;
                slot.currencyType = CurrencyType.Coin;
                slot.pullRate = data.GenWeight;
                slot.Price = data.Price;
                goldShopItemPool.Add(slot);
            }
        }

        // 외부 탭 버튼에서 호출되는 메서드
        public void OnClickTab(int categoryIndex)
        {
            // TODO: LJH
            //RefreshCategory((ShopRefreshType)categoryIndex);
            SetSlotForCategory((ShopRefreshType)categoryIndex);
            // ~TODO
        }

        // 주어진 카테고리로 아이템을 갱신
        public void RefreshCategory(ShopRefreshType category)
        {
            currentCategory = category;

            // [수정된 조건] 일반(Common)은 최초 1회만 랜덤 배치
            bool needGenerate = categoryItems[category].Count == 0;

            if (needGenerate)
            {
                Debug.Log($"[{category}] 새로 랜덤 배치됨");
                GenerateRandomItemsFor(category);
            }
            else
            {
                Debug.Log($"[{category}] 기존 아이템 재사용");
            }

            ApplyItemsToSlots(categoryItems[category]);
            ResetScrollPosition();

            // TODO: LJH
            resetTime[(int)category] = DateTime.UtcNow;
            // ~TODO
        }

        // TODO: LJH
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
                        if (AccountMgr.Coin >= item.Price)
                        {
                            var itemCount = AccountMgr.ItemCount(item.itemType);
                            itemCount += 1;
                            AccountMgr.SetItemCount(item.itemType, itemCount);
                            AccountMgr.Coin -= item.Price;
                            DrawableMgr.Dialog("안내", $"{item.ItemName} 구매 성공");
                        }
                        else
                        {
                            DrawableMgr.Dialog("안내", "골드가 부족합니다");
                        }
                    }
                    else
                    {
                        if (AccountMgr.Coin >= item.Price && item.currCount > 0)
                        {
                            var itemCount = AccountMgr.ItemCount(item.itemType);
                            itemCount += 1;
                            item.currCount -= 1;
                            AccountMgr.SetItemCount(item.itemType, itemCount);
                            AccountMgr.Coin -= item.Price;
                            DrawableMgr.Dialog("안내", $"{item.ItemName} 구매 성공");
                        }
                        else if (!(item.currCount > 0))
                        {
                            DrawableMgr.Dialog("안내", "아이템 재고가 없습니다");
                        }
                        else if (AccountMgr.Coin < item.Price)
                        {
                            DrawableMgr.Dialog("안내", "골드가 부족합니다");
                        }
                    }
                });
                slotList.Add(slot.gameObject);
            }
        }

        private void SetSlotDataOfRefreshType(ShopRefreshType refreshType)
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
                slotData.Price = goldShopData.Price;
                itemDataListDict[refreshType].Add(slotData);
            }

            resetTime[(int)refreshType] = DateTime.UtcNow;
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
                slotData.Price = goldShopData.Price;
                itemDataListDict[ShopRefreshType.Common].Add(slotData);
                index++;
            }

            resetTime[(int)ShopRefreshType.Common] = DateTime.UtcNow;
        }
        // ~TODO

        // Private 메서드
        private void ResetScrollPosition()
        {
            // 캔버스 강제 갱신 후 최상단으로 이동
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        // 카테고리에 대해 출현 확률 기반으로 아이템을 선택하고 저장
        private void GenerateRandomItemsFor(ShopRefreshType category)
        {
            List<ShopSlotState> result = new();

            for (int i = 0; i < slotHandlers.Count; i++)
            {
                var selectedItem = GetWeightedRandomItem(goldShopItemPool);
                result.Add(new ShopSlotState((ItemSlotData)selectedItem));
            }

            categoryItems[category] = result;

            // [수정 포인트] 일반(Common) 탭은 리셋 코루틴을 실행하지 않음
            if (category == ShopRefreshType.Common)
                return;

            // 기존 리셋 코루틴 정리
            if (resetRoutines.ContainsKey(category))
                StopCoroutine(resetRoutines[category]);

            // 리셋 타이머 실행
            resetRoutines[category] = StartCoroutine(ResetCategoryAfterDelay(category, GetDelay(category)));
        }

        // 출현 확률(pullRate)을 기반으로 하나의 아이템을 선택
        private ItemSlotData? GetWeightedRandomItem(List<ItemSlotData> pool)
        {
            var valid = pool.ToList();
            if (valid.Count == 0)
            {
                Debug.LogError("GetWeightedRandomItem: 유효한 아이템이 없습니다.");
                return null;
            }

            float totalWeight = valid.Sum(item => item.pullRate); // 전체 가중치 합
            float rand = UnityEngine.Random.Range(0f, totalWeight);            // 랜덤 값 선택
            float cumulative = 0f;

            foreach (var item in valid)
            {
                cumulative += item.pullRate;
                if (rand <= cumulative)
                    return item;
            }

            return valid.Last(); // fallback
        }

        // 각 카테고리에 대해 설정된 갱신 시간 반환
        private float GetDelay(ShopRefreshType category)
        {
            return category switch
            {
                ShopRefreshType.Daily => dailyResetTime,
                ShopRefreshType.Weekly => weeklyResetTime,
                ShopRefreshType.Monthly => monthlyResetTime,
                _ => 0f
            };
        }

        // 일정 시간이 지나면 해당 카테고리의 아이템 초기화
        private IEnumerator ResetCategoryAfterDelay(ShopRefreshType category, float delay)
        {
            yield return new WaitForSeconds(delay);
            categoryItems[category].Clear();
        }

        // 슬롯 UI에 저장된 아이템 상태 리스트를 순서대로 반영
        private void ApplyItemsToSlots(List<ShopSlotState> items)
        {
            for (int i = 0; i < slotHandlers.Count; i++)
            {
                slotHandlers[i].Initialize(items[i], favorailityMgr, shopType);
            }
        }


        // Others

    } // Scope by class DiamondShopController

} // namespace Root