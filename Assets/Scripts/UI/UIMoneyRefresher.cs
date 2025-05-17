using NPOI.SS.Formula.UDF;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SkyDragonHunter.UI {

    [System.Serializable]
    public class RefreshSlot
    {
        public ItemType type;
        public TextMeshProUGUI text;
    }

    public class UIMoneyRefresher : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Money Panel")]
        [SerializeField] private TextMeshProUGUI m_CoinText;
        [SerializeField] private TextMeshProUGUI m_DiamondText;
        [SerializeField] private TextMeshProUGUI m_MasteryRewardText;
        [SerializeField] private TextMeshProUGUI m_RepairerLevelUpText;
        [SerializeField] private RefreshSlot m_Custom;

        [Header("Stat Panel")]
        [SerializeField] private TextMeshProUGUI m_Damage;
        [SerializeField] private TextMeshProUGUI m_Armor;
        [SerializeField] private TextMeshProUGUI m_Health;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            AccountMgr.RemoveItemCountChangedEvent(OnChangedItem);
            AccountMgr.AddItemCountChangedEvent(OnChangedItem);

            var status = GameMgr.FindObject<CharacterStatus>("Airship");
            if (status != null)
            {
                if (m_Damage != null)
                {
                    m_Damage.text = status.MaxDamage.ToUnit();
                    status.RemoveChangedEvent(StatusChangedEventType.MaxDamage, OnChangedDamage);
                    status.AddChangedEvent(StatusChangedEventType.MaxDamage, OnChangedDamage);
                }

                if (m_Health != null)
                {
                    m_Health.text = status.MaxHealth.ToUnit();
                    status.RemoveChangedEvent(StatusChangedEventType.MaxHealth, OnChangedHealth);
                    status.AddChangedEvent(StatusChangedEventType.MaxHealth, OnChangedHealth);
                }

                if (m_Armor != null)
                {
                    m_Armor.text = status.MaxArmor.ToUnit();
                    status.RemoveChangedEvent(StatusChangedEventType.MaxArmor, OnChangedArmor);
                    status.AddChangedEvent(StatusChangedEventType.MaxArmor, OnChangedArmor);
                }
            }

            if (m_Custom != null && m_Custom.type != ItemType.None)
            {
                SetItemCount(m_Custom.type, m_Custom.text);
            }
        }

        private void OnEnable()
        {
            UpdateItemCounts();
        }

        // Public 메서드
        public void OnChangedDamage(BigNum stat)
        {
            if (m_Damage == null)
                return;

            m_Damage.text = stat.ToUnit();
        }

        public void OnChangedHealth(BigNum stat)
        {
            if (m_Health == null)
                return;

            m_Health.text = stat.ToUnit();
        }

        public void OnChangedArmor(BigNum stat)
        {
            if (m_Armor == null)
                return;

            m_Armor.text = stat.ToUnit();
        }

        public void OnChangedItemCountToUnit(BigNum stat)
        {
            if (m_Armor == null)
                return;

            m_Armor.text = stat.ToUnit();
        }

        public void OnChangedItemCountToString(BigNum stat)
        {
            if (m_Armor == null)
                return;

            m_Armor.text = stat.ToString();
        }

        public void OnChangedItem(ItemType type)
        {
            if (m_Custom != null && m_Custom.type == type && m_Custom.type != ItemType.None)
            {
                SetItemCount(m_Custom.type, m_Custom.text);
            }

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
                    if (m_RepairerLevelUpText != null)
                        m_RepairerLevelUpText.text = AccountMgr.ItemCount(ItemType.RepairExp).ToString();
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

        public void SetItemCount(ItemType type, TextMeshProUGUI target)
        {
            if (type == ItemType.Coin || type == ItemType.Diamond)
            {
                target.text = AccountMgr.ItemCount(type).ToUnit();
            }
            else
            {
                target.text = AccountMgr.ItemCount(type).ToString();
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