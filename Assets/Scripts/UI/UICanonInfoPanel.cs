using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UICanonInfoPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("UI Canon Info Panels")]
        [SerializeField] private Image m_CanonIcon;
        [SerializeField] private TextMeshProUGUI m_CanonTitle;
        [SerializeField] private TextMeshProUGUI m_CanonLevel;
        [SerializeField] private TextMeshProUGUI m_CanonCount;
        [SerializeField] private Slider m_CanonCountSlider;
        [SerializeField] private Button m_CanonCombineButton;

        [Header("UI Canon Equip Effect Panels")]
        [SerializeField] private TextMeshProUGUI m_StatNameTopLeft;
        [SerializeField] private TextMeshProUGUI m_StatDescTopLeft;
        [SerializeField] private TextMeshProUGUI m_StatNameBotLeft;
        [SerializeField] private TextMeshProUGUI m_StatDescBotLeft;

        [Header("UI Canon Hold Effect Panels")]
        [SerializeField] private TextMeshProUGUI m_StatNameTopRight;
        [SerializeField] private TextMeshProUGUI m_StatDescTopRight;
        [SerializeField] private TextMeshProUGUI m_StatNameBotRight;
        [SerializeField] private TextMeshProUGUI m_StatDescBotRight;

        [Header("UI Canon Special Effect Panels")]
        [SerializeField] private TextMeshProUGUI m_EffectNameLeft;
        [SerializeField] private TextMeshProUGUI m_EffectDescLeft;
        [SerializeField] private TextMeshProUGUI m_EffectNameRight;
        [SerializeField] private TextMeshProUGUI m_EffectDescRight;
        [SerializeField] private TextMeshProUGUI m_EffectDetails;
        [SerializeField] private Button m_LevelUpButton;
        [SerializeField] private Button m_EquipButton;
        [SerializeField] private Button m_UnequipButton;

        private CanonDummy m_CurrentDummy = null;
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
                    m_CanonCombineButton.interactable = false;
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
                    m_CanonCombineButton.interactable = true;
                }
            }
        }

        // Public 메서드
        public void ShowInfo(CanonDummy canonDummy)
        {
            m_CurrentDummy = canonDummy;
            UpdateUICanonInfoPanels(canonDummy);
            UpdateUICanonEquipEffectPanels(canonDummy);
            UpdateUICanonHoldEffectPanels(canonDummy);
            UpdateUICanonSpecialEffectPanels(canonDummy);
            DirtyCannonLevelUpNeedCost(canonDummy);
        }

        // Private 메서드
        private void UpdateUICanonInfoPanels(CanonDummy canonDummy)
        {
            DirtyCanonTitleAndIconState(canonDummy);
            DirtyLevelState(canonDummy);
            DirtyCanonCountState(canonDummy);
            DirtyCanonCombineButton(canonDummy);
        }

        private void UpdateUICanonEquipEffectPanels(CanonDummy canonDummy)
        {
            DirtyCanonEffectState(canonDummy, true);
        }

        private void UpdateUICanonHoldEffectPanels(CanonDummy canonDummy)
        {
            DirtyCanonEffectState(canonDummy, false);
        }

        private void UpdateUICanonSpecialEffectPanels(CanonDummy canonDummy)
        {
            DirtyCanonLevelUpButton(canonDummy);
            DirtyCanonEquipButton(canonDummy);
            DirtyCannonUnequipButton(canonDummy);
            DirtyCanonSpecialEffectStats(canonDummy);
        }

        private void OnCombineButton(CanonDummy canonDummy, int maxCount)
        {
            if (canonDummy.Count < maxCount)
            {
                DrawableMgr.Dialog("Alert", $"합성 재료가 부족합니다. {maxCount - canonDummy.Count}");
                return;
            }

            AccountMgr.CanonGradeUp(canonDummy);
            DirtyCanonCountState(canonDummy);
        }

        private void OnLevelUp(CanonDummy canonDummy)
        {
            if (AccountMgr.Coin >= m_CurrentNeedLevelUpCost)
            {
                AccountMgr.Coin -= m_CurrentNeedLevelUpCost;
            }
            canonDummy.Level++;
            DirtyLevelState(canonDummy);
            DirtyCanonEffectState(canonDummy, true);
            DirtyCanonEffectState(canonDummy, false);
            DirtyCannonLevelUpNeedCost(canonDummy);
            SaveLoadMgr.CallSaveGameData();
        }

        private void DirtyCanonTitleAndIconState(CanonDummy canonDummy)
        {
            GameObject canonInstance = canonDummy.GetCanonInstance();
            if (canonInstance.TryGetComponent<ICanonInfoProvider>(out var canonProvider))
            {
                m_CanonTitle.text = canonProvider.Name;
                m_CanonIcon.sprite = canonProvider.Icon;
                m_CanonIcon.color = canonProvider.Color;
            }
        }

        private void DirtyLevelState(CanonDummy canonDummy)
        {
            int currLevel = canonDummy.Level;
            int maxLevel = 50;
            m_CanonLevel.text = "Lv " + currLevel.ToString() + "/" + maxLevel.ToString();
        }

        private void DirtyCanonEffectState(CanonDummy canonDummy, bool IsLeftPanel)
        {
            GameObject canonInstance = canonDummy.GetCanonInstance();
            CanonBase canonBase = canonInstance.GetComponent<CanonBase>();
            CanonDefinition canonData = canonBase.CanonData;

            if (IsLeftPanel)
            {
                BigNum newEqATK = new BigNum(canonData.canEqATK) * canonDummy.Level;
                BigNum newEqDEF = new BigNum(canonData.canEqDEF) * canonDummy.Level;
                m_StatNameTopLeft.text = "공격력";
                m_StatDescTopLeft.text = newEqATK.ToUnit();
                m_StatNameBotLeft.text = "방어력";
                m_StatDescBotLeft.text = newEqDEF.ToUnit();

            }
            else
            {
                BigNum newHoldATK = new BigNum(canonData.canHoldATK) * canonDummy.Level;
                BigNum newHoldDEF = new BigNum(canonData.canHoldDEF) * canonDummy.Level;
                m_StatNameTopRight.text = "공격력";
                m_StatDescTopRight.text = newHoldATK.ToUnit();
                m_StatNameBotRight.text = "방어력";
                m_StatDescBotRight.text = newHoldDEF.ToUnit();
            }
        }

        private void DirtyCanonCountState(CanonDummy canonDummy)
        {
            int currCount = canonDummy.Count;
            int maxCount = 5;
            m_CanonCount.text = currCount.ToString() + "/" + maxCount.ToString();
            m_CanonCountSlider.value = (float)currCount / (float)maxCount;
        }

        private void DirtyCanonCombineButton(CanonDummy canonDummy)
        {
            int maxCount = 5;
            m_CanonCombineButton.onClick.RemoveAllListeners();
            m_CanonCombineButton.onClick.AddListener(() => { OnCombineButton(canonDummy, maxCount); });
        }

        private void DirtyCanonLevelUpButton(CanonDummy canonDummy)
        {
            m_LevelUpButton.onClick.RemoveAllListeners();
            m_LevelUpButton.onClick.AddListener(() => { OnLevelUp(canonDummy); });
        }

        private void DirtyCanonEquipButton(CanonDummy canonDummy)
        {
            var equipPanel = GameMgr.FindObject<UICanonEquipmentPanel>("UICanonEquipmentPanel");
            m_EquipButton.onClick.RemoveAllListeners();
            m_EquipButton.onClick.AddListener(() => { equipPanel.OnEquip(); });
        }

        private void DirtyCannonUnequipButton(CanonDummy canonDummy)
        {
            var equipPanel = GameMgr.FindObject<UICanonEquipmentPanel>("UICanonEquipmentPanel");
            m_UnequipButton.onClick.RemoveAllListeners();
            m_UnequipButton.onClick.AddListener(() => { equipPanel.OnUnequip(); });
        }

        private void DirtyCanonSpecialEffectStats(CanonDummy canonDummy)
        {
            GameObject canonInstance = canonDummy.GetCanonInstance();
            CanonBase canonBase = canonInstance.GetComponent<CanonBase>();
            CanonDefinition canonData = canonBase.CanonData;
            m_EffectNameLeft.text = "공격력 배율";
            m_EffectDescLeft.text = (canonData.canATKMultiplier * 100f).ToString() + "%";
            m_EffectNameRight.text = "공격 속도";
            m_EffectDescRight.text = (1.0 / canonData.canCooldown).ToString("F1") + "/s";

            if (CanonType.Normal == canonDummy.Type)
                m_EffectDetails.text = "없음";
            else if (CanonType.Repeater == canonDummy.Type)
                m_EffectDetails.text = "포탄을 빠른 속도로 연사합니다. ";
            else
                m_EffectDetails.text = "포탄에 맞은 적에게 " + canonBase.AilmentString + " 상태를 부여합니다.";
        }

        private void DirtyCannonLevelUpNeedCost(CanonDummy canonDummy)
        {
            if (canonDummy.Level >= canonDummy.MaxLevel)
            {
                return;
            }
            BigNum needCost = new BigNum(100) * new BigNum(Math.Pow(1.1, canonDummy.Level));
            m_CurrentNeedLevelUpCost = needCost;
        }

        // Others

    } // Scope by class UICanonInfoPanel
} // namespace SkyDragonHunter