using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.Test {
    public class AccountLevelUI : MonoBehaviour
    {
        // 필드 (Fields)
        private TextMeshProUGUI m_Text;
        private int m_LastLevel;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            m_Text = GetComponentInChildren<TextMeshProUGUI>();
            if (m_Text != null)
            {
                m_Text.text = "Crystal Level:" + AccountMgr.CurrentLevel.ToString();
            }
            m_LastLevel = AccountMgr.CurrentLevel;
        }

        private void Update()
        {
            if (m_LastLevel != AccountMgr.CurrentLevel)
            {
                if (m_Text != null)
                {
                    m_Text.text = "Crystal Level:" + AccountMgr.CurrentLevel.ToString();
                }
            }
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class AccountLevelUI
} // namespace SkyDragonHunter