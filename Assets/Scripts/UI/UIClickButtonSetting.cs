using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.PointerEventData;
using static UnityEngine.UI.Button;

namespace SkyDragonHunter.UI {

    public enum ClickType
    {
        LeftDown,
        LeftRelease,
        // RightDown,
        // MiddleDown,
    }

    public class UIClickButtonSetting : MonoBehaviour
        , IPointerDownHandler, IPointerUpHandler
    {
        // 필드 (Fields)
        [SerializeField] private ClickType m_ClickType = ClickType.LeftDown;

        private ButtonClickedEvent m_OnCachingClickEvents; 
        private Button m_TargetButton;
        private bool m_IsFirstUpdate = true;

        // 속성 (Properties)

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Update()
        {
            if (!m_IsFirstUpdate)
                return;
            m_IsFirstUpdate = false;

            m_TargetButton = GetComponent<Button>();
            m_OnCachingClickEvents = new Button.ButtonClickedEvent();
            m_OnCachingClickEvents = m_TargetButton.onClick;
            m_TargetButton.onClick = new Button.ButtonClickedEvent();
        }

        // Public 메서드
        public void OnPointerDown(PointerEventData eventData)
        {
            if (m_ClickType == ClickType.LeftDown)
            {
                m_OnCachingClickEvents?.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (m_ClickType == ClickType.LeftRelease)
            {
                m_OnCachingClickEvents?.Invoke();
            }
        }

        // Private 메서드
        // Others

    } // Scope by class UIClickButtonSetting
} // namespace SkyDragonHunter