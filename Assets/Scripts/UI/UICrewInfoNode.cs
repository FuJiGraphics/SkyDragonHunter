using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public struct CrewNode
    {
        public GameObject crewNode;
        public GameObject crewInstance;
    }

    public class UICrewInfoNode : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private GameObject m_UIDetails;
        [SerializeField] private Button m_UIDetailsButton;

        // 속성 (Properties)
        public GameObject UIDetails => m_UIDetails;
        public Button UIDetailsButton => m_UIDetailsButton;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class UICrewInfoNode
} // namespace SkyDragonHunter