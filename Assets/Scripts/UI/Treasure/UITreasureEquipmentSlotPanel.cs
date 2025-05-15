using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UITreasureEquipmentSlotPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UITreasureSelectPanel m_SelectPanel;
        [SerializeField] private UITreasureEquipmentSlot[] m_Slots;
        [SerializeField] private Button m_EquipButton;

        // 속성 (Properties)
        public static List<ArtifactDummy> EquipList { get; private set; } = new();
        public ArtifactDummy ClickedDummy { get; set; }
        public UITreasureEquipmentSlot[] Slots => m_Slots;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void Unequip(ArtifactDummy artifact)
        {
            if (!IsArtifactEquipped(artifact))
                return;

            for (int i = 0; i < m_Slots.Length; ++i)
            {
                if (!m_Slots[i].IsEmpty && m_Slots[i].Value == artifact)
                {
                    m_Slots[i].Clear();
                    AccountMgr.RemoveArtifactSlot(artifact);
                    UITreasureEquipmentSlotPanel.EquipList.Remove(artifact);
                    artifact.IsEquip = false;
                    artifact.CurrentSlot = -1;

                    var uiPanel = GameMgr.FindObject<UIFortressEquipmentPanel>("UIFortressEquipmentPanel");
                    uiPanel?.ResetArtifactIcon(i);

                    break;
                }
            }

            m_SelectPanel.UpdateSortedState();
        }

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
                    UITreasureEquipmentSlotPanel.EquipList.Add(artifact);
                    artifact.IsEquip = true;
                    artifact.CurrentSlot = i;

                    var uiPanel = GameMgr.FindObject<UIFortressEquipmentPanel>("UIFortressEquipmentPanel");
                    uiPanel?.SetArtifactIcon(i, artifact.Icon);

                    break;
                }
            }

            m_SelectPanel.UpdateSortedState();
        }

        public bool IsArtifactEquipped(ArtifactDummy artifact)
        {
            bool result = false;
            
            if (artifact == null)
                return result;

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

        public void ClearClickedDummy()
            => ClickedDummy = null;

        // Private 메서드
        // Others

    } // Scope by class UITreasureEquipmentSlot
} // namespace SkyDragonHunter