using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SkyDragonHunter {

    /// <summary>
    /// 버튼이 없는 튜토리얼 단계에서,
    /// 마스크(패널)를 클릭했을 때 튜토리얼을 진행시키는 역할
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class TutorialMaskClickCatcher : MonoBehaviour, IPointerClickHandler
    {
        // 필드 (Fields)
        [SerializeField] private TutorialMgr tutorialMgr; // 튜토리얼 매니저 참조
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (tutorialMgr == null)
            {
                tutorialMgr = FindObjectOfType<TutorialMgr>(); // 자동 참조 보완
            }
        }
        // Public 메서드
        /// <summary>
        /// 마스크 클릭 시 튜토리얼 매니저에 스텝 진행 요청
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (tutorialMgr != null)
            {
                tutorialMgr.OnTutorialMaskClicked(); // 구멍이 없을 때만 반응
            }
        }
        // Private 메서드
        // Others

    } // Scope by class TutorialMaskClickCatcher

} // namespace Root