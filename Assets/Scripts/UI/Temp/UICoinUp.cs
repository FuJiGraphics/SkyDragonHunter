using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    public enum CoinType
    {
        Coin, Diamond,
    }

    public class UICoinUp : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] CoinType m_Type; 
        [SerializeField] Button m_Button;
        [SerializeField] private TMP_InputField m_Input;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        private void Start()
        {
            m_Button.onClick.AddListener(OnCoinButtonClicked);
        }

        private void OnCoinButtonClicked()
        {
            if (BigInteger.TryParse(m_Input.text, out BigInteger coinValue))
            {
                switch (m_Type)
                {
                    case CoinType.Coin:
                        AccountMgr.Coin += coinValue;
                        DrawableMgr.Dialog("Alert", $"코인이 {coinValue}만큼 충전되었습니다.");
                        break;
                    case CoinType.Diamond:
                        AccountMgr.Diamond += coinValue;
                        DrawableMgr.Dialog("Alert", $"다이아가 {coinValue}만큼 충전되었습니다.");
                        break;
                }
            }
            else
            {
                DrawableMgr.Dialog("Alert", "유효하지 않은 숫자 입력입니다.");
            }
        }

        // Private 메서드
        // Others

    } // Scope by class UICoinUp
} // namespace Root
