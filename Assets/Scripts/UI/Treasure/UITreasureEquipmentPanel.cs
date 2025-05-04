using SkyDragonHunter.Gameplay;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class UITreasureEquipmentPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UITreasureSelectPanel m_UiTreasureSelect;
        [SerializeField] private UITreasureInfo m_UiTreasureInfo;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void AddSlot(ArtifactDummy dummy)
        {
            m_UiTreasureSelect.AddSlot(dummy);
        }

        public void RemoveSlot(ArtifactDummy dummy)
        {
            m_UiTreasureSelect.RemoveSlot(dummy);
        }

        // Private 메서드
        // Others

    } // Scope by class TreasureEquipmentPanel
} // namespace SkyDragonHunter