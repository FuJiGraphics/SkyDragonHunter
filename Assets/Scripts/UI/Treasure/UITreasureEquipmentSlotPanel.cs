using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UITreasureEquipmentSlotPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UITreasureEquipmentSlot[] m_Slots;
        [SerializeField] private Button m_EquipButton;

        // 속성 (Properties)
        public ArtifactDummy ClickedDummy { get; set; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            if (ClickedDummy == null)
                gameObject.SetActive(false);
        }

        // Public 메서드
        public void Equip(ArtifactDummy artifact)
        {
            if (IsArtifactEquipped(artifact))
                return;

            for (int i = 0; i < m_Slots.Length; ++i)
            {
                if (m_Slots[i].IsEmpty)
                {
                    m_Slots[i].SetSlot(artifact);
                    AccountMgr.SetArtifactSlot(artifact, i);
                    break;
                }
            }
        }

        public bool IsArtifactEquipped(ArtifactDummy artifact)
        {
            bool result = false;
            foreach (var slot in m_Slots)
            {
                if (slot.Value == artifact)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        // Private 메서드
        // Others
    
    } // Scope by class UITreasureEquipmentSlot
} // namespace SkyDragonHunter