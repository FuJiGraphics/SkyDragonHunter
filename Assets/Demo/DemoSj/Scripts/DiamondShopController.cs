using SixLabors.ImageSharp.PixelFormats;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.test;
using SkyDragonHunter.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    // 상점 종류를 나타내는 열거형 정의 (일반 / 일일 / 주간 / 월간)
    public enum DiamondShopCategory { Common, Daily, Weekly, Monthly }

    public class DiamondShopController : MonoBehaviour
    {
        public ShopType shopType;

        [Header("UI 참조")]
        [SerializeField] private FavorailityMgr favorailityMgr;              // 16개의 아이템 슬롯이 배치된 Content 오브젝트
        [SerializeField] private Transform contentParent;              // 16개의 아이템 슬롯이 배치된 Content 오브젝트
        [SerializeField] private List<ItemSlotData> diamondShopItemPool;       // 전체 아이템 풀 (출현 확률 포함)

        [SerializeField] private DiamondShopCategory currentCategory;         // 현재 활성화된 상점 탭 종류

        [SerializeField] private ScrollRect scrollRect; // (추가) 스크롤뷰

        private List<ShopSlotHandler> slotHandlers = new();            // UI 슬롯 핸들러 리스트

        // 각 카테고리별로 아이템 상태(아이템 + 현재 수량)를 저장하는 리스트
        private Dictionary<DiamondShopCategory, List<ShopSlotState>> categoryItems = new();


        // 각 카테고리별 자동 갱신을 위한 코루틴 추적용
        private Dictionary<DiamondShopCategory, Coroutine> resetRoutines = new();

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
            RefreshCategory(currentCategory);
        }

        public void Init()
        {
            if (diamondShopItemPool != null && diamondShopItemPool.Count > 0)
                return;

            if (diamondShopItemPool == null)
                diamondShopItemPool = new();

            var tableData = DataTableMgr.DiamondShopTable.ToArray();
            foreach (var data in tableData)
            {
                ItemSlotData slot = new ItemSlotData();
                slot.itemType = data.GetItemType();
                slot.currCount = data.ItemAmount;
                slot.maxCount = data.ItemBuyLimitCount;
                slot.currencyType = CurrencyType.Coin;
                slot.pullRate = data.GenWeight;
                slot.Price = data.Price;
                diamondShopItemPool.Add(slot);
            }

            // Content 하위의 자식 오브젝트에서 슬롯 핸들러 수집
            for (int i = 0; i < contentParent.childCount; i++)
            {
                var handler = contentParent.GetChild(i).GetComponent<ShopSlotHandler>();
                if (handler != null)
                    slotHandlers.Add(handler);
            }

            // 각 카테고리에 대해 빈 리스트 초기화
            foreach (DiamondShopCategory cat in System.Enum.GetValues(typeof(DiamondShopCategory)))
                categoryItems[cat] = new List<ShopSlotState>();
        }

        // Public 메서드

        // 외부 탭 버튼에서 호출되는 메서드
        public void OnClickTab(int categoryIndex)
        {
            RefreshCategory((DiamondShopCategory)categoryIndex);
        }

        // 주어진 카테고리로 아이템을 갱신
        public void RefreshCategory(DiamondShopCategory category)
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
        }

        // Private 메서드

        private void ResetScrollPosition()
        {
            // 캔버스 강제 갱신 후 최상단으로 이동
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        // 카테고리에 대해 출현 확률 기반으로 아이템을 선택하고 저장
        private void GenerateRandomItemsFor(DiamondShopCategory category)
        {
            List<ShopSlotState> result = new();

            for (int i = 0; i < slotHandlers.Count; i++)
            {
                var selectedItem = GetWeightedRandomItem(diamondShopItemPool);
                result.Add(new ShopSlotState((ItemSlotData)selectedItem));
            }

            categoryItems[category] = result;

            // [수정 포인트] 일반(Common) 탭은 리셋 코루틴을 실행하지 않음
            if (category == DiamondShopCategory.Common)
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
            float rand = Random.Range(0f, totalWeight);            // 랜덤 값 선택
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
        private float GetDelay(DiamondShopCategory category)
        {
            return category switch
            {
                DiamondShopCategory.Daily => dailyResetTime,
                DiamondShopCategory.Weekly => weeklyResetTime,
                DiamondShopCategory.Monthly => monthlyResetTime,
                _ => 0f
            };
        }

        // 일정 시간이 지나면 해당 카테고리의 아이템 초기화
        private IEnumerator ResetCategoryAfterDelay(DiamondShopCategory category, float delay)
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