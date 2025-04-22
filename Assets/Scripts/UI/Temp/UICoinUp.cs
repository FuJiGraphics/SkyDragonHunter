using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter
{
    public class UICoinUp : MonoBehaviour
    {
        // 필드 (Fields)
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
                AccountMgr.Coin += coinValue;
                Debug.Log($"코인이 {coinValue}만큼 충전되었습니다.");
            }
            else
            {
                Debug.LogWarning("유효하지 않은 숫자 입력입니다.");
            }
        }

        // Private 메서드
        // Others

    } // Scope by class UICoinUp
} // namespace Root
