using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
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

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void ShowInfo(CanonDummy canonDummy)
        {
            UpdateUICanonInfoPanels(canonDummy);
            UpdateUICanonEquipEffectPanels(canonDummy);
            UpdateUICanonHoldEffectPanels(canonDummy);
            UpdateUICanonSpecialEffectPanels(canonDummy);
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
        }

        private void OnCombineButton(CanonDummy canonDummy, int currentCount, int maxCount)
        {
            if (currentCount < maxCount)
            {
                DrawableMgr.Dialog("Alert", $"합성 재료가 부족합니다. {maxCount - currentCount}");
                return;
            }

            AccountMgr.CanonGradeUp(canonDummy);
            DirtyCanonCountState(canonDummy);
        }

        private void OnLevelUp(CanonDummy canonDummy)
        {
            canonDummy.Level++;
            DirtyLevelState(canonDummy);
            DirtyCanonEffectState(canonDummy, true);
            DirtyCanonEffectState(canonDummy, false);
        }

        private void DirtyCanonTitleAndIconState(CanonDummy canonDummy)
        {
            GameObject canonInstance = canonDummy.GetCanonInstance();
            if (canonInstance.TryGetComponent<ICanonInfoProvider>(out var canonProvider))
            {
                m_CanonTitle.text = canonProvider.Name;
                m_CanonIcon.sprite = canonProvider.Icon;
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
                AlphaUnit newEqATK = BigInteger.Parse(canonData.canEqATK) * canonDummy.Level;
                AlphaUnit newEqDEF = BigInteger.Parse(canonData.canEqDEF) * canonDummy.Level;
                m_StatNameTopLeft.text = "공격력";
                m_StatDescTopLeft.text = newEqATK.ToUnit();
                m_StatNameBotLeft.text = "방어력";
                m_StatDescBotLeft.text = newEqDEF.ToUnit();

            }
            else
            {
                AlphaUnit newHoldATK = BigInteger.Parse(canonData.canHoldATK) * canonDummy.Level;
                AlphaUnit newHoldDEF = BigInteger.Parse(canonData.canHoldDEF) * canonDummy.Level;
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
            int currCount = canonDummy.Count;
            int maxCount = 5;
            m_CanonCombineButton.onClick.RemoveAllListeners();
            m_CanonCombineButton.onClick.AddListener(() => { OnCombineButton(canonDummy, currCount, maxCount); });
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

        // Others

    } // Scope by class UICanonInfoPanel
} // namespace SkyDragonHunter