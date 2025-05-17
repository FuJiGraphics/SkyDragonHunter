using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Slider;

namespace SkyDragonHunter.UI {

    public class UIShieldAndHealth : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UIHealthBar m_Shield;
        [SerializeField] private UIHealthBar m_Health;
        [SerializeField] private bool m_LeftToRight = true;

        [SerializeField] private Color m_Color = Color.green;

        // 속성 (Properties)
        public UIHealthBar UIShieldBar => m_Shield;
        public UIHealthBar UIHealthBar => m_Health;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            m_Health.SetColor(m_Color);
            if (!m_LeftToRight)
            {
                m_Shield.Slider.direction = Direction.RightToLeft;
            }
        }

        // Public 메서드
        public void SetHealthColor(Color color)
            => m_Health.SetColor(color);

        public void SetDirection(bool isLeftToRight)
        {
            m_LeftToRight = isLeftToRight;
            if (!m_LeftToRight)
            {
                m_Shield.Slider.direction = Direction.RightToLeft;
                m_Health.Slider.direction = Direction.RightToLeft;
            }
            else
            {
                m_Shield.Slider.direction = Direction.LeftToRight;
                m_Health.Slider.direction = Direction.LeftToRight;
            }
        }

        // Private 메서드
        // Others

    } // Scope by class UIShieldAndHealth
} // namespace SkyDragonHunter