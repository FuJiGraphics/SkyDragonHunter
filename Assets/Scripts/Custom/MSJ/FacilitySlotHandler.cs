using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SkyDragonHunter.Managers;

namespace SkyDragonHunter.UI {

    public class FacilitySlotHandler : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private FavorailityMgr favorailityMgr;
        [SerializeField] private GameObject WorkInProgressObj;                // 레벨업 대기 상태 표시용 오브젝트
        [SerializeField] private FacilityLevelUpPanelController levelUpPanel; // 레벨업 패널 참조
        [SerializeField] private FacilityType type;                           // 시설 종류 식별용 타입
        [SerializeField] private EventTrigger acquireButton;                  // 아이템 수령 버튼 (EventTrigger 사용)
        [SerializeField] private Image itemIcon;                              // 생성 아이템 아이콘
        [SerializeField] private EventTrigger workInProgressImageButton;      // 공사 완료 버튼 (EventTrigger 사용)
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI itemCountText;
        [SerializeField] private TextMeshProUGUI itemAcquireCountText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI WorkInProgressText;          // 공사중 텍스트
        [SerializeField] private TextMeshProUGUI levelUpTimerText;           // 레벨업 타이머 텍스트

        private FacilitySystemMgr.FacilityData data;

        // 속성 (Properties)
        public bool isLevelUpCompleteReady { get; private set; } = false;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)


        private void Start()
        {
            // acquireButton에 붙은 이미지 컴포넌트 캐싱
            itemIcon = acquireButton.GetComponent<Image>();
            isLevelUpCompleteReady = true;
        }

        // Public 메서드
        // 슬롯 초기화 메서드
        public void Initialize(FacilitySystemMgr.FacilityData facilityData)
        {
            data = facilityData;

            WorkInProgressObj.SetActive(true);
            UpdateUI();

            // 아이템 수령 버튼에 이벤트 연결
            AddPointerDownEvent(acquireButton, (BaseEventData _) => OnClickAcquire());

            // 공사 완료 버튼에 이벤트 연결
            AddPointerDownEvent(workInProgressImageButton, (BaseEventData _) => TryCompleteLevelUp());
        }

        public void UpdateUI()
        {
            // TODO: LJH
            if (data.isUpgrading)
            {
                if(data.IsUpgradeComplete)
                {
                    // Logics when upgrade is completed
                    // shows remaining time
                    levelUpTimerText.text = "00:00:00";

                    // indicates if under construction
                    WorkInProgressObj.SetActive(true);
                    WorkInProgressText.text = "공사완료";
                    workInProgressImageButton.GetComponent<Image>().color = Color.green;

                    // disables production info
                    timerText.text = "--:--:--";
                    itemAcquireCountText.text = "--";

                    itemCountText.text = $"0 / {data.FacilityTableData.KeepItemAmount}";
                    levelText.text = $"Lv. {data.level}";
                }
                else
                {
                    // Logics when upgrade is on process
                    // shows remaining time
                    levelUpTimerText.text = data.UpgradeRemainingTimeSpan.ToString(@"hh\:mm\:ss");

                    // indicates if under construction
                    WorkInProgressObj.SetActive(true);
                    WorkInProgressText.text = "공사중";
                    workInProgressImageButton.GetComponent<Image>().color = Color.red;

                    // disables production info
                    timerText.text = "--:--:--";
                    itemAcquireCountText.text = "--";

                    itemCountText.text = $"0 / {data.FacilityTableData.KeepItemAmount}";
                    levelText.text = $"Lv. {data.level}";
                }
            }
            else
            {
                WorkInProgressObj.SetActive(false);
                itemIcon.sprite = DataTableMgr.ItemTable.Get(data.FacilityTableData.ItemID).Icon;

                levelText.text = $"Lv. {data.level}";
                itemCountText.text = $"{data.TotalProducts} / {data.FacilityTableData.KeepItemAmount}";
                itemAcquireCountText.text = $"{data.FacilityTableData.ItemYield}";
                timerText.text = data.RemainingTimeSpanUntilNextProduct.ToString(@"hh\:mm\:ss");
            }
            #region MSJ
            //if (data.isInLevelUpCooldown)
            //{
            //    // 공사 진행 중
            //    isLevelUpCompleteReady = false;
            //
            //    // 남은 시간 표시
            //    TimeSpan remain = TimeSpan.FromSeconds(data.levelUpCooldown);
            //    levelUpTimerText.text = remain.ToString(@"hh\:mm\:ss");
            //
            //    // 공사중 표시
            //    WorkInProgressText.text = "공사중";
            //    workInProgressImageButton.GetComponent<Image>().color = Color.red;
            //
            //    // 공사중 오브젝트 활성화
            //    WorkInProgressObj.SetActive(true);
            //
            //    // 생산 정보 비활성화
            //    timerText.text = "--:--:--";
            //    itemAcquireCountText.text = "--";
            //
            //    // 아이템 개수 및 레벨은 공통 출력
            //    itemCountText.text = $"{data.itemCount} / {data.maxCount}";
            //    levelText.text = $"Lv. {data.level}";
            //}
            //else if (isLevelUpCompleteReady)
            //{
            //
            //    // 일반 상태
            //
            //    WorkInProgressObj.SetActive(false);
            //
            //    // 아이콘 적용
            //    if (data?.itemToGenerate.ItemIcon != null)
            //    {
            //        itemIcon.sprite = data.itemToGenerate.ItemIcon;
            //    }
            //    else
            //    {
            //        Debug.LogWarning($"[FacilitySlotHandler] 아이템 이미지가 비어 있음 (type: {type})");
            //    }
            //
            //    // 기본 정보 출력
            //    levelText.text = $"Lv. {data.level}";
            //    itemCountText.text = $"{data.itemCount} / {data.maxCount}";
            //    itemAcquireCountText.text = $"{data.perGenerate}";
            //
            //    // 남은 생산 시간
            //    TimeSpan remaining = TimeSpan.FromSeconds(Mathf.Max(0, data.generateInterval - data.timer));
            //    timerText.text = remaining.ToString(@"hh\:mm\:ss");
            //}
            //else
            //{
            //    // 공사 완료 UI 표시
            //    WorkInProgressText.text = "공사완료";
            //    workInProgressImageButton.GetComponent<Image>().color = Color.green;
            //    levelUpTimerText.text = "00:00:00";
            //
            //    // 공사중 오브젝트 활성화
            //    WorkInProgressObj.SetActive(true);
            //
            //    // 생산 정보 비활성화
            //    timerText.text = "--:--:--";
            //    itemAcquireCountText.text = "--";
            //
            //    // 아이템 개수 및 레벨은 공통 출력
            //    itemCountText.text = $"{data.itemCount} / {data.maxCount}";
            //    levelText.text = $"Lv. {data.level}";
            //}
            #endregion
            // ~TODO
        }


        // 아이템 수령 로직
        public void OnClickAcquire()
        {
            // TODO: LJH
            if (data.isUpgrading)
                return;

            if (data.ProducedCounts == 0)
                return;

            Debug.LogWarning($"{data.ProductItemType} acquired {data.TotalProducts}");
            AccountMgr.AddItemCount(data.ProductItemType, data.TotalProducts);
            data.lastAccquiredTime = DateTime.UtcNow;
            UpdateUI();
            SaveLoadMgr.UpdateSaveData(SaveDataTypes.Facility);
            #region MSJ
            //if (data.itemCount > 0)
            //{
            //    Debug.Log($"{data.itemToGenerate.ItemName}을 {data.itemCount}만큼 획득했습니다.");
            //    AccountMgr.AddItemCount(data.itemToGenerate.itemType, data.itemCount);
            //
            //    bool wasFull = data.itemCount >= data.maxCount;
            //    data.itemCount = 0;
            //
            //    // 최대치일 때만 타이머 리셋
            //    if (wasFull)
            //    {
            //        data.timer = 0f;
            //    }
            //
            //    UpdateUI();
            //}
            #endregion
            // ~TODO
        }

        // 레벨업 완료 처리 시도
        public void TryCompleteLevelUp()
        {
            // TODO: LJH
            if(!data.IsUpgradeComplete)
                return;

            data.level++;
            data.lastAccquiredTime = DateTime.UtcNow;
            data.upgradeStartedTime = DateTime.UtcNow;
            data.isUpgrading = false;
            UpdateUI();
            SaveLoadMgr.CallSaveGameData();

            #region MSJ
            //if (!data.isInLevelUpCooldown && !isLevelUpCompleteReady)
            //{
            //    data.level++;
            //    data.currentExp = 0;
            //    data.timer = 0f; // 생산 시간 초기화
            //    Debug.Log("레벨업 완료!");
            //    isLevelUpCompleteReady = true;
            //    UpdateUI();
            //}
            #endregion
            // ~TODO
        }

        // 슬롯 클릭 시 레벨업 패널 열기
        public void OnSandDataToLevelUpPanel()
        {
            if (data.isUpgrading)
                return;
            levelUpPanel.Open(this);
        }

        public FacilitySystemMgr.FacilityData GetFacilityData()
        {
            return data;
        }

        public void TryStartLevelUp(int cost)
        {
            if (data.isInLevelUpCooldown)
            {
                Debug.Log("레벨업 대기 중입니다."); // 사용자 확인용 로그
                return;
            }
            if (favorailityMgr.testGold < cost)
            {
                Debug.Log("골드 부족");
                return;
            }

            favorailityMgr.testGold -= cost;
            data.levelUpCooldown = data.level * 10f;
            UpdateUI();            
        }

        public FacilityType GetFacilityType() => type;
        // Private 메서드

        // 이벤트트리거 등록 유틸리티 메서드
        private void AddPointerDownEvent(EventTrigger trigger, UnityEngine.Events.UnityAction<BaseEventData> action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }
        // Others
                
        // TODO: LJH
        public void TryLevelUp()
        {            
            
            // Check resource availabilities
            var tableData = data.FacilityTableData;
            if (data.IsMaxLevel || data.isUpgrading)
                return;

            bool resourceInsufficient = false;

            if(AccountMgr.Coin < tableData.UpgradeGold)
            {
                Debug.Log($"Facility Level Up failed : insufficient coin");
                resourceInsufficient = true;
            }
            for (int i = 0; i < tableData.UpgradeItemID.Length; ++i)
            {
                if (AccountMgr.ItemCount(tableData.RequiredItemTypes[i]) < tableData.UpgradeItemCount[i])
                {
                    Debug.Log($"Facility Level Up failed : insufficient [{tableData.RequiredItemTypes[i]}]");
                    resourceInsufficient = true;
                }
            }
            if (resourceInsufficient)
            {
                DrawableMgr.Dialog($"안내", $"재화가 부족합니다");
                return;
            }
            
            // Resource count reset
            AccountMgr.Coin -= tableData.UpgradeGold;
            for (int i = 0; i < tableData.UpgradeItemID.Length; ++i)
            {
                var newCount = AccountMgr.ItemCount(tableData.RequiredItemTypes[i]) - tableData.UpgradeItemCount[i];
                AccountMgr.SetItemCount(tableData.RequiredItemTypes[i], newCount);
            }

            // Acquires products before upgrade if there are any
            OnClickAcquire();
            // Apply to data
            data.isUpgrading = true;
            data.upgradeStartedTime = DateTime.UtcNow;
            SaveLoadMgr.CallSaveGameData();
        }

        // ~TODO
        
    } // Scope by class FacilitySlotHandler

} // namespace Root