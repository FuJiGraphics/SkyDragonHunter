using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIRepairInfoPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("UI Repair Info Panels")]
        [SerializeField] private Image m_RepairIcon;
        [SerializeField] private TextMeshProUGUI m_RepairTitle;
        [SerializeField] private TextMeshProUGUI m_RepairLevel;
        [SerializeField] private TextMeshProUGUI m_RepairCount;
        [SerializeField] private Slider m_RepairCountSlider;
        [SerializeField] private Button m_RepairCombineButton;

        [Header("UI Repair Equip Effect Panels")]
        [SerializeField] private TextMeshProUGUI m_StatNameTopLeft;
        [SerializeField] private TextMeshProUGUI m_StatDescTopLeft;
        [SerializeField] private TextMeshProUGUI m_StatNameBotLeft;
        [SerializeField] private TextMeshProUGUI m_StatDescBotLeft;

        [Header("UI Repair Hold Effect Panels")]
        [SerializeField] private TextMeshProUGUI m_StatNameTopRight;
        [SerializeField] private TextMeshProUGUI m_StatDescTopRight;
        [SerializeField] private TextMeshProUGUI m_StatNameBotRight;
        [SerializeField] private TextMeshProUGUI m_StatDescBotRight;

        [Header("UI Repair Special Effect Panels")]
        [SerializeField] private TextMeshProUGUI m_EffectNameLeft;
        [SerializeField] private TextMeshProUGUI m_EffectDescLeft;
        [SerializeField] private TextMeshProUGUI m_EffectNameRight;
        [SerializeField] private TextMeshProUGUI m_EffectDescRight;
        [SerializeField] private TextMeshProUGUI m_EffectDetails;
        [SerializeField] private Button m_LevelUpButton;
        [SerializeField] private Button m_EquipButton;
        [SerializeField] private Button m_UnequipButton;

        private RepairDummy m_CurrentDummy = null;
        private BigNum m_CurrentNeedLevelUpCost = 0;
        private readonly string m_MaxLeveFormat = "MaxLevel";
        private readonly string m_LevelUpFormat = "LevelUp : ";

        // 속성 (Properties)
        public Button EquipButton => m_EquipButton;
        public Button UnequipButton => m_UnequipButton;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Update()
        {
            if (m_CurrentDummy != null)
            {
                var targetLevelUpText = m_LevelUpButton.gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
                targetLevelUpText.text = m_LevelUpFormat + m_CurrentNeedLevelUpCost.ToUnit();
                if (!m_CurrentDummy.IsUnlock)
                {
                    m_LevelUpButton.interactable = false;
                    m_RepairCombineButton.interactable = false;
                }
                else
                {
                    if (m_CurrentDummy.Level >= m_CurrentDummy.MaxLevel)
                    {
                        targetLevelUpText.text = m_MaxLeveFormat;
                        m_LevelUpButton.interactable = false;
                    }
                    else if (AccountMgr.Coin >= m_CurrentNeedLevelUpCost)
                    {
                        m_LevelUpButton.interactable = true;
                    }
                    else
                    {
                        m_LevelUpButton.interactable = false;
                    }
                    m_RepairCombineButton.interactable = true;
                }
            }
        }

        // Public 메서드
        public void ShowInfo(RepairDummy RepairDummy)
        {
            m_CurrentDummy = RepairDummy;
            UpdateUIRepairInfoPanels(RepairDummy);
            UpdateUIRepairEquipEffectPanels(RepairDummy);
            UpdateUIRepairHoldEffectPanels(RepairDummy);
            UpdateUIRepairSpecialEffectPanels(RepairDummy);
            DirtyCannonLevelUpNeedCost(RepairDummy);
        }

        // Private 메서드
        private void UpdateUIRepairInfoPanels(RepairDummy RepairDummy)
        {
            DirtyRepairTitleAndIconState(RepairDummy);
            DirtyLevelState(RepairDummy);
            DirtyRepairCountState(RepairDummy);
            DirtyRepairCombineButton(RepairDummy);
        }

        private void UpdateUIRepairEquipEffectPanels(RepairDummy RepairDummy)
        {
            DirtyRepairEffectState(RepairDummy, true);
        }

        private void UpdateUIRepairHoldEffectPanels(RepairDummy RepairDummy)
        {
            DirtyRepairEffectState(RepairDummy, false);
        }

        private void UpdateUIRepairSpecialEffectPanels(RepairDummy RepairDummy)
        {
            DirtyRepairLevelUpButton(RepairDummy);
            DirtyRepairEquipButton(RepairDummy);
            DirtyCannonUnequipButton(RepairDummy);
            DirtyRepairSpecialEffectStats(RepairDummy);
        }

        private void OnCombineButton(RepairDummy RepairDummy, int maxCount)
        {
            if (RepairDummy.Count < maxCount)
            {
                DrawableMgr.Dialog("Alert", $"합성 재료가 부족합니다. {maxCount - RepairDummy.Count}");
                return;
            }

            AccountMgr.RepairGradeUp(RepairDummy);
            DirtyRepairCountState(RepairDummy);
        }

        private void OnLevelUp(RepairDummy RepairDummy)
        {
            if (AccountMgr.Coin >= m_CurrentNeedLevelUpCost)
            {
                AccountMgr.Coin -= m_CurrentNeedLevelUpCost;
            }
            RepairDummy.Level++;
            DirtyLevelState(RepairDummy);
            DirtyRepairEffectState(RepairDummy, true);
            DirtyRepairEffectState(RepairDummy, false);
            DirtyCannonLevelUpNeedCost(RepairDummy);
        }

        private void DirtyRepairTitleAndIconState(RepairDummy RepairDummy)
        {
            m_RepairTitle.text = RepairDummy.GetData().RepName;
            m_RepairIcon.sprite = RepairDummy.Icon;
        }

        private void DirtyLevelState(RepairDummy RepairDummy)
        {
            int currLevel = RepairDummy.Level;
            int maxLevel = 50;
            m_RepairLevel.text = "Lv " + currLevel.ToString() + "/" + maxLevel.ToString();
        }

        private void DirtyRepairEffectState(RepairDummy RepairDummy, bool IsLeftPanel)
        {
            var RepairData = RepairDummy.GetData();

            if (IsLeftPanel)
            {
                BigNum newEqHP = new BigNum(RepairData.RepEqHP) * RepairDummy.Level;
                BigNum newEqREC = new BigNum(RepairData.RepEqREC) * RepairDummy.Level;
                m_StatNameTopLeft.text = "체력";
                m_StatDescTopLeft.text = newEqHP.ToUnit();
                m_StatNameBotLeft.text = "회복력";
                m_StatDescBotLeft.text = newEqREC.ToUnit();

            }
            else
            {
                BigNum newHoldHP = new BigNum(RepairData.RepHoldHP) * RepairDummy.Level;
                BigNum newHoldREC = new BigNum(RepairData.RepHoldREC) * RepairDummy.Level;
                m_StatNameTopRight.text = "체력";
                m_StatDescTopRight.text = newHoldHP.ToUnit();
                m_StatNameBotRight.text = "회복력";
                m_StatDescBotRight.text = newHoldREC.ToUnit();
            }
        }

        private void DirtyRepairCountState(RepairDummy RepairDummy)
        {
            int currCount = RepairDummy.Count;
            int maxCount = 5;
            m_RepairCount.text = currCount.ToString() + "/" + maxCount.ToString();
            m_RepairCountSlider.value = (float)currCount / (float)maxCount;
        }

        private void DirtyRepairCombineButton(RepairDummy RepairDummy)
        {
            int currCount = RepairDummy.Count;
            int maxCount = 5;
            m_RepairCombineButton.onClick.RemoveAllListeners();
            m_RepairCombineButton.onClick.AddListener(() => { OnCombineButton(RepairDummy, maxCount); });
        }

        private void DirtyRepairLevelUpButton(RepairDummy RepairDummy)
        {
            m_LevelUpButton.onClick.RemoveAllListeners();
            m_LevelUpButton.onClick.AddListener(() => { OnLevelUp(RepairDummy); });
        }

        private void DirtyRepairEquipButton(RepairDummy RepairDummy)
        {
            var equipPanel = GameMgr.FindObject<UIRepairEquipmentPanel>("UIRepairEquipmentPanel");
            m_EquipButton.onClick.RemoveAllListeners();
            m_EquipButton.onClick.AddListener(() => { equipPanel.OnEquip(); });
        }

        private void DirtyCannonUnequipButton(RepairDummy RepairDummy)
        {
            var equipPanel = GameMgr.FindObject<UIRepairEquipmentPanel>("UIRepairEquipmentPanel");
            m_UnequipButton.onClick.RemoveAllListeners();
            m_UnequipButton.onClick.AddListener(() => { equipPanel.OnUnequip(); });
        }

        private void DirtyRepairSpecialEffectStats(RepairDummy RepairDummy)
        {
            var RepairData = RepairDummy.GetData();

            m_EffectNameLeft.text = "체력 배율";
            m_EffectDescLeft.text = (RepairData.RepHpMultiplier * 100).ToString() + "%";
            m_EffectNameRight.text = "회복력 배율";
            m_EffectDescRight.text = (RepairData.RepRecMultiplier * 100).ToString() + "%";

            switch (RepairDummy.Type)
            {
                case RepairType.Normal:
                    m_EffectDetails.text = "없음";
                    break;
                case RepairType.Elite:
                    m_EffectDetails.text = "비공정을 조금 더 회복시킵니다.";
                    break;
                case RepairType.Shield:
                    m_EffectDetails.text = "비공정에 방어막을 부여합니다.";
                    break;
                case RepairType.Healer:
                    m_EffectDetails.text = "비공정을 크게 회복시킵니다.";
                    break;
                case RepairType.Divine:
                    m_EffectDetails.text = "비공정에 무적 상태를 부여합니다.";
                    break;
            }
        }

        private void DirtyCannonLevelUpNeedCost(RepairDummy RepairDummy)
        {
            if (RepairDummy.Level >= RepairDummy.MaxLevel)
            {
                return;
            }
            BigNum needCost = new BigNum(100) * new BigNum(Math.Pow(1.1, RepairDummy.Level));
            m_CurrentNeedLevelUpCost = needCost;
        }

        // Others

    } // Scope by class UIRepairInfoPanel
} // namespace SkyDragonHunter