using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            if (tutorialMgr == null)
                tutorialMgr = FindObjectOfType<TutorialMgr>(); // 또는 외부에서 다시 할당
        }

        // Public 메서드
        /// <summary>
        /// 수정됨: 외부에서 TutorialMgr 참조를 주입
        /// </summary>
        public void SetTutorialMgr(TutorialMgr mgr)
        {
            tutorialMgr = mgr;
        }

        /// <summary>
        /// 클릭되었을 때 TutorialMgr에게 알림 전송
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"[튜토리얼] 클릭 감지됨: {gameObject.name}"); // 수정됨: 클릭 확인용 디버그 출력
            tutorialMgr?.NotifyClicked(gameObject); // 수정됨: 싱글톤 사용 안 함
        }
        // Private 메서드
        // Others

    } // Scope by class TutorialClickListener

} // namespace Root