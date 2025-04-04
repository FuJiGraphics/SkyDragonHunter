using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter {
    public class LevelUpUI : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private int m_LevelUpInc = 1;
        [SerializeField] private Button m_Button;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            if (m_Button == null)
            {
                m_Button = GetComponentInChildren<Button>();
            }
            m_Button.onClick.AddListener(LevelUp);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.U))
            {
                LevelUp();
            }
        }
        // Public 메서드
        // Private 메서드
        private void LevelUp()
        {
            for (int i = 0; i < m_LevelUpInc; ++i)
            {
                AccountMgr.LevelUp();
            }
        }

        // Others

    } // Scope by class LevelUpUI
} // namespace SkyDragonHunter