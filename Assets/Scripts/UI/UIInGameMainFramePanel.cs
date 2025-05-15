using SkyDragonHunter.Database;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIInGameMainFramePanel : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Account Info Panel")]
        [SerializeField] private TextMeshProUGUI m_Nickname;
        [SerializeField] private TextMeshProUGUI m_Level;
        [SerializeField] private Slider m_Exp;

        [Header("Crystal Info Panel")]
        [SerializeField] private Image m_AtkIcon;
        [SerializeField] private TextMeshProUGUI m_AtkText;
        [SerializeField] private Image m_HpIcon;
        [SerializeField] private TextMeshProUGUI m_HpText;

        [Header("Top Right Panel")]
        [SerializeField] private TextMeshProUGUI m_CoinText;
        [SerializeField] private TextMeshProUGUI m_DiamondText;
        [SerializeField] private TextMeshProUGUI m_SpoilsText;

        // 속성 (Properties)
        public string Nickname { get => m_Nickname.text; set => m_Nickname.text = value; }
        public string Level { get => m_Level.text; set => m_Level.text = "Lv: " + value; }
        public Slider Exp { get => m_Exp; set => m_Exp = value; }

        public Image AtkIcon { get => m_AtkIcon; set => m_AtkIcon = value; }
        public string AtkText { get => m_AtkText.text; set => m_AtkText.text = value; }
        public Image HpIcon { get => m_HpIcon; set => m_HpIcon = value; }
        public string HpText { get => m_HpText.text; set => m_HpText.text = value; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class UIInGameMainFramePanel
} // namespace SkyDragonHunter