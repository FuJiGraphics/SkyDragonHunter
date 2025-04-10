using SkyDragonHunter.Managers;
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
        public GameObject waveSlider;
        public GameObject waveRetryButton;
        public GameObject[] pickPanels;
        public GameObject[] fortressPickPanels;
        public GameObject[] rewardSlots;
        public GameObject[] ShopPanels;
        private TestWaveController waveControlerScript;
        private RectTransform rectTransform;
        private Coroutine moveCoroutine;
        private bool isHideOptionsPanel = true;
        private bool isHideRandomCrewPanel = true;
        private bool isHideQuestPanel;
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
                waveSlider.SetActive(true);
            }

            if (waveControlerScript.isRewardSet)
            {
                SetRewardItmes();
            }
        }

        // Public 메서드
        public void OnInGamePanels()
        {
            AllPanelsOff();
            inGameButtonPanel.SetActive(true);
            inGameWaveInfoPanel.SetActive(true);
        }

        public void OnGoldShopPanel()
        {
            AllPanelsOff();
            ShopPanels[0].SetActive(true);
            ShopPanels[1].SetActive(false);
            ShopPanels[2].SetActive(false);
        }

        public void OnDiamondShopPanel()
        {
            AllPanelsOff();
            ShopPanels[0].SetActive(false);
            ShopPanels[1].SetActive(true);
            ShopPanels[2].SetActive(false);
        }

        public void OnRerollShopPanel()
        {
            AllPanelsOff();
            ShopPanels[0].SetActive(false);
            ShopPanels[1].SetActive(false);
            ShopPanels[2].SetActive(true);
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
        }

        public void OnFortressPickPanel1()
        {
            AllPanelsOff();
            fortressPickPanels[1].SetActive(true);
            fortressPickPanels[0].SetActive(false);
            fortressPickPanels[2].SetActive(false);
            fortressPickPanels[3].SetActive(false);
        }

        public void OnFortressPickPanel2()
        {
            AllPanelsOff();
            fortressPickPanels[2].SetActive(true);
            fortressPickPanels[0].SetActive(false);
            fortressPickPanels[1].SetActive(false);
            fortressPickPanels[3].SetActive(false);
        }

        public void OnFortressPickPanel3()
        {
            AllPanelsOff();
            fortressPickPanels[3].SetActive(true);
            fortressPickPanels[0].SetActive(false);
            fortressPickPanels[1].SetActive(false);
            fortressPickPanels[2].SetActive(false);
        }

        public void OnPickPanel0()
        {
            pickPanels[0].SetActive(true);
            pickPanels[1].SetActive(false);
            pickPanels[2].SetActive(false);
        }

        public void OnPickPanel1()
        {
            pickPanels[0].SetActive(false);
            pickPanels[1].SetActive(true);
            pickPanels[2].SetActive(false);
        }

        public void OnPickPanel2()
        {
            pickPanels[0].SetActive(false);
            pickPanels[1].SetActive(false);
            pickPanels[2].SetActive(true);
        }

        public void OnOffSummonPanel()
        {
            if (summonPanel != null)
            {
                AllPanelsOff();
                summonPanel.SetActive(true);
                OnPickPanel0();
                randomCrewPickUpInfoPanel.SetActive(false);
                isHideRandomCrewPanel = true;
            }
        }

        public void OnOffWaveSelectPanel()
        {
            if (waveSelectInfoPanel != null)
            {
                AllPanelsOff();
                waveSelectInfoPanel.SetActive(true);
            }
        }

        public void OnOffCharacterInfoPanel()
        {
            if (characterInfoPanel != null)
            {
                AllPanelsOff();
                characterInfoPanel.SetActive(true);
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

        public void OnOffHideQuestPanel()
        {
            //rectTransform.DOAnchorPosX(-270f, 0.3f); // 패키지 필요
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            if (!isHideQuestPanel)
            {
                moveCoroutine = StartCoroutine(MovePanelX(rectTransform, -270f, 0.5f));
                isHideQuestPanel = true;
            }
            else
            {
                moveCoroutine = StartCoroutine(MovePanelX(rectTransform, 0f, 0.5f));
                isHideQuestPanel = false;
            }
        }


        // Private 메서드
        private void SetRewardItmes()
        {
            // DropTracker에 기록된 드롭 아이템 종류별 수량을 가져옴
            var dropDict = ItemMgr.GetDropCounts();

            int index = 0; // rewardSlots 배열 인덱스용

            // Dictionary의 모든 드롭 데이터를 순회
            foreach (var kvp in dropDict)
            {
                // 슬롯 수를 초과하면 더 이상 표시하지 않음 (예: 슬롯 4개인데 드롭 6개인 경우)
                if (index >= rewardSlots.Length) break;

                // 현재 표시할 슬롯 오브젝트
                GameObject slot = rewardSlots[index];

                // 슬롯의 자식 구조에서 "RewardSlotText" 오브젝트를 찾아 텍스트 컴포넌트 참조
                var typeText = slot.transform.Find("Image/RewardSlotText")?.GetComponent<TMP_Text>();

                // 슬롯 내부에 있는 또 다른 Image 자식 아래의 "RewardSlotCountText" 텍스트 컴포넌트 참조
                var countText = slot.transform.Find("Image/Image/RewardSlotCountText")?.GetComponent<TMP_Text>();

                // 드롭된 아이템의 타입을 텍스트로 표시 (예: Gold, Potion 등)
                if (typeText != null) typeText.text = kvp.Key.ToString();

                // 드롭된 아이템의 수량을 숫자로 표시
                if (countText != null) countText.text = kvp.Value.ToString();

                index++; // 다음 슬롯으로 인덱스 증가
            }

            // 남은 슬롯이 있을 경우, 빈 문자열로 초기화해서 이전 내용이 안 보이도록 처리
            for (; index < rewardSlots.Length; index++)
            {
                // 남은 슬롯 오브젝트
                var typeText = rewardSlots[index].transform.Find("Image/RewardSlotText")?.GetComponent<TMP_Text>();
                var countText = rewardSlots[index].transform.Find("Image/Image/RewardSlotCountText")?.GetComponent<TMP_Text>();

                // 텍스트를 빈 문자열로 설정하여 보이지 않게 함
                if (typeText != null) typeText.text = "";
                if (countText != null) countText.text = "";
            }
        }
        // Others
        private IEnumerator MovePanelX(RectTransform target, float toX, float duration)
        {
            Vector2 start = target.anchoredPosition;
            Vector2 end = new Vector2(toX, start.y);
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                target.anchoredPosition = Vector2.Lerp(start, end, time / duration);
                yield return null;
            }

            target.anchoredPosition = end;
        }

        private void AllPanelsOff()
        {
            inGameButtonPanel.SetActive(false);
            inGameWaveInfoPanel.SetActive(false);
            fortressEquipmentPanel.SetActive(false);
            summonPanel.SetActive(false);
            waveSelectInfoPanel.SetActive(false);
            characterInfoPanel.SetActive(false);
            questPanel.SetActive(false);
            optionsPanel.SetActive(false);
            foreach (var panel in pickPanels)
            {
                panel.SetActive(false);
            }
            foreach (var panel in fortressPickPanels)
            {
                panel?.SetActive(false);
            }

            foreach (var panel in rewardSlots)
            {
                panel?.SetActive(false);
            }
            foreach (var panel in ShopPanels)
            {
                panel?.SetActive(false);
            }
        }

    } // Scope by class PanelHide

} // namespace Root