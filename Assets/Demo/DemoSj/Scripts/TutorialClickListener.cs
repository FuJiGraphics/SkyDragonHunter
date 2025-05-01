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
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        public void OnPointerClick(PointerEventData eventData)
        {
            //TutorialManager.Instance?.NotifyClicked(gameObject);
        }
        // Private 메서드
        // Others

    } // Scope by class TutorialClickListener

} // namespace Root