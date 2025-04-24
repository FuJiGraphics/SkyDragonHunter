using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class UIMoneyRefresher : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Money Panel")]
        [SerializeField] private TextMeshProUGUI m_CoinText;
        [SerializeField] private TextMeshProUGUI m_DiamondText;

        private AlphaUnit m_PrevCoin;
        private AlphaUnit m_PrevDiamond;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Update()
        {
            if (m_PrevCoin != AccountMgr.Coin)
            {
                m_PrevCoin = AccountMgr.Coin;
                if (m_CoinText != null)
                {
                    m_CoinText.text = m_PrevCoin.ToString();
                }
            }
            if (m_PrevDiamond != AccountMgr.Diamond)
            {
                m_PrevDiamond = AccountMgr.Diamond;
                if (m_DiamondText != null)
                {
                    m_DiamondText.text = m_PrevDiamond.ToString();
                }
            }
        }
    
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class UI
} // namespace SkyDragonHunter