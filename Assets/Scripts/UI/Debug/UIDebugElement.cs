using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    public enum IncreaseType
    {
        Coin, Diamond, CrystalLevel, MasteryLevelUp, CrewTicket, 
    }

    public class UIDebugElement : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] IncreaseType m_Type; 
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
            BigNum coinValue = new BigNum(m_Input.text);

            switch (m_Type)
            {
                case IncreaseType.Coin:
                    AccountMgr.Coin += coinValue;
                    DrawableMgr.Dialog("Alert", $"코인이 {coinValue}만큼 증가되었습니다.");
                    break;
                case IncreaseType.Diamond:
                    AccountMgr.Diamond += coinValue;
                    DrawableMgr.Dialog("Alert", $"다이아가 {coinValue}만큼 증가되었습니다.");
                    break;
                case IncreaseType.CrystalLevel:
                    if (AccountMgr.IsMaxLevel)
                    {
                        DrawableMgr.Dialog("Alert", $"계정 레벨이 최대입니다.");
                    }

                    int diffLevel = 1000 - AccountMgr.CurrentLevel;
                    if (int.TryParse(coinValue.ToString(), out var levelValue))
                    {
                        if (levelValue <= diffLevel)
                        {
                            DrawableMgr.Dialog("Alert", $"계정 레벨이 {levelValue}만큼 증가되었습니다.");
                        }
                        else
                        {
                            DrawableMgr.Dialog("Alert", $"계정 레벨이 {diffLevel}만큼 증가되었습니다.");
                        }
                        AccountMgr.LevelUp(levelValue);
                    }
                    break;
                case IncreaseType.MasteryLevelUp:
                    AccountMgr.SetItemCount(ItemType.MasteryLevelUp, coinValue);
                    DrawableMgr.Dialog("Alert", $"마스터리 재료가 {coinValue}만큼 설정되었습니다.");
                    break;
                case IncreaseType.CrewTicket:
                    AccountMgr.SetItemCount(ItemType.CrewTicket, coinValue);
                    DrawableMgr.Dialog("Alert", $"단원 티켓이 {coinValue}만큼 설정되었습니다.");
                    break;
            }            
        }
        // Private 메서드
        // Others
    } // Scope by class UICoinUp
} // namespace Root
