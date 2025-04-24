using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter {
    public class AccountStatUI : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private string m_StringBase;
        [SerializeField] private bool isCrystal;
        [SerializeField] private bool isHp;
        [SerializeField] private CharacterStatus m_Stat;
        [SerializeField] private AccountStatProvider m_AccProvider;
            
        private TextMeshProUGUI m_Text;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            m_Text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void LateUpdate()
        {
            if (isCrystal)
            {
                if (isHp)
                    m_Text.text = m_StringBase + AccountMgr.Crystal.IncreaseHealth.ToUnit();
                else
                    m_Text.text = m_StringBase + AccountMgr.Crystal.IncreaseDamage.ToUnit();
            }
            else
            {
                if (isHp)
                    m_Text.text = m_StringBase + m_AccProvider.FirstStat.MaxHealth.ToUnit() + " + "
                        + AccountMgr.Crystal.IncreaseDamage.ToUnit() + " = "
                        + m_Stat.MaxHealth.ToUnit();
                else
                    m_Text.text = m_StringBase + m_AccProvider.FirstStat.MaxDamage.ToUnit() + " + "
                        + AccountMgr.Crystal.IncreaseHealth.ToUnit() + " = "
                        + m_Stat.MaxDamage.ToUnit();
            }
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class AccountStatUI
} // namespace SkyDragonHunter