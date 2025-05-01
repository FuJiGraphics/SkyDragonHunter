using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.UI {

    [System.Serializable]
    public class RerollShopSlotState
    {
        public ItemSlotData item;
        public bool isLocked;
        public bool isPurchased;

        public RerollShopSlotState(ItemSlotData item, bool isLocked = false, bool isPurchased = false)
        {
            this.item = item;
            this.isLocked = isLocked;
            this.isPurchased = isPurchased;
        }
    }

    public class RerollShopController : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("UI 참조")]
        [SerializeField] private FavorabilityUIController favorabilityUIController;
        [SerializeField] private FavorailityMgr favorailityMgr;               
        [SerializeField] private Transform contentParent;               // 6개의 슬롯이 배치된 Content 오브젝트
        [SerializeField] private float rerollInterval = 10800f;         // 자동 리롤 시간 (초 단위, 기본 3시간)
        [SerializeField] private List<ItemSlotData> shopItemPool;        // 전체 리롤 상점 아이템 풀
        [SerializeField] private RerollShopLockConfirmPanel lockConfirmPanel;

        private List<RerollShopSlotHandler> slotHandlers = new();      // 슬롯 핸들러 리스트
        private List<RerollShopSlotState> currentSlotStates = new(); // 현재 상태 저장용
        private Coroutine autoRerollRoutine;                            // 자동 리롤 코루틴 참조

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            if (currentSlotStates.Count == slotHandlers.Count)
            {
                // 기존 상태 복원
                for (int i = 0; i < slotHandlers.Count; i++)
                {
                    slotHandlers[i].InitializeWithState(currentSlotStates[i], this, favorailityMgr, favorabilityUIController, lockConfirmPanel);
                }
            }
            else
            {
                // 신규 진입 시 리롤
                ManualReroll();
            }

            autoRerollRoutine = StartCoroutine(AutoRerollTimer());
        }

        private void OnDisable()
        {
            // 꺼질 때 자동 리롤 코루틴 정지
            if (autoRerollRoutine != null)
                StopCoroutine(autoRerollRoutine);
        }

        // Public 메서드
        public void Init()
        {
            if (shopItemPool != null && shopItemPool.Count > 0)
                return;

            if (shopItemPool == null)
                shopItemPool = new();

            var tableData = DataTableMgr.RerollShopTable.ToArray();
            foreach (var data in tableData)
            {
                ItemSlotData slot = new ItemSlotData();
                slot.itemType = data.GetItemType();
                slot.currCount = data.ItemAmount;
                slot.maxCount = data.ItemAmount;
                slot.currencyType = CurrencyType.Coin;
                slot.pullRate = data.RerollRate;
                slot.Price = data.Price;
                shopItemPool.Add(slot);
            }

            // Content 아래에 있는 슬롯에 붙어있는 핸들러 수집
            for (int i = 0; i < contentParent.childCount; i++)
            {
                var handler = contentParent.GetChild(i).GetComponent<RerollShopSlotHandler>();
                if (handler != null)
                    slotHandlers.Add(handler);
            }
        }

        /// <summary>
        /// 외부 버튼에서 호출되는 수동 리롤
        /// </summary>
        public void ManualReroll()
        {
            currentSlotStates.Clear();

            foreach (var slot in slotHandlers)
            {
                if (!slot.IsLocked())
                {
                    var item = GetWeightedRandomItem(shopItemPool);
                    slot.Initialize((ItemSlotData)item, this, favorailityMgr, favorabilityUIController, lockConfirmPanel);
                }

                currentSlotStates.Add(slot.GetSlotState());
            }
        }

        public void SaveAllSlotStates()
        {
            currentSlotStates.Clear();
            foreach (var slot in slotHandlers)
            {
                currentSlotStates.Add(slot.GetSlotState());
            }
        }

        // Private 메서드

        private void RestoreSlotStates()
        {
            for (int i = 0; i < slotHandlers.Count; i++)
            {
                var state = currentSlotStates[i];
                slotHandlers[i].InitializeWithState(state, this, favorailityMgr, favorabilityUIController, lockConfirmPanel);
            }
        }
        /// <summary>
        /// 주기적으로 자동 리롤 수행
        /// </summary>
        private IEnumerator AutoRerollTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(rerollInterval);
                ManualReroll();
            }
        }

        /// <summary>
        /// 출현 확률(PullRate)에 따라 아이템을 무작위 선택 (중복 허용)
        /// </summary>
        private ItemSlotData? GetWeightedRandomItem(List<ItemSlotData> pool)
        {
            var valid = pool.ToList();
            if (valid.Count == 0)
            {
                Debug.LogError("리롤 상점에 사용할 유효한 아이템이 없습니다.");
                return null;
            }

            float totalWeight = valid.Sum(item => item.pullRate);
            float rand = Random.Range(0, totalWeight);
            float cumulative = 0f;

            foreach (var item in valid)
            {
                cumulative += item.pullRate;
                if (rand <= cumulative)
                    return item;
            }

            // 예외 상황 대비: 마지막 항목 반환
            return valid.Last();
        }
        // Others

    } // Scope by class RerollShopController

} // namespace Root