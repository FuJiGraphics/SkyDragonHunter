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

        [SerializeField] private TextMeshProUGUI[] equipCanonSlotTexts;
        [SerializeField] private TextMeshProUGUI[] equipRepairSlotTexts;

        // 속성 (Properties)
        public Image[] MountSlotIcons => mountSlotIcons;
        public Image[] EquipCanonSlotIcons => equipCanonSlotIcons;
        public Image[] EquipRepairSlotIcons => equipRepairSlotIcons;
        public Image[] EquipArtifactSlotIcons => equipArtifactSlotIcons;

        public TextMeshProUGUI[] EquipCanonSlotTexts => equipCanonSlotTexts;
        public TextMeshProUGUI[] EquipRepairSlotTexts => equipRepairSlotTexts;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void SetCanonText(int slot, string title)
            => equipCanonSlotTexts[slot].text = title;

        public void SetCanonText(int slot, string title, string atk, string def)
        {
            string str = title + "\n" + ("공격력: " + atk) + "\n" + ("방어력: " + def);
            equipCanonSlotTexts[slot].text = str;
        }

        public void ResetCanonText(int slot)
            => equipCanonSlotTexts[slot].text = "빈 슬롯";



        public void SetRepairText(int slot, string title)
            => equipRepairSlotTexts[slot].text = title;

        public void SetRepairText(int slot, string title, string hp, string res)
        {
            string str = title + "\n" + ("체력: " + hp) + "\n" + ("회복력: " + res);
            equipRepairSlotTexts[slot].text = str;
        }

        public void ResetRepairText(int slot)
            => equipRepairSlotTexts[slot].text = "빈 슬롯";



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