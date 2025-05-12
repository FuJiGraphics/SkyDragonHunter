using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UITreasureInfo : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Status Panel")]
        [SerializeField] private Image m_Icon;
        [SerializeField] private TextMeshProUGUI m_NameText;
        [SerializeField] private TextMeshProUGUI m_EffectText;

        [Header("Additional Panel")]
        [SerializeField] private Button m_AddStatChangeButton;
        [SerializeField] private int m_AddStatChangePrice;
        [SerializeField] private TextMeshProUGUI m_AddStatChangeText;
        [SerializeField] private TextMeshProUGUI[] m_SlotTexts;

        [Header("Equipment Panel")]
        [SerializeField] private UITreasureEquipmentSlotPanel m_TargetEquipSlotPanel;
        [SerializeField] private Button m_EquipButton;
        [SerializeField] private TextMeshProUGUI m_EquipButtonText;

        private ArtifactDummy m_CurrentTarget;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            if (m_CurrentTarget == null)
                gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            m_TargetEquipSlotPanel.ClickedDummy = null;
            m_CurrentTarget = null;
            m_Icon.sprite = null;
        }

        // Public 메서드
        public void ShowInfo(ArtifactDummy target)
        {
            m_CurrentTarget = target;
            m_Icon.sprite = target.Icon;
            m_NameText.text = target.Name;
            m_EffectText.text = target.ConstantStat.ToString();
            m_AddStatChangeText.text = "스탯 변경 " + m_AddStatChangePrice.ToString();
            m_TargetEquipSlotPanel.ClickedDummy = target;

            var addStatList = target.AdditionalStats;
            for (int i = 0; i < m_SlotTexts.Length; ++i)
            {
                if (i < addStatList.Length)
                    m_SlotTexts[i].text = addStatList[i].ToString();
                else
                    m_SlotTexts[i].text = "잠금 상태";
            }

            if (UITreasureEquipmentPanel.IsFusionMode)
            {
                m_EquipButton.gameObject.SetActive(false);
                m_AddStatChangeButton.gameObject.SetActive(false);
            }
            else
            {
                m_EquipButton.gameObject.SetActive(true);
                m_AddStatChangeButton.gameObject.SetActive(true);
                EquipButtonStateDirty();
                m_EquipButton.onClick.RemoveAllListeners();
                m_EquipButton.onClick.AddListener(() =>
                {
                    if (m_TargetEquipSlotPanel.IsArtifactEquipped(target))
                        m_TargetEquipSlotPanel.Unequip(target);
                    else
                        m_TargetEquipSlotPanel.Equip(target);

                    EquipButtonStateDirty();
                });
            }
        }

        public void ChangedStats()
        {
            if (m_CurrentTarget == null)
            {
                DrawableMgr.Dialog("Error", "[UITreasureInfo]: 추가 스탯 변경 실패! m_CurrentTarget이 null입니다.");
                return;
            }

            if (AccountMgr.ItemCount(Tables.ItemType.GrindingStone) < m_AddStatChangePrice)
            {
                DrawableMgr.Dialog("Alert", "연마석이 부족합니다.");
                return;
            }

            AccountMgr.AddItemCount(Tables.ItemType.GrindingStone, m_AddStatChangePrice * -1);
            m_CurrentTarget.RerollAdditionalStats();
            ShowInfo(m_CurrentTarget);
            AccountMgr.DirtyAccountAndAirshipStat();
        }

        public void EquipButtonStateDirty()
        {
            if (!m_TargetEquipSlotPanel.IsArtifactEquipped(m_TargetEquipSlotPanel.ClickedDummy))
                m_EquipButtonText.text = "장착";
            else
                m_EquipButtonText.text = "장착해제";
        }


        // Private 메서드
        // Others

    } // Scope by class UITreasureInfo
} // namespace SkyDragonHunter