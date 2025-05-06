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
        // �ʵ� (Fields)
        [Header("Status Panel")]
        [SerializeField] private TextMeshProUGUI m_Title;
        [SerializeField] private Image m_Icon;
        [SerializeField] private TextMeshProUGUI m_NameText;
        [SerializeField] private TextMeshProUGUI m_EffectText;

        [Header("Additional Panel")]
        [SerializeField] private TextMeshProUGUI[] m_SlotTexts;

        // �Ӽ� (Properties)
        public string Title { get => m_Title.text; set => m_Title.text = value; }

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)

        // Public �޼���
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
                    m_SlotTexts[i].text = "��� ����";
            }
        }

        // Private �޼���
        // Others

    } // Scope by class UITreasureInfo
} // namespace SkyDragonHunter