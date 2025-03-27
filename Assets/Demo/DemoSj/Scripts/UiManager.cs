using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.Test
{

    public class UiManager : MonoBehaviour
    {
        // 필드 (Fields)
        public GameObject characterInfoPanel;
        public GameObject questPanel;
        public GameObject optionsPanel;
        private RectTransform rectTransform;
        private Coroutine moveCoroutine;
        private bool isHideCharacterInfoPanel = true;
        private bool isHideQuestPanel;
        private bool isHideOptionsPanel = true;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            rectTransform = questPanel.GetComponent<RectTransform>();
        }

        // Public 메서드
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