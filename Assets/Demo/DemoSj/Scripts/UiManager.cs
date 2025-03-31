using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SkyDragonHunter.Test
{

    public class UiManager : MonoBehaviour
    {
        // 필드 (Fields)
        public GameObject inGameButtonPanel;
        public GameObject inGameWaveInfoPanel;
        public GameObject fortressEquipmentPanel;
        public GameObject waveControler;
        public GameObject summonPanel;
        public GameObject[] pickPanels;
        public GameObject waveSelectInfoPanel;
        public GameObject characterInfoPanel;
        public GameObject questPanel;
        public GameObject optionsPanel;
        public GameObject waveSlider;
        public GameObject waveRetryButton;
        private TestWaveController waveControlerScript;
        private RectTransform rectTransform;
        private Coroutine moveCoroutine;
        private bool isHideSummonPanel = true;
        private bool isHideCharacterInfoPanel = true;
        private bool isHideOptionsPanel = true;
        private bool isHideWaveSelectPanel = true;
        private bool isHideFortressEquipmentPanel;
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
        }

        // Public 메서드
        public void OnOffFortressEquipmentPanel()
        {
            if (isHideFortressEquipmentPanel)
            {
                inGameWaveInfoPanel.SetActive(true);
                inGameButtonPanel.SetActive(true);
                fortressEquipmentPanel.SetActive(false);
                isHideFortressEquipmentPanel = false;
            }
            else
            {
                inGameWaveInfoPanel.SetActive(false);
                inGameButtonPanel.SetActive(false);
                fortressEquipmentPanel.SetActive(true);
                isHideFortressEquipmentPanel = true;
            }
        }

        public void OnSelectWaveToGO()
        {
            waveSelectInfoPanel.SetActive(false);
            waveControlerScript.OnSelectWave(currentMissionLevel, currentZoneLevel);
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
                if (!isHideSummonPanel)
                {
                    summonPanel.SetActive(false);
                    isHideSummonPanel = true;
                }
                else
                {
                    summonPanel.SetActive(true);
                    OnPickPanel0();
                    isHideSummonPanel = false;
                }
            }
        }

        public void OnOffWaveSelectPanel()
        {
            if (waveSelectInfoPanel != null)
            {
                if (!isHideWaveSelectPanel)
                {
                    waveSelectInfoPanel.SetActive(false);
                    isHideWaveSelectPanel = true;
                }
                else
                {
                    waveSelectInfoPanel.SetActive(true);
                    isHideWaveSelectPanel = false;
                }
            }
        }

        public void OnOffCharacterInfoPanel()
        {
            if (characterInfoPanel != null)
            {
                if (!isHideCharacterInfoPanel)
                {
                    characterInfoPanel.SetActive(false);
                    isHideCharacterInfoPanel = true;
                }
                else
                {
                    characterInfoPanel.SetActive(true);
                    isHideCharacterInfoPanel = false;
                }
            }
        }

        public void OnOffHideOptionsPanel()
        {
            if (optionsPanel != null)
            {
                if (!isHideOptionsPanel)
                {
                    optionsPanel.SetActive(false);
                    isHideOptionsPanel = true;
                }
                else
                {
                    optionsPanel.SetActive(true);
                    isHideOptionsPanel = false;
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

    } // Scope by class PanelHide

} // namespace Root