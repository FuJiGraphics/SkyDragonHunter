using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UITreasureEquipmentSlot : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Image m_Icon;
        [SerializeField] private Button m_Button;

        private Sprite m_BackUpBasicIcon;

        // 속성 (Properties)
        public ArtifactDummy Value { get; private set; } = null;
        public bool IsEmpty => Value == null;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void SetSlot(ArtifactDummy artifact)
        {
            if (artifact == null)
            {
                DrawableMgr.Dialog("Error", $"장착하려는 아티팩트가 null입니다.");
                return;
            }
            m_BackUpBasicIcon = m_Icon.sprite;
            m_Icon.sprite = artifact.Icon;
            Value = artifact;
        }

        public void Clear()
        {
            var slot = UITreasureSlot.FindSlot(Value);
            UITreasureEquipmentSlotPanel.EquipList.Remove(Value);
            slot.SetClickedIcon(false);
            AccountMgr.RemoveArtifactSlot(Value);
            Value = null;
            m_Icon.sprite = m_BackUpBasicIcon;
        }

        // Private 메서드
        // Others
    
    } // Scope by class UITreasureEquipmentSlot1
} // namespace SkyDragonHunter