using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class UIShieldAndHealth : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UIHealthBar m_Shield;
        [SerializeField] private UIHealthBar m_Health;

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
        }

        // Public 메서드
        public void SetHealthColor(Color color)
            => m_Health.SetColor(color);
        // Private 메서드
        // Others

    } // Scope by class UIShieldAndHealth
} // namespace SkyDragonHunter