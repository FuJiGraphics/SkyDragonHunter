using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


namespace SkyDragonHunter.UI {

    public class FacilityLevelUpPanelController : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("UI 연결")]
        [SerializeField] private TextMeshProUGUI facilityNameText;
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI nextLevelText;

        [SerializeField] private TextMeshProUGUI currentProduceAmountText;
        [SerializeField] private TextMeshProUGUI currentMaxAmountText;
        [SerializeField] private TextMeshProUGUI currentIntervalText;

        [SerializeField] private TextMeshProUGUI nextProduceAmountText;
        [SerializeField] private TextMeshProUGUI nextMaxAmountText;
        [SerializeField] private TextMeshProUGUI nextIntervalText;

        [SerializeField] private TextMeshProUGUI levelUpCost;
        [SerializeField] private TextMeshProUGUI levelUpIntervalTimeText;

        [SerializeField] private EventTrigger levelUpButton;

        public int currentLevelUpCost;
        private FacilitySlotHandler currentSlot;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 기본 메서드
        private void Awake()
        {
            AddPointerDownEvent(levelUpButton, (BaseEventData _) => OnClickLevelUp());
        }
        // Public 메서드
        // 외부에서 슬롯 전달 시 호출
        public void Open(FacilitySlotHandler slot)
        {
            currentSlot = slot;
            var data = slot.GetFacilityData();

            // TODO: LJH
            //facilityNameText.text = data.type.ToString();
            //currentLevelText.text = $"Lv. {data.level}";
            //nextLevelText.text = $"Lv. {data.level + 1}";
            //
            //currentProduceAmountText.text = $"{data.perGenerate}";
            //currentMaxAmountText.text = $"{data.level * 20}";
            //TimeSpan currentInterval = TimeSpan.FromSeconds(data.generateInterval);
            //currentIntervalText.text = currentInterval.ToString(@"mm\:ss");
            //
            //nextProduceAmountText.text = $"{(data.level + 1) * 5}";
            //nextMaxAmountText.text = $"{(data.level + 1) * 20}";
            //TimeSpan nextInterval = TimeSpan.FromSeconds(data.generateInterval * 0.9f);
            //nextIntervalText.text = nextInterval.ToString(@"mm\:ss");
            //
            //levelUpCost.text = GetLevelUpCost(data.level).ToString();
            //TimeSpan levelUpInterval = TimeSpan.FromSeconds(data.levelUpInterval);
            //levelUpIntervalTimeText.text = levelUpInterval.ToString(@"mm\:ss");





            // ~TODO
        }


        // 레벨업 버튼 클릭 시 처리
        public void OnClickLevelUp()
        {
            // TODO: LJH
            //if (currentSlot != null)
            //{
            //    currentSlot.TryStartLevelUp(currentLevelUpCost);
            //    gameObject.SetActive(false);
            //}

            if(currentSlot != null)
            {
                currentSlot.TryLevelUp();
                gameObject.SetActive(false);
            }

            // ~TODO
        }
        // Private 메서드

        // PointerDown 이벤트 등록 함수
        // 포인터 다운 이벤트 추가 메서드
        private void AddPointerDownEvent(EventTrigger trigger, UnityEngine.Events.UnityAction<BaseEventData> action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }

        // 레벨업 비용 계산
        private int GetLevelUpCost(int level)
        {
            currentLevelUpCost = level * 500;
            return currentLevelUpCost;
        }
        // Others

    } // Scope by class FacilityLevelUpPanel

} // namespace Root