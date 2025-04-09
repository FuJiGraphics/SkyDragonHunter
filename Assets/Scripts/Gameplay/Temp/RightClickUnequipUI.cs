using SkyDragonHunter.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SkyDragonHunter.Test {
    public class RightClickUnequipUI : MonoBehaviour
        , IPointerDownHandler
    {
        // 필드 (Fields)
        public int slotNumber;
        public UIAssignUnitTofortressPanel slotPanel;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                slotPanel.UnequipSlot(slotNumber);
            }
        }

        // Private 메서드
        // Others

    } // Scope by class RightClickUnequipUI
} // namespace SkyDragonHunter