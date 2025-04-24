using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class UIAlertDialog : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private TextMeshProUGUI m_TitleText;
        [SerializeField] private TextMeshProUGUI m_Text;

        // 속성 (Properties)
        public string Title { get => m_TitleText.text; set => m_TitleText.text = value; }
        public string Text { get => m_Text.text; set => m_Text.text = value; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class UIAlertDialog
} // namespace SkyDragonHunter