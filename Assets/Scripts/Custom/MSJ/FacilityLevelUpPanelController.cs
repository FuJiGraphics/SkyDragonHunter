using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using SkyDragonHunter.Managers;
using UnityEngine.UI;
using System.Collections.Generic;


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


        // TODO: LJH
        [SerializeField] private Button levelUpButtonOnlyForInteractableManaging;
        [SerializeField] private Transform materialContentsArea;
        [SerializeField] private UIFacilityMaterialSlot prefab;
        [SerializeField] private List<GameObject> materialsList;

        [SerializeField] private Image currentSingleProductionCountImage;
        [SerializeField] private Image nextSingleProductionCountImage;
        [SerializeField] private Image currentMaxCapacityImage;
        [SerializeField] private Image nextMaxCapacityImage;
        // ~TODO

        public int currentLevelUpCost;
        private FacilitySlotHandler currentSlot;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 기본 메서드
        private void Awake()
        {
            materialsList = new List<GameObject>();
            AddPointerDownEvent(levelUpButton, (BaseEventData _) => OnClickLevelUp());
        }
        // Public 메서드
        // 외부에서 슬롯 전달 시 호출
        public void Open(FacilitySlotHandler slot)
        {
            currentSlot = slot;
            var data = slot.GetFacilityData();

            foreach(var go in materialsList)
            {
                Destroy(go);
            }
            materialsList.Clear();

            // TODO: LJH
            #region LJH
            var tableData = data.FacilityTableData;
            facilityNameText.text = tableData.FacilityName;
            currentLevelText.text = $"Lv. {data.level}";
            currentProduceAmountText.text = $"{tableData.ItemYield}";
            currentMaxAmountText.text = $"{tableData.KeepItemAmount}";
            currentIntervalText.text = TimeSpan.FromSeconds(data.FacilityTableData.ItemMadeTime).ToString(@"mm\:ss");

            if (!data.FacilityTableData.IsMaxLevel)
            {
                var nextLevelData = DataTableMgr.FacilityTable.GetFacilityData(data.type, data.level + 1);
                nextLevelText.text = $"Lv. {data.level + 1}";
                nextProduceAmountText.text = $"{nextLevelData.ItemYield}";
                nextMaxAmountText.text = $"{nextLevelData.KeepItemAmount}";
                nextIntervalText.text = TimeSpan.FromSeconds(nextLevelData.ItemMadeTime).ToString(@"mm\:ss");
                levelUpIntervalTimeText.text = TimeSpan.FromSeconds(tableData.UpgradeTime).ToString(@"mm\:ss");
                levelUpCost.text = $"{tableData.UpgradeGold.ToUnit()}";
                for(int i = 0; i < tableData.RequiredItemTypes.Length; ++i)
                {
                    var matSlot = Instantiate(prefab, materialContentsArea);
                    matSlot.SetSlot(tableData.RequiredItemTypes[i], tableData.UpgradeItemCount[i]);
                    materialsList.Add(matSlot.gameObject);
                }

                currentSingleProductionCountImage.color = Color.white;
                currentMaxCapacityImage.color = Color.white;
                nextSingleProductionCountImage.color = Color.white;
                nextMaxCapacityImage.color = Color.white;

                currentSingleProductionCountImage.sprite = DataTableMgr.ItemTable.Get(tableData.ItemID).Icon;
                currentMaxCapacityImage.sprite = DataTableMgr.ItemTable.Get(tableData.ItemID).Icon;
                nextSingleProductionCountImage.sprite = DataTableMgr.ItemTable.Get(tableData.ItemID).Icon;
                nextMaxCapacityImage.sprite = DataTableMgr.ItemTable.Get(tableData.ItemID).Icon;

                levelUpButtonOnlyForInteractableManaging.interactable = true;
            }
            else
            {
                nextLevelText.text = $"Max";
                nextProduceAmountText.text = $"Max";
                nextMaxAmountText.text = $"Max";
                nextIntervalText.text = $"Max";
                levelUpIntervalTimeText.text = $"Max";
                levelUpCost.text = $"N/A";

                currentSingleProductionCountImage.color = new Color(0, 0, 0, 0);
                currentMaxCapacityImage.color = new Color(0, 0, 0, 0);
                nextSingleProductionCountImage.color = new Color(0, 0, 0, 0);
                nextMaxCapacityImage.color = new Color(0, 0, 0, 0);

                levelUpButtonOnlyForInteractableManaging.interactable = false;
            }
            #endregion

            #region MSJ
            //facilityNameText.text = data.type.ToString();
            //currentLevelText.text = $"Lv. {data.level}";
            //nextLevelText.text = $"Lv. {data.level + 1}";

            //currentProduceAmountText.text = $"{data.perGenerate}";
            //currentMaxAmountText.text = $"{data.level * 20}";
            //TimeSpan currentInterval = TimeSpan.FromSeconds(data.generateInterval);
            //currentIntervalText.text = currentInterval.ToString(@"mm\:ss");

            //nextProduceAmountText.text = $"{(data.level + 1) * 5}";
            //nextMaxAmountText.text = $"{(data.level + 1) * 20}";
            //TimeSpan nextInterval = TimeSpan.FromSeconds(data.generateInterval * 0.9f);
            //nextIntervalText.text = nextInterval.ToString(@"mm\:ss");

            //levelUpCost.text = GetLevelUpCost(data.level).ToString();
            //TimeSpan levelUpInterval = TimeSpan.FromSeconds(data.levelUpInterval);
            //levelUpIntervalTimeText.text = levelUpInterval.ToString(@"mm\:ss");
            #endregion
            // ~TODO
        }


        // 레벨업 버튼 클릭 시 처리
        public void OnClickLevelUp()
        {
            // TODO: LJH
            if(currentSlot != null)
            {
                currentSlot.TryLevelUp();
                gameObject.SetActive(false);
            }

            #region MSJ
            //if (currentSlot != null)
            //{
            //    currentSlot.TryStartLevelUp(currentLevelUpCost);
            //    gameObject.SetActive(false);
            //}
            #endregion
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