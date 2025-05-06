using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UITreasureSlot : MonoBehaviour
    {
        // 필드 (Fields)
        private static List<UITreasureSlot> s_GenList = new();
        private static List<UITreasureSlot> s_SelectedList = new();

        [SerializeField] private Image m_Icon;
        [SerializeField] private Button m_Button;
        [SerializeField] private Image m_RareBG;
        [SerializeField] private Image m_EpicBG;
        [SerializeField] private Image m_UniqueBG;
        [SerializeField] private Image m_LegendBG;
        [SerializeField] private Image m_Clicked;

        // 속성 (Properties)
        public ArtifactDummy ArtifactDummy { get; private set; }
        public UITreasureEquipmentSlotPanel TargetEquipPanel { get; set; }
        public UITreasureFusionPanel TargetFusionPanel { get; set; }
        public UITreasureInfo TargetInfoPanel { get; set; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (!s_GenList.Contains(this))
            {
                s_GenList.Add(this);
            }
        }

        private void OnDestroy()
        {
            s_GenList.Remove(this);
            RemoveSelectedState();
        }

        // Public 메서드
        public void SetSlot(ArtifactDummy artifact)
        {
            ArtifactDummy = artifact;
            m_Icon.sprite = artifact.Icon;
            SetGradeBG(artifact.Grade);
            m_Button.onClick.RemoveAllListeners();
            m_Button.onClick.AddListener(() =>
            {
                TargetInfoPanel.ShowInfo(artifact);
                TargetInfoPanel.gameObject.SetActive(true);
                if (!UITreasureEquipmentPanel.IsFusionMode)
                {
                    EquipState(artifact);
                }
                else
                {
                    FusionState(artifact);
                }
            });
        }

        public void RemoveSelectedState()
        {
            s_SelectedList.Remove(this);
        }

        public void SetGradeBG(ArtifactGrade grade)
        {
            m_RareBG.enabled = false;
            m_EpicBG.enabled = false;
            m_UniqueBG.enabled = false;
            m_LegendBG.enabled = false;

            switch (grade)
            {
                case ArtifactGrade.Rare:
                    m_RareBG.enabled = true;
                    break;
                case ArtifactGrade.Epic:
                    m_EpicBG.enabled = true;
                    break;
                case ArtifactGrade.Unique:
                    m_UniqueBG.enabled = true;
                    break;
                case ArtifactGrade.Legend:
                    m_LegendBG.enabled = true;
                    break;
            }
        }

        public void SetClickedIcon(bool enabled)
        {
            m_Clicked.gameObject.SetActive(enabled);
        }

        public void ClearAllClickedIcons()
        {
            foreach (var slot in s_GenList)
            {
                slot.SetClickedIcon(false);
            }
        }

        // Private 메서드
        private void EquipState(ArtifactDummy artifact)
        {
            TargetEquipPanel.ClickedDummy = artifact;
            TargetEquipPanel.gameObject.SetActive(true);
            ClearAllClickedIcons();
            SetClickedIcon(true);
        }

        private void FusionState(ArtifactDummy artifact)
        {
            if (TargetFusionPanel.HasArtifact(artifact))
            {
                TargetFusionPanel.RemoveSlot(artifact);
                s_SelectedList.Remove(this);
                SetClickedIcon(false);
            }
            else if (!TargetFusionPanel.IsFull)
            {
                if (TargetFusionPanel.CurrentGrade != ArtifactGrade.None &&
                    TargetFusionPanel.CurrentGrade != artifact.Grade)
                {
                    DrawableMgr.Dialog("Alert", "등급이 일치하지 않습니다.");
                    TargetInfoPanel.gameObject.SetActive(false);
                    SetClickedIcon(false);
                }
                else if (TargetEquipPanel.IsArtifactEquipped(artifact))
                {
                    DrawableMgr.Dialog("Alert", $"장착 중인 보물은 합성할 수 없습니다. {artifact}");
                }
                else
                {
                    TargetFusionPanel.SetSlot(artifact);
                    SetClickedIcon(true);
                    s_SelectedList.Add(this);
                    SortedSelectList();
                }
            }
        }

        // Others
        public static void SortedSelectList()
        {
            if (s_SelectedList.Count <= 0)
                return;

            for (int i = s_SelectedList.Count - 1; i >= 0; i--)
            {
                s_SelectedList[i]?.transform.SetAsFirstSibling();
            }
        }

        public static void ClearAllSelectList()
        {
            s_SelectedList.Clear();
        }

        public static UITreasureSlot FindSlot(ArtifactDummy artifact)
        {
            UITreasureSlot result = null;
            foreach (var slot in s_GenList)
            {
                if (slot.ArtifactDummy == artifact)
                {
                    result = slot;
                    break;
                }
            }
            return result;
        }

    } // Scope by class UITreasureEquipementSlot
} // namespace SkyDragonHunter