using SkyDragonHunter.test;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.test
{

    // 상점 종류를 나타내는 열거형 정의 (일반 / 일일 / 주간 / 월간)
    public enum ShopCategory { Common, Daily, Weekly, Monthly }

    public class DiamondShopController : MonoBehaviour
    {
        [Header("UI 참조")]
        [SerializeField] private Transform contentParent;          // 슬롯들이 배치된 Content 오브젝트
        [SerializeField] private List<ItemStatus> shopItemPool;    // 전체 아이템 풀

        [SerializeField] private ShopCategory currentCategory;     // 현재 탭 카테고리

        private Dictionary<ShopCategory, List<ItemStatus>> categoryItems = new();  // 탭별 아이템 데이터 저장소
        private List<ShopSlotHandler> slotHandlers = new();                        // 슬롯 UI 핸들러 목록

        [Header("탭별 갱신 시간 (초)")]
        [SerializeField] private float dailyResetTime = 86400f;
        [SerializeField] private float weeklyResetTime = 604800f;
        [SerializeField] private float monthlyResetTime = 2592000f;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            // Content 아래의 모든 슬롯에 있는 ShopSlotHandler 수집
            for (int i = 0; i < contentParent.childCount; i++)
            {
                var handler = contentParent.GetChild(i).GetComponent<ShopSlotHandler>();
                if (handler != null)
                    slotHandlers.Add(handler);
            }

            // 각 카테고리별로 빈 리스트 준비
            foreach (ShopCategory cat in System.Enum.GetValues(typeof(ShopCategory)))
                categoryItems[cat] = new List<ItemStatus>();
        }

        private void OnEnable()
        {
            RefreshCategory(currentCategory); // 창이 켜질 때 현재 탭 갱신
        }
        // Public 메서드
        // 특정 카테고리를 기준으로 슬롯에 아이템 갱신
        // 탭 갱신 시 호출
        public void RefreshCategory(ShopCategory category)
        {
            currentCategory = category;

            // 저장된 아이템이 없으면 랜덤 생성
            if (categoryItems[category].Count == 0)
                GenerateRandomItemsFor(category);

            // 슬롯 UI에 적용
            ApplyItemsToSlots(categoryItems[category]);
        }


        // 외부 탭 버튼에서 호출되도록 만든 함수 (Button에서 OnClick 연결용)
        public void OnClickTab(int categoryIndex)
        {
            RefreshCategory((ShopCategory)categoryIndex);
        }

        // Private 메서드

        // 카테고리별로 랜덤하게 아이템을 골라 저장하고 리셋 예약
        // 카테고리에 랜덤 아이템 설정
        private void GenerateRandomItemsFor(ShopCategory category)
        {
            var randomItems = shopItemPool.OrderBy(x => Random.value).Take(slotHandlers.Count).ToList();
            categoryItems[category] = randomItems;

            // 자동 리셋 예약
            switch (category)
            {
                case ShopCategory.Daily:
                    StartCoroutine(ResetCategoryAfterDelay(category, dailyResetTime));
                    break;
                case ShopCategory.Weekly:
                    StartCoroutine(ResetCategoryAfterDelay(category, weeklyResetTime));
                    break;
                case ShopCategory.Monthly:
                    StartCoroutine(ResetCategoryAfterDelay(category, monthlyResetTime));
                    break;
            }
        }

        // 일정 시간 후 해당 카테고리의 아이템 초기화
        private IEnumerator ResetCategoryAfterDelay(ShopCategory category, float delay)
        {
            yield return new WaitForSeconds(delay);
            categoryItems[category].Clear();
        }

        // 슬롯 UI에 아이템 적용
        private void ApplyItemsToSlots(List<ItemStatus> items)
        {
            for (int i = 0; i < slotHandlers.Count; i++)
            {
                slotHandlers[i].Initialize(items[Random.Range(0, items.Count - 1)]);
            }
        }


        // Others

    } // Scope by class DiamondShopController

} // namespace Root