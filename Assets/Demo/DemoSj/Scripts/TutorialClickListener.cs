using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SkyDragonHunter
{

    /// <summary>
    /// 튜토리얼 대상 오브젝트에서 클릭을 감지하는 컴포넌트
    /// </summary>
    public class TutorialClickListener : MonoBehaviour, IPointerClickHandler
    {
        // 필드 (Fields)

        private TutorialMgr tutorialMgr;    //TutorialMgr 참조를 저장할 필드 추가
        private Button button;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            if (tutorialMgr == null)
            {
                tutorialMgr = FindAnyObjectByType<TutorialMgr>();
            }
        }

        private void Awake()
        {
            button = GetComponent<Button>();

            // 수정됨: 버튼이 있으면 onClick에 연결
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    tutorialMgr?.AdvanceStepIfValid(gameObject);
                });
            }
        }

        // Public 메서드
        /// <summary>
        /// 수정됨: 외부에서 TutorialMgr 참조를 주입
        /// </summary>
        public void SetTutorialMgr(TutorialMgr mgr)
        {
            tutorialMgr = mgr;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // 버튼이 없다면 직접 처리
            if (button == null)
            {
                tutorialMgr?.AdvanceStepIfValid(gameObject);
            }
        }
        // Private 메서드
        // Others

    } // Scope by class TutorialClickListener

} // namespace Root