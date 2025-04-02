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
            if (m_Text != null)
            {
                if (isCrystal)
                {
                    if (isHp)
                        m_Text.text = m_StringBase + AccountMgr.Crystal.hpUp.ToString();
                    else
                        m_Text.text = m_StringBase + AccountMgr.Crystal.atkUp.ToString();
                }
                else
                {
                    if (isHp)
                        m_Text.text = m_StringBase + m_AccProvider.FirstStat.MaxHealth.ToString() + " + " 
                            + AccountMgr.Crystal.hpUp.ToString() + " = " 
                            + m_Stat.MaxHealth.ToString();
                    else
                        m_Text.text = m_StringBase + m_AccProvider.FirstStat.MaxDamage.ToString() + " + "
                            + AccountMgr.Crystal.atkUp.ToString() + " = "
                            + m_Stat.MaxDamage.ToString();
                }
            }
        }

        private void Update()
        {
            if (isCrystal)
            {
                if (isHp)
                    m_Text.text = m_StringBase + AccountMgr.Crystal.hpUp.ToString();
                else
                    m_Text.text = m_StringBase + AccountMgr.Crystal.atkUp.ToString();
            }
            else
            {
                if (isHp)
                    m_Text.text = m_StringBase + m_AccProvider.FirstStat.MaxHealth.ToString() + " + "
                        + AccountMgr.Crystal.hpUp.ToString() + " = "
                        + m_Stat.MaxHealth.ToString();
                else
                    m_Text.text = m_StringBase + m_AccProvider.FirstStat.MaxDamage.ToString() + " + "
                        + AccountMgr.Crystal.atkUp.ToString() + " = "
                        + m_Stat.MaxDamage.ToString();
            }
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class AccountStatUI
} // namespace SkyDragonHunter