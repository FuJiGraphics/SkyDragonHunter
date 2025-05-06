using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    public class UIAlertArtifactInfo : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Status Panel")]
        [SerializeField] private TextMeshProUGUI m_Title;
        [SerializeField] private Image m_Icon;
        [SerializeField] private TextMeshProUGUI m_NameText;
        [SerializeField] private TextMeshProUGUI m_EffectText;

        [Header("Additional Panel")]
        [SerializeField] private TextMeshProUGUI[] m_SlotTexts;

        // 속성 (Properties)
        public string Title { get => m_Title.text; set => m_Title.text = value; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        public void ShowInfo(ArtifactDummy target)
        {
            m_Icon.sprite = target.Icon;
            m_NameText.text = target.Name;
            m_EffectText.text = target.ConstantStat.ToString();

            var addStatList = target.AdditionalStats;
            for (int i = 0; i < m_SlotTexts.Length; ++i)
            {
                if (i < addStatList.Length)
                    m_SlotTexts[i].text = addStatList[i].ToString();
                else
                    m_SlotTexts[i].text = "잠금 상태";
            }
        }

        // Private 메서드
        // Others

    } // Scope by class UITreasureInfo
} // namespace SkyDragonHunter