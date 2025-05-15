using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIFortressEquipmentPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Image[] mountSlotIcons;
        [SerializeField] private Image[] equipCanonSlotIcons;
        [SerializeField] private Image[] equipRepairSlotIcons;
        [SerializeField] private Image[] equipArtifactSlotIcons;

        // 속성 (Properties)
        public Image[] MountSlotIcons => mountSlotIcons;
        public Image[] EquipCanonSlotIcons => equipCanonSlotIcons;
        public Image[] EquipRepairSlotIcons => equipRepairSlotIcons;
        public Image[] EquipArtifactSlotIcons => equipArtifactSlotIcons;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void SetCanonIcon(int slot, Sprite sprite)
            => equipCanonSlotIcons[slot].sprite = sprite;

        public void SetCanonIconColor(int slot, Color color)
            => equipCanonSlotIcons[slot].color = color;


        public void ResetCanonIcon(int slot)
            => equipCanonSlotIcons[slot].sprite = ResourcesMgr.EmptySprite;

        public void SetRepairIcon(int slot, Sprite sprite)
            => equipRepairSlotIcons[slot].sprite = sprite;


        public void SetRepairIconColor(int slot, Color color)
            => equipRepairSlotIcons[slot].color = color;

        public void ResetRepairIcon(int slot)
            => equipRepairSlotIcons[slot].sprite = ResourcesMgr.EmptySprite;


        public void SetArtifactIcon(int slot, Sprite sprite)
            => equipArtifactSlotIcons[slot].sprite = sprite;

        public void ResetArtifactIcon(int slot)
            => equipArtifactSlotIcons[slot].sprite = ResourcesMgr.EmptySprite;

        // Private 메서드
        // Others

    } // Scope by class UIFortressEquipmentPanel
} // namespace SkyDragonHunter