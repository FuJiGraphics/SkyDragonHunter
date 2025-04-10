using SkyDragonHunter.test;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkyDragonHunter {

    public class RerollShopController : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("UI 참조")]
        [SerializeField] private Transform contentParent;               // 6개의 슬롯이 배치된 Content 오브젝트
        [SerializeField] private float rerollInterval = 10800f;         // 자동 리롤 시간 (초 단위, 기본 3시간)
        [SerializeField] private List<ItemStatus> shopItemPool;        // 전체 리롤 상점 아이템 풀

        private List<RerollShopSlotHandler> slotHandlers = new();      // 슬롯 핸들러 리스트
        private Coroutine autoRerollRoutine;                            // 자동 리롤 코루틴 참조
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            // Content 아래에 있는 슬롯에 붙어있는 핸들러 수집
            for (int i = 0; i < contentParent.childCount; i++)
            {
                var handler = contentParent.GetChild(i).GetComponent<RerollShopSlotHandler>();
                if (handler != null)
                    slotHandlers.Add(handler);
            }
        }

        private void OnEnable()
        {
            // 상점이 켜질 때 리롤 수행
            ManualReroll();

            // 자동 리롤 코루틴 시작
            autoRerollRoutine = StartCoroutine(AutoRerollTimer());
        }

        private void OnDisable()
        {
            // 꺼질 때 자동 리롤 코루틴 정지
            if (autoRerollRoutine != null)
                StopCoroutine(autoRerollRoutine);
        }
        // Public 메서드

        /// <summary>
        /// 외부 버튼에서 호출되는 수동 리롤
        /// </summary>
        public void ManualReroll()
        {
            foreach (var slot in slotHandlers)
            {
                if (!slot.IsLocked())
                {
                    var item = GetWeightedRandomItem(shopItemPool);
                    slot.Initialize(item);
                }
            }
        }
        // Private 메서드

        /// <summary>
        /// 주기적으로 자동 리롤 수행
        /// </summary>
   
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
        private ItemStatus GetWeightedRandomItem(List<ItemStatus> pool)
        {
            var valid = pool.Where(p => p != null).ToList();
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