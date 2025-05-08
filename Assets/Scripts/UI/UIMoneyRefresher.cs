using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
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
        [SerializeField] private TextMeshProUGUI m_MasteryRewardText;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            AccountMgr.RemoveItemCountChangedEvent(OnChangedItem);
            AccountMgr.AddItemCountChangedEvent(OnChangedItem);
        }

        private void OnEnable()
        {
            UpdateItemCounts();
        }

        // Public 메서드
        public void OnChangedItem(ItemType type)
        {
            switch (type)
            {
                case ItemType.None:
                    break;
                case ItemType.Coin:
                    if (m_CoinText != null)
                        m_CoinText.text = AccountMgr.Coin.ToUnit();
                    break;
                case ItemType.Diamond:
                    if (m_DiamondText != null)
                        m_DiamondText.text = AccountMgr.Diamond.ToUnit();
                    break;
                case ItemType.Spoils:
                    break;
                case ItemType.CrewTicket:
                    break;
                case ItemType.CanonTicket:
                    break;
                case ItemType.RepairTicket:
                    break;
                case ItemType.Food:
                    break;
                case ItemType.Gear:
                    break;
                case ItemType.RepairExp:
                    break;
                case ItemType.WaveDungeonTicket:
                    break;
                case ItemType.BossDungeonTicket:
                    break;
                case ItemType.SandbagDungeonTicket:
                    break;
                case ItemType.Wood:
                    break;
                case ItemType.Brick:
                    break;
                case ItemType.Steel:
                    break;
                case ItemType.MasteryLevelUp:
                    if (m_MasteryRewardText != null)
                        m_MasteryRewardText.text = AccountMgr.ItemCount(ItemType.MasteryLevelUp).ToString();
                    break;
                case ItemType.ExpansionMaterial:
                    break;
                case ItemType.GrindingStone:
                    break;
            }
        }

        public void UpdateItemCounts()
        {
            foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
            {
                OnChangedItem(itemType);
            }
        }
        // Private 메서드
        // Others

    } // Scope by class UI
} // namespace SkyDragonHunter