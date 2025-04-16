using SkyDragonHunter.test;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {

    public class FacilitySlotHandler : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private FacilityType type;         // 어떤 시설인지 식별
        [SerializeField] private Image itemIcon;         // 어떤 시설인지 식별
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI itemCountText;
        [SerializeField] private TextMeshProUGUI itemAcquireCountText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Button acquireButton;      // 개별 수령 버튼

        private FacilitySystemMgr.FacilityData data;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            itemIcon = acquireButton.GetComponent<Image>();
        }
        // Public 메서드
        public void Initialize(FacilitySystemMgr.FacilityData facilityData)
        {
            data = facilityData;

            acquireButton.onClick.RemoveAllListeners(); // 중복 방지
            acquireButton.onClick.AddListener(OnClickAcquire);

            UpdateUI();
        }

        public void UpdateUI()
        {
            if (data?.itemToGenerate?.itemImage != null)
            {
                itemIcon.sprite = data.itemToGenerate.itemImage;
            }
            else
            {
                Debug.LogWarning($"[FacilitySlotHandler] 아이템 이미지가 비어 있음 (type: {type})");
            }
            levelText.text = $"Lv. {data.level}";
            itemCountText.text = $"{data.itemCount} / {data.maxCount}";
            itemAcquireCountText.text = $"{data.perGenerate}";
            TimeSpan remaining = TimeSpan.FromSeconds(Mathf.Max(0, data.generateInterval - data.timer));
            timerText.text = remaining.ToString(@"hh\:mm\:ss");
        }

        public void OnClickAcquire()
        {
            if (data.itemCount > 0)
            {
                Debug.Log($"{data.itemToGenerate.itemName}을 {data.itemCount}만큼 획득했습니다.");
                // 최대 수량일 때만 타이머 리셋
                bool wasFull = data.itemCount >= data.maxCount;

                data.itemCount = 0;

                if (wasFull)
                {
                    data.timer = 0f; // 최대 수량에서 수령한 경우만 리셋
                }

                UpdateUI();
            }
        }

        public FacilityType GetFacilityType() => type;
        // Private 메서드
        // Others

    } // Scope by class FacilitySlotHandler

} // namespace Root