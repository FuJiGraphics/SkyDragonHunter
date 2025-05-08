using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UISkillButtons : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Button[] m_Buttons;
        [SerializeField] private Image[] m_Icons;
        [SerializeField] private Slider[] m_CooldownSliders;

        // 속성 (Properties)
        public Button[] Buttons => m_Buttons;
        public Image[] Icons => m_Icons;
        public Slider[] CooldownSliders => m_CooldownSliders;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class UISkillButtons
} // namespace SkyDragonHunter