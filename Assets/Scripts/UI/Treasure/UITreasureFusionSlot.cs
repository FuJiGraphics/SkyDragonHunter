using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    public class UITreasureFusionSlot : MonoBehaviour
    {
        // 필드 (Fields)
        private static List<UITreasureFusionSlot> s_GenList = new();

        [SerializeField] private UITreasureFusionResult m_UiTreasureFusionResult;
        [SerializeField] private UITreasureSelectPanel m_UiTreasureSelectPanel;
        [SerializeField] private GameObject m_EmptyBG;
        [SerializeField] private GameObject m_RareBG;
        [SerializeField] private GameObject m_EpicBG;
        [SerializeField] private GameObject m_UniqueBG;
        [SerializeField] private GameObject m_LegendBG;
        [SerializeField] private Image m_Icon;

        // 속성 (Properties)
        public ArtifactDummy ArtifactDummy { get; private set; }
        public bool IsEmpty => ArtifactDummy == null;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (!s_GenList.Contains(this))
                s_GenList.Add(this);
        }

        // Public 메서드
        public void SetSlot(ArtifactDummy artifact)
        {
            ArtifactDummy = artifact;
            m_Icon.enabled = true;
            m_Icon.sprite = artifact.Icon;
            UITreasureFusionSlot.SetAllGradeBG(artifact.Grade);
            m_UiTreasureFusionResult.SetGradeBG(artifact.Grade);
        }

        public void ClearSlot()
        {
            if (ArtifactDummy == null)
                return;

            var slot = UITreasureSlot.FindSlot(ArtifactDummy);
            slot.RemoveSelectedState();
            slot.SetClickedIcon(false);
            m_Icon.sprite = ResourcesMgr.EmptySprite;
            ArtifactDummy = null;
            if (IsAllEmpty())
            {
                UITreasureFusionSlot.ClearAllGradeBG();
                m_UiTreasureFusionResult.ClearGradeBG();
            }
            m_UiTreasureSelectPanel.UpdateSortedState();
        }

        public void SetGradeBG(ArtifactGrade grade)
        {
            m_EmptyBG.SetActive(false);
            m_RareBG.SetActive(false);
            m_EpicBG.SetActive(false);
            m_UniqueBG.SetActive(false);
            m_LegendBG.SetActive(false);

            switch (grade)
            {
                case ArtifactGrade.Rare:
                    m_RareBG.SetActive(true);
                    break;
                case ArtifactGrade.Epic:
                    m_EpicBG.SetActive(true);
                    break;
                case ArtifactGrade.Unique:
                    m_UniqueBG.SetActive(true);
                    break;
                case ArtifactGrade.Legend:
                    m_LegendBG.SetActive(true);
                    break;
            }
        }

        public void ClearGradeBG()
        {
            m_RareBG.SetActive(false);
            m_EpicBG.SetActive(false);
            m_UniqueBG.SetActive(false);
            m_LegendBG.SetActive(false);
            m_EmptyBG.SetActive(true);
        }

        public static bool IsAllEmpty()
        {
            bool result = true;

            foreach (var slot in s_GenList)
            {
                if (!slot.IsEmpty)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public static bool IsFullAllSlot()
        {
            bool result = true;
            foreach (var slot in s_GenList)
            {
                if (slot.IsEmpty)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public static void ClearAllSlot()
        {
            foreach (var slot in s_GenList)
            {
                slot.ClearSlot();
            }
        }

        public static void SetAllGradeBG(ArtifactGrade grade)
        {
            foreach (var slot in s_GenList)
            {
                slot.SetGradeBG(grade);
            }
        }

        public static void ClearAllGradeBG()
        {
            foreach (var slot in s_GenList)
            {
                slot.ClearGradeBG();
            }
        }

        // Private 메서드
        // Others

    } // Scope by class UITreasureFusionSlot
} // namespace Root
