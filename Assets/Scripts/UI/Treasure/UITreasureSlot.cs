using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Tables;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UITreasureSlot : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Image m_Icon;
        [SerializeField] private Button m_Button;
        [SerializeField] private Image m_RareBG;
        [SerializeField] private Image m_EpicBG;
        [SerializeField] private Image m_UniqueBG;
        [SerializeField] private Image m_LegendBG;

        // 속성 (Properties)
        public UITreasureEquipmentSlotPanel TargetEquipPanel { get; set; }
        public UITreasureInfo TargetInfoPanel { get; set; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void SetSlot(ArtifactDummy artifact)
        {
            m_Icon.sprite = artifact.Icon;
            SetGradeBG(artifact.Grade);
            m_Button.onClick.RemoveAllListeners();
            m_Button.onClick.AddListener(() =>
            {
                TargetEquipPanel.ClickedDummy = artifact;
                TargetInfoPanel.ShowInfo(artifact);
                TargetEquipPanel.gameObject.SetActive(true);
                TargetInfoPanel.gameObject.SetActive(true);
            });
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
        // Private 메서드
        // Others

    } // Scope by class UITreasureEquipementSlot
} // namespace SkyDragonHunter