using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SkyDragonHunter.UI {

    public class TutorialRewardPanel : MonoBehaviour
    {
        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        [SerializeField] private UnityEvent m_EnableEvents;

        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            m_EnableEvents?.Invoke();
        }
    
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class TutorialRewardPanel
} // namespace SkyDragonHunter