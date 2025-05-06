using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UITreasureFusionResult : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private GameObject m_EmptyBG;
        [SerializeField] private GameObject m_RareBG;
        [SerializeField] private GameObject m_EpicBG;
        [SerializeField] private GameObject m_UniqueBG;
        [SerializeField] private GameObject m_LegendBG;
        [SerializeField] private Image m_Icon;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
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
                    m_EpicBG.SetActive(true);
                    break;
                case ArtifactGrade.Epic:
                    m_UniqueBG.SetActive(true);
                    break;
                case ArtifactGrade.Unique:
                    m_LegendBG.SetActive(true);
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

        // Private 메서드
        // Others

    } // Scope by class UITreasureFusionResult
} // namespace Root
