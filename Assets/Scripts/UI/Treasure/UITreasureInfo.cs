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
        [SerializeField] private int m_AddStatChangePrice;
        [SerializeField] private TextMeshProUGUI m_AddStatChangeText;
        [SerializeField] private TextMeshProUGUI[] m_SlotTexts;

        [Header("Equipment Panel")]
        [SerializeField] private UITreasureEquipmentSlotPanel m_TargetEquipSlotPanel;
        [SerializeField] private Button m_EquipButton;

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

        // Public 메서드
        public void ShowInfo(ArtifactDummy target)
        {
            m_CurrentTarget = target;
            m_Icon.sprite = target.Icon;
            m_NameText.text = target.Name;
            m_EffectText.text = target.ConstantStat.ToString();
            m_AddStatChangeText.text = "추가 스탯 변경 :" + m_AddStatChangePrice.ToString();

            var addStatList = target.AdditionalStats;
            for (int i = 0; i < m_SlotTexts.Length; ++i)
            {
                if (i < addStatList.Length)
                {
                    m_SlotTexts[i].text = addStatList[i].ToString();
                }
                else
                {
                    m_SlotTexts[i].text = "잠금 상태";
                }
            }

            m_EquipButton.onClick.RemoveAllListeners();
            m_EquipButton.onClick.AddListener(() => 
            {
                m_TargetEquipSlotPanel.Equip(target);
            });
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
        // Private 메서드
        // Others

    } // Scope by class UITreasureInfo
} // namespace SkyDragonHunter