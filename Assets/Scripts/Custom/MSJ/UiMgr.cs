using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using SkyDragonHunter.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SkyDragonHunter.Test
{
    public class UiMgr : MonoBehaviour
    {
        // 필드 (Fields)
        public GameObject codexBonusStats;
        public GameObject codexTreasureInfo;
        public GameObject rankTab;
        public GameObject addToCodexInfo;
        public GameObject treasureEquipmentSlot;
        public GameObject treasureFusion;
        public GameObject treasureInfo;
        public GameObject fusionDeselectAllButton;
        public GameObject treasureSelectErro;
        public GameObject treasureFusionWarning;
        public GameObject treasureFusionSuccessInfo;
        public GameObject[] FusionButtons;
        //
        public GameObject inventoryPanel;
        public GameObject panelBackGroundImage;
        public GameObject facilityPanelPanel;
        public GameObject facilityPanelLevelUpPanel;
        public GameObject dungeonEntryPanel;
        public GameObject profileSettingsPanel;
        public GameObject growthPanel;
        public GameObject masteryPanelPicks;
        public GameObject randomCrewPickUpInfoPanel;
        public GameObject inGameButtonPanel;
        public GameObject inGameWaveInfoPanel;
        public GameObject fortressEquipmentPanel;
        public GameObject waveControler;
        public GameObject summonPanel;
        public GameObject waveSelectInfoPanel;
        public GameObject characterInfoPanel;
        public GameObject questPanel;
        public GameObject optionsPanel;
        public GameObject masteryPanel;
        public GameObject waveSlider;
        public GameObject waveRetryButton;
        public GameObject[] pickPanels;
        public GameObject[] fortressPickPanels;
        public GameObject[] rewardSlots;
        public GameObject[] ShopPanels;
        // TODO: LJH
        //private 
        // ~TODO
        private TestWaveController waveControlerScript;
        private RectTransform rectTransform;
        private Coroutine moveCoroutine;
        private bool isHideFusionButtons = true;
        private bool isHideInventoryPanel = true;
        private bool isHideFacilityLevelUpPanel = true;
        private bool isHideDungeonEntryPanel = true;
        private bool isHideProfileSettingsPanel = true;
        private bool isHideOptionsPanel = true;
        private bool isHideRandomCrewPanel = true;
        private bool isHideQuestPanel;
        private bool isFacilityWorkProgress;
        private int currentMissionLevel = 1;
        private int currentZoneLevel = 1;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            rectTransform = questPanel.GetComponent<RectTransform>();
            waveControler = GameMgr.FindObject("WaveController");
            waveControlerScript = waveControler.GetComponent<TestWaveController>();
            OnInGamePanels();
            AccountMgr.AddItemCountChangedEvent(OnChangedItemCount);
        }

        private void Update()
        {
            if (waveControlerScript.isInfiniteMode)
            {
                waveRetryButton.SetActive(true);
                waveSlider.SetActive(false);
            }
            else
            {
                waveRetryButton.SetActive(false);
                if (waveControlerScript.bossSlider.gameObject.activeSelf)
                {
                    waveSlider.SetActive(false);
                }
            }

            //if (waveControlerScript.isRewardSet)
            //{
            //    SetRewardItmes();
            //}

            if (!facilityPanelLevelUpPanel.activeInHierarchy)
            {
                isHideFacilityLevelUpPanel = true;
            }
        }

        private void OnDestroy()
        {
            AccountMgr.RemoveItemCountChangedEvent(OnChangedItemCount);
        }

        // Public 메서드
        public void OnInGamePanels()
        {
            AllPanelsOff();
            inGameButtonPanel.SetActive(true);
            inGameWaveInfoPanel.SetActive(true);
            //questPanel.SetActive(true);
            panelBackGroundImage.SetActive(false);

            if (waveControlerScript == null)
                return;

            if (waveControlerScript.tutorialController == null)
            {
                return;
            }
            else
            {
                if (!waveControlerScript.tutorialController.endTutorial &&
                    waveControlerScript.tutorialController.firstActive &&
                    !waveControlerScript.tutorialController.secondActive)
                {
                    waveControlerScript.tutorialController.OnSecondActive();
                }
                else if (!waveControlerScript.tutorialController.endTutorial &&
                    waveControlerScript.tutorialController.thirdActive &&
                    !waveControlerScript.tutorialController.fourthActive)
                {
                    waveControlerScript.tutorialController.OnFourthActive();
                }
                else if (!waveControlerScript.tutorialController.endTutorial &&
                    waveControlerScript.tutorialController.sixthActive &&
                    !waveControlerScript.tutorialController.seventhActive)
                {
                    waveControlerScript.tutorialController.OnSeventhActive();
                }
            }
        }

        public void OnTreasureCodexPanel()
        {
            AllPanelsOff();
            SetTreasureCodexPanel();
            panelBackGroundImage.SetActive(false);
        }

        public void OnOffAddToCodexInfo()
        {
            if (addToCodexInfo.activeSelf)
            {
                addToCodexInfo.SetActive(false);
                codexTreasureInfo.SetActive(false);
                rankTab.SetActive(true);
                codexBonusStats.SetActive(true);
            }
            else
            {
                addToCodexInfo.SetActive(true);
                codexTreasureInfo.SetActive(true);
                rankTab.SetActive(false);
                codexBonusStats.SetActive(false);
            }
        }

        public void OnOffTreasureEquipmentSlot()
        {
            if (treasureEquipmentSlot.activeSelf)
            {
                treasureEquipmentSlot.SetActive(false);
                treasureFusion.SetActive(true);
            }
            else
            {
                treasureEquipmentSlot.SetActive(true);
                treasureFusion.SetActive(false);
            }
        }

        public void OnOffTreasureInfo()
        {
            if (treasureInfo.activeSelf)
            {
                treasureInfo.SetActive(false);
            }
            else
            {
                treasureInfo.SetActive(true);
            }
        }

        public void OnOffTreasureSelectErro()
        {
            if (treasureSelectErro.activeSelf)
            {
                treasureSelectErro.SetActive(false);
            }
            else
            {
                treasureSelectErro.SetActive(true);
            }
        }

        public void OnOffTreasureFusionWarning()
        {
            if (treasureFusionWarning.activeSelf)
            {
                treasureFusionWarning.SetActive(false);
            }
            else
            {
                treasureFusionWarning.SetActive(true);
            }
        }

        public void OnOffTreasureFusionSuccessInfo()
        {
            if (treasureFusionSuccessInfo.activeSelf)
            {
                treasureFusionSuccessInfo.SetActive(false);
            }
            else
            {
                treasureFusionSuccessInfo.SetActive(true);
            }
        }

        public void OnOffFusionButtons()
        {
            if (isHideFusionButtons)
            {
                FusionButtons[0].SetActive(true);
                FusionButtons[1].SetActive(false);
                FusionButtons[2].SetActive(false);
                FusionButtons[3].SetActive(false);
                FusionButtons[4].SetActive(false);
                isHideFusionButtons = false;
            }
            else
            {
                FusionButtons[0].SetActive(false);
                FusionButtons[1].SetActive(true);
                FusionButtons[2].SetActive(true);
                FusionButtons[3].SetActive(true);
                FusionButtons[4].SetActive(true);
                isHideFusionButtons = true;

            }
        }

        public void OnOffInventoryPanel()
        {
            if (isHideInventoryPanel)
            {
                inventoryPanel.SetActive(true);
                isHideInventoryPanel = false;
            }
            else
            {
                inventoryPanel.SetActive(false);
                isHideInventoryPanel = true;
            }
        }

        public void OnOffDungeonEntryPanel()
        {
            if (isHideDungeonEntryPanel)
            {
                dungeonEntryPanel.SetActive(true);
                isHideDungeonEntryPanel = false;
            }
            else
            {
                dungeonEntryPanel.SetActive(false);
                isHideDungeonEntryPanel = true;
            }
        }

        public void OnOffProfileSettingsPanel()
        {
            if (isHideProfileSettingsPanel)
            {
                profileSettingsPanel.SetActive(true);
                isHideProfileSettingsPanel = false;
            }
            else
            {
                profileSettingsPanel.SetActive(false);
                isHideProfileSettingsPanel = true;
            }
        }

        public void OnOffFacilityLevelUpPanel()
        {
            GameObject clicked = EventSystem.current.currentSelectedGameObject;
            if (clicked != null)
            {
                // 클릭된 버튼의 부모 중에서 FacilitySlotHandler를 탐색
                FacilitySlotHandler handler = clicked.GetComponentInParent<FacilitySlotHandler>();
                if (handler != null)
                {
                    isFacilityWorkProgress = handler.isLevelUpCompleteReady;
                }
            }

            if (isHideFacilityLevelUpPanel && isFacilityWorkProgress)
            {
                facilityPanelLevelUpPanel.SetActive(true);
                isHideFacilityLevelUpPanel = false;
            }
            else
            {
                facilityPanelLevelUpPanel.SetActive(false);
                isHideFacilityLevelUpPanel = true;
            }
        }

        public void OnFacilityPanel()
        {
            AllPanelsOff();
            facilityPanelPanel.SetActive(true);
            panelBackGroundImage.SetActive(true);
        }

        public void OnGrowthPanel()
        {
            AllPanelsOff();
            growthPanel.SetActive(true);
            panelBackGroundImage.SetActive(true);
        }

        public void OnMasteryPanel()
        {
            AllPanelsOff();
            masteryPanelPicks.SetActive(true);
            panelBackGroundImage.SetActive(true);
        }

        public void OnGoldShopPanel()
        {
            AllPanelsOff();
            ShopPanels[0].SetActive(true);
            ShopPanels[1].SetActive(false);
            ShopPanels[2].SetActive(false);
            panelBackGroundImage.SetActive(true);
        }

        public void OnDiamondShopPanel()
        {
            AllPanelsOff();
            ShopPanels[0].SetActive(false);
            ShopPanels[1].SetActive(true);
            ShopPanels[2].SetActive(false);
            panelBackGroundImage.SetActive(true);
        }

        public void OnRerollShopPanel()
        {
            AllPanelsOff();
            ShopPanels[0].SetActive(false);
            ShopPanels[1].SetActive(false);
            ShopPanels[2].SetActive(true);
            panelBackGroundImage.SetActive(true);
        }

        public void OnOffRandomCrewPickUpInfo()
        {
            if (randomCrewPickUpInfoPanel != null)
            {
                if (isHideRandomCrewPanel)
                {
                    randomCrewPickUpInfoPanel.SetActive(true);
                    isHideRandomCrewPanel = false;
                }
                else
                {
                    randomCrewPickUpInfoPanel.SetActive(false);
                    isHideRandomCrewPanel = true;
                }
            }
        }

        public void OnOffFortressEquipmentPanel()
        {
            AllPanelsOff();
            fortressEquipmentPanel.SetActive(true);
            OnFortressPickPanel0();
        }

        public void OnSelectWaveToGO()
        {
            waveSelectInfoPanel.SetActive(false);
            waveControlerScript.OnSelectWave(currentMissionLevel, currentZoneLevel);
            OnInGamePanels();
        }

        public void OnMissionButtonClicked()
        {
            GameObject clicked = EventSystem.current.currentSelectedGameObject;
            if (clicked == null) return;

            string name = clicked.name;
            if (name.StartsWith("Mission"))
            {
                string numStr = name.Substring(7);
                if (int.TryParse(numStr, out int level))
                {
                    currentMissionLevel = level;
                    Debug.Log($"Mission selected: {currentMissionLevel}");
                }
            }
        }

        public void OnZoneButtonClicked()
        {
            GameObject clicked = EventSystem.current.currentSelectedGameObject;
            if (clicked == null) return;

            string name = clicked.name; // 예: "Zone3"
            if (name.StartsWith("Zone"))
            {
                string numStr = name.Substring(4);
                if (int.TryParse(numStr, out int level))
                {
                    currentZoneLevel = level;
                    Debug.Log($"Zone selected: {currentZoneLevel}");
                }
            }
        }

        public void AllOffFortressPickPanel()
        {
            AllPanelsOff();
            OnInGamePanels();
        }

        public void OnFortressPickPanel0()
        {
            AllPanelsOff();
            fortressPickPanels[0].SetActive(true);
            fortressPickPanels[1].SetActive(false);
            fortressPickPanels[2].SetActive(false);
            fortressPickPanels[3].SetActive(false);
            fortressPickPanels[4].SetActive(false);
            panelBackGroundImage.SetActive(false);

        }

        public void OnFortressPickPanel1()
        {
            AllPanelsOff();
            fortressPickPanels[1].SetActive(true);
            fortressPickPanels[0].SetActive(false);
            fortressPickPanels[2].SetActive(false);
            fortressPickPanels[3].SetActive(false);
            fortressPickPanels[4].SetActive(false);
            panelBackGroundImage.SetActive(false);
        }

        public void OnFortressPickPanel2()
        {
            AllPanelsOff();
            fortressPickPanels[2].SetActive(true);
            fortressPickPanels[0].SetActive(false);
            fortressPickPanels[1].SetActive(false);
            fortressPickPanels[3].SetActive(false);
            fortressPickPanels[4].SetActive(false);
            panelBackGroundImage.SetActive(false);
        }

        public void OnFortressPickPanel3()
        {
            AllPanelsOff();
            fortressPickPanels[3].SetActive(true);
            fortressPickPanels[0].SetActive(false);
            fortressPickPanels[1].SetActive(false);
            fortressPickPanels[2].SetActive(false);
            fortressPickPanels[4].SetActive(false);
            panelBackGroundImage.SetActive(false);
        }
        public void OnFortressPickPanel4()
        {
            AllPanelsOff();
            fortressPickPanels[4].SetActive(true);
            fortressPickPanels[0].SetActive(false);
            fortressPickPanels[1].SetActive(false);
            fortressPickPanels[2].SetActive(false);
            fortressPickPanels[3].SetActive(false);
            SetTreasureEquipmentPanel();
            panelBackGroundImage.SetActive(false);
        }

        public void OnPickPanel0()
        {
            pickPanels[0].SetActive(true);
            pickPanels[1].SetActive(false);
            pickPanels[2].SetActive(false);
            pickPanels[3].SetActive(false);
            panelBackGroundImage.SetActive(true);
        }

        public void OnPickPanel1()
        {
            pickPanels[0].SetActive(false);
            pickPanels[1].SetActive(true);
            pickPanels[2].SetActive(false);
            pickPanels[3].SetActive(false);
            panelBackGroundImage.SetActive(true);
        }

        public void OnPickPanel2()
        {
            pickPanels[0].SetActive(false);
            pickPanels[1].SetActive(false);
            pickPanels[2].SetActive(true);
            pickPanels[3].SetActive(false);
            panelBackGroundImage.SetActive(true);
        }

        public void OnPickPanel3()
        {
            pickPanels[0].SetActive(false);
            pickPanels[1].SetActive(false);
            pickPanels[2].SetActive(false);
            pickPanels[3].SetActive(true);
            panelBackGroundImage.SetActive(true);
        }

        public void OnSummonPanel()
        {
            if (summonPanel != null)
            {
                AllPanelsOff();
                panelBackGroundImage.SetActive(true);
                summonPanel.SetActive(true);
                OnPickPanel0();
                randomCrewPickUpInfoPanel.SetActive(false);
                isHideRandomCrewPanel = true;
            }
        }

        public void OnWaveSelectPanel()
        {
            if (waveSelectInfoPanel != null)
            {
                AllPanelsOff();
                waveSelectInfoPanel.SetActive(true);
            }
        }

        public void OnCharacterInfoPanel()
        {
            if (characterInfoPanel != null)
            {
                AllPanelsOff();
                characterInfoPanel.SetActive(true);
                panelBackGroundImage.SetActive(true);
            }
        }

        public void OnOffHideOptionsPanel()
        {
            if (optionsPanel != null)
            {
                if (isHideOptionsPanel)
                {
                    optionsPanel.SetActive(true);
                    isHideOptionsPanel = false;
                }
                else
                {
                    optionsPanel.SetActive(false);
                    isHideOptionsPanel = true;
                }
            }
        }

        //public void OnOffHideQuestPanel()
        //{
        //    //rectTransform.DOAnchorPosX(-270f, 0.3f); // 패키지 필요
        //    if (moveCoroutine != null)
        //    {
        //        StopCoroutine(moveCoroutine);
        //        moveCoroutine = null;
        //    }

        //    if (!isHideQuestPanel)
        //    {
        //        moveCoroutine = StartCoroutine(MovePanelX(rectTransform, -150f, 0.5f));
        //        isHideQuestPanel = true;
        //    }
        //    else
        //    {
        //        moveCoroutine = StartCoroutine(MovePanelX(rectTransform, 0f, 0.5f));
        //        isHideQuestPanel = false;
        //    }
        //}


        // Private 메서드
        //private void SetRewardItmes()
        //{
        //    // DropTracker에 기록된 드롭 아이템 종류별 수량을 가져옴
        //    var dropDict = RewardMgr.GetRewards();

        //    int index = 0; // rewardSlots 배열 인덱스용

        //    // Dictionary의 모든 드롭 데이터를 순회
        //    foreach (var kvp in dropDict)
        //    {
        //        // 슬롯 수를 초과하면 더 이상 표시하지 않음 (예: 슬롯 4개인데 드롭 6개인 경우)
        //        if (index >= rewardSlots.Length) break;

        //        // 현재 표시할 슬롯 오브젝트
        //        GameObject slot = rewardSlots[index];

        //        // 슬롯의 자식 구조에서 "RewardSlotText" 오브젝트를 찾아 텍스트 컴포넌트 참조
        //        var typeText = slot.transform.Find("Image/RewardSlotText")?.GetComponent<TextMeshProUGUI>();

        //        // 슬롯 내부에 있는 또 다른 Image 자식 아래의 "RewardSlotCountText" 텍스트 컴포넌트 참조
        //        var countText = slot.transform.Find("Image/Image/RewardSlotCountText")?.GetComponent<TextMeshProUGUI>();

        //        // 드롭된 아이템의 타입을 텍스트로 표시 (예: Gold, Potion 등)
        //        if (typeText != null) typeText.text = kvp.Key.ToString();

        //        // 드롭된 아이템의 수량을 숫자로 표시
        //        if (countText != null) countText.text = kvp.Value.ToString();

        //        index++; // 다음 슬롯으로 인덱스 증가
        //    }

        //    // 남은 슬롯이 있을 경우, 빈 문자열로 초기화해서 이전 내용이 안 보이도록 처리
        //    for (; index < rewardSlots.Length; index++)
        //    {
        //        // 남은 슬롯 오브젝트
        //        var typeText = rewardSlots[index].transform.Find("Image/RewardSlotText")?.GetComponent<TextMeshProUGUI>();
        //        var countText = rewardSlots[index].transform.Find("Image/Image/RewardSlotCountText")?.GetComponent<TextMeshProUGUI>();

        //        // 텍스트를 빈 문자열로 설정하여 보이지 않게 함
        //        if (typeText != null) typeText.text = "";
        //        if (countText != null) countText.text = "";
        //    }
        //}
        // Others
        //private IEnumerator MovePanelX(RectTransform target, float toX, float duration)
        //{
        //    Vector2 start = target.anchoredPosition;
        //    Vector2 end = new Vector2(toX, start.y);
        //    float time = 0f;

        //    while (time < duration)
        //    {
        //        time += Time.deltaTime;
        //        target.anchoredPosition = Vector2.Lerp(start, end, time / duration);
        //        yield return null;
        //    }

        //    target.anchoredPosition = end;
        //}

        public void AllPanelsOff()
        {
            inGameButtonPanel.SetActive(false);
            inGameWaveInfoPanel.SetActive(false);
            fortressEquipmentPanel.SetActive(false);
            summonPanel.SetActive(false);
            waveSelectInfoPanel.SetActive(false);
            characterInfoPanel.SetActive(false);
            questPanel.SetActive(false);
            optionsPanel.SetActive(false);
            masteryPanel.SetActive(false);
            growthPanel.SetActive(false);
            masteryPanelPicks.SetActive(false);
            facilityPanelPanel.SetActive(false);
            inventoryPanel.SetActive(false);
            dungeonEntryPanel.SetActive(false);
            isHideDungeonEntryPanel = true;
            foreach (var panel in pickPanels)
            {
                panel.SetActive(false);
            }
            foreach (var panel in fortressPickPanels)
            {
                panel?.SetActive(false);
            }

            foreach (var panel in ShopPanels)
            {
                panel?.SetActive(false);
            }
        }

        private void SetTreasureCodexPanel()
        {
            addToCodexInfo.SetActive(false);
            codexTreasureInfo.SetActive(false);
            rankTab.SetActive(true);
            codexBonusStats.SetActive(true);
        }

        private void SetTreasureEquipmentPanel()
        {
            treasureEquipmentSlot.SetActive(true);
            treasureFusion.SetActive(false);
            treasureInfo.SetActive(true);
            fusionDeselectAllButton.SetActive(false);
            treasureSelectErro.SetActive(false);
            treasureFusionWarning.SetActive(false);
            treasureFusionSuccessInfo.SetActive(false);
        }

        private void OnChangedItemCount(ItemType type)
        {
            if (type == ItemType.Coin)
            {
                if (inventoryPanel.TryGetComponent<UIInventoryPanel>(out var invenComp))
                {
                    invenComp.CoinText = AccountMgr.Coin.ToUnit();
                }
            }
        }

    } // Scope by class PanelHide

} // namespace Root