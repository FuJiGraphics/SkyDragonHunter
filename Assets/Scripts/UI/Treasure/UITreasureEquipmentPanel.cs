using SkyDragonHunter.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SkyDragonHunter.UI {

    public class UITreasureEquipmentPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UITreasureSelectPanel m_UiTreasureSelect;
        [SerializeField] private UITreasureInfo m_UiTreasureInfo;
        [SerializeField] private UnityEvent m_OnDisableEvents;

        // 속성 (Properties)
        public static bool IsFusionMode { get; private set; } = false;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            m_UiTreasureSelect.UpdateSortedState();
        }

        private void OnDisable()
        {
            m_OnDisableEvents?.Invoke();
        }

        // Public 메서드
        public void AddSlot(ArtifactDummy dummy)
        {
            m_UiTreasureSelect.AddSlot(dummy);
            m_UiTreasureSelect.UpdateSortedState();
        }

        public void RemoveSlot(ArtifactDummy dummy)
        {
            m_UiTreasureSelect.RemoveSlot(dummy);
            m_UiTreasureSelect.UpdateSortedState();
        }

        public void EnableFusionMode()
            => IsFusionMode = true;

        public void DisableFusionMode()
        {
            IsFusionMode = false;
            UITreasureSlot.ClearAllSelectList();
        }

        // Private 메서드
        // Others

    } // Scope by class TreasureEquipmentPanel
} // namespace SkyDragonHunter