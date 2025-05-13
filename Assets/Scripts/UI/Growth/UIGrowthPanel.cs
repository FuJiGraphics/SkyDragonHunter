using NPOI.SS.Formula.Functions;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIGrowthPanel : MonoBehaviour
        , ICrystalLevelUpHandler
    {
        // 필드 (Fields)
        [Header("Default Growth Panel Controller")]
        [SerializeField] private GameObject[] m_ClickableIcons;
        [SerializeField] private Sprite m_NextArrowIcon;
        [SerializeField] private Sprite m_LevelUpIcon;
        [SerializeField] private TextMeshProUGUI m_AccountNicknameText;
        [SerializeField] private TextMeshProUGUI m_AccountLevelText;
        [SerializeField] private Image m_AttackInfoIcon;
        [SerializeField] private TextMeshProUGUI m_AttackInfoText;
        [SerializeField] private Image m_HealthInfoIcon;
        [SerializeField] private TextMeshProUGUI m_HealthInfoText;
        [SerializeField] private UIGrowthNode[] m_GrowthNodes;

        [Header("Level Up Panel")]
        [SerializeField] private Button m_LevelUpButton;
        [SerializeField] private Slider m_ExpSlider;

        private int m_LevelUpInc = 1;
        private CharacterStatus m_AirshipStats;

        // 속성 (Properties)
        public UIGrowthNode[] GrowthNodes => m_GrowthNodes;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            if (m_GrowthNodes != null)
            {
                foreach (var node in m_GrowthNodes)
                {
                    node.UpdateLevelUpArrowState();
                }
            }
        }

        private void OnDestroy()
        {
            m_AirshipStats.RemoveChangedEvent(StatusChangedEventType.MaxDamage, OnChangedAirshipMaxDamage);
            m_AirshipStats.RemoveChangedEvent(StatusChangedEventType.MaxHealth, OnChangedAirshipMaxHealth);
            AccountMgr.RemoveNicknameChangedEvent(OnChangedAccountNickname);
            AccountMgr.RemoveLevelUpEvent(OnAccountLevelUpEvent);
        }

        // Public 메서드
        public void Init()
        {
            foreach (var node in m_GrowthNodes)
            {
                UIGrowthNode currNode = node;
                node.Init();
                node.AddLevelUpEvent(() => { OnLevelUp(currNode); });
                node.NextArrowIcon = m_NextArrowIcon;
                node.LevelUpIcon = m_LevelUpIcon;
            }

            AccountMgr.RemoveNicknameChangedEvent(OnChangedAccountNickname);
            AccountMgr.AddNicknameChangedEvent(OnChangedAccountNickname);
            AccountMgr.RemoveLevelUpEvent(OnAccountLevelUpEvent);
            AccountMgr.AddLevelUpEvent(OnAccountLevelUpEvent);
            AccountMgr.RemoveExpChangedEvent(OnChangedAccountExp);
            AccountMgr.AddExpChangedEvent(OnChangedAccountExp);
            m_AccountLevelText.text = "Lv: " + AccountMgr.CurrentLevel.ToString();
            m_AccountNicknameText.text = AccountMgr.Nickname;
            m_ExpSlider.value = (float)((double)AccountMgr.CurrentExp / (double)AccountMgr.NextExp);
            m_LevelUpButton.onClick.AddListener(() => { AccountMgr.LevelUp(1); });
            m_AirshipStats = GameMgr.FindObject<CharacterStatus>("Airship");
            m_AirshipStats.RemoveChangedEvent(StatusChangedEventType.MaxDamage, OnChangedAirshipMaxDamage);
            m_AirshipStats.RemoveChangedEvent(StatusChangedEventType.MaxHealth, OnChangedAirshipMaxHealth);
            m_AirshipStats.AddChangedEvent(StatusChangedEventType.MaxDamage, OnChangedAirshipMaxDamage);
            m_AirshipStats.AddChangedEvent(StatusChangedEventType.MaxHealth, OnChangedAirshipMaxHealth);
        }

        public UIGrowthNode FindNode(int tableId)
        {
            UIGrowthNode result = null;
            foreach (var node in m_GrowthNodes)
            {
                if (node.ID == tableId)
                {
                    result = node;
                    break;
                }
            }
            return result;
        }

        public void OnChangedAccountNickname(string nickname)
        {
            m_AccountNicknameText.text = nickname;
        }

        public void OnAccountLevelUpEvent()
        {
            m_AccountLevelText.text = "Lv: " + AccountMgr.CurrentLevel.ToString();
        }

        public void OnChangedAirshipMaxDamage(BigNum stats)
        {
            m_AttackInfoText.text = stats.ToUnit();
        }

        public void OnChangedAirshipMaxHealth(BigNum stats)
        {
            m_HealthInfoText.text = stats.ToUnit();
        }

        public void OnChangedAccountExp(BigNum exp)
        {
            double value = (double)exp / (double)AccountMgr.NextExp;
            m_ExpSlider.value = (float)value;
            if (Mathf.Approximately(m_ExpSlider.value, m_ExpSlider.maxValue))
            {
                m_LevelUpButton.interactable = true;
            }
            else
            {
                m_LevelUpButton.interactable = false;
            }
        }

        public void OnLevelUp(UIGrowthNode node)
        {
            node.LevelUp(m_LevelUpInc);
            // TODO: 세이브용 각 스탯 현재 레벨 상태 저장 필요함

            switch (node.StatType)
            {
                case GrowthStatType.Attack:
                    AccountMgr.DefaultGrowthStats.SetMaxDamage(node.CurrentStat);
                    AccountMgr.DefaultGrowthStats.SetDamage(node.CurrentStat);
                    break;
                case GrowthStatType.Defense:
                    AccountMgr.DefaultGrowthStats.SetMaxArmor(node.CurrentStat);
                    AccountMgr.DefaultGrowthStats.SetArmor(node.CurrentStat);
                    break;
                case GrowthStatType.Health:
                    AccountMgr.DefaultGrowthStats.SetMaxHealth(node.CurrentStat);
                    AccountMgr.DefaultGrowthStats.SetHealth(node.CurrentStat);
                    break;
                case GrowthStatType.Resilient:
                    AccountMgr.DefaultGrowthStats.SetMaxResilient(node.CurrentStat);
                    AccountMgr.DefaultGrowthStats.SetResilient(node.CurrentStat);
                    break;
                case GrowthStatType.CriticalMultiplier:
                    // TODO: AlphaUnit Convert
                    //AccountMgr.DefaultGrowthStats.SetCriticalMultiplier((float)node.CurrentStat.Value);
                    AccountMgr.DefaultGrowthStats.SetCriticalMultiplier((float)node.CurrentStat);
                    // ~TODO
                    break;
            }
            AccountMgr.DirtyAccountAndAirshipStat();
            UpdateNodeLevelUpArrowState();
        }

        public void LevelUp1()
        {
            if (m_LevelUpInc == 1)
                return;

            m_LevelUpInc = 1;
            ClearAllClickableIcons();
            m_ClickableIcons[0].SetActive(true);
            foreach (var node in m_GrowthNodes)
            {
                node.SetNextStatInfo(m_LevelUpInc);
                node.SetNeedCoin(m_LevelUpInc);
                node.DirtyUI();
            }
            UpdateNodeLevelUpArrowState();
        }
        public void LevelUp10()
        {
            if (m_LevelUpInc == 10)
                return;

            m_LevelUpInc = 10;
            ClearAllClickableIcons();
            m_ClickableIcons[1].SetActive(true);
            foreach (var node in m_GrowthNodes)
            {
                node.SetNextStatInfo(m_LevelUpInc);
                node.SetNeedCoin(m_LevelUpInc);
                node.DirtyUI();
            }
            UpdateNodeLevelUpArrowState();
        }
        public void LevelUp100()
        {
            if (m_LevelUpInc == 100)
                return;

            m_LevelUpInc = 100;
            ClearAllClickableIcons();
            m_ClickableIcons[2].SetActive(true);
            foreach (var node in m_GrowthNodes)
            {
                node.SetNextStatInfo(m_LevelUpInc);
                node.SetNeedCoin(m_LevelUpInc);
                node.DirtyUI();
            }
            UpdateNodeLevelUpArrowState();
        }
        public void LevelUp1000()
        {
            if (m_LevelUpInc == 1000)
                return;

            m_LevelUpInc = 1000;
            ClearAllClickableIcons();
            m_ClickableIcons[3].SetActive(true);
            foreach (var node in m_GrowthNodes)
            {
                node.SetNextStatInfo(m_LevelUpInc);
                node.SetNeedCoin(m_LevelUpInc);
                node.DirtyUI();
            }
            UpdateNodeLevelUpArrowState();
        }

        public void OnCrystalLevelUp()
        {
            if (m_GrowthNodes == null)
                return;

            foreach (var node in m_GrowthNodes)
            {
                node.DirtyUI();
            }
        }
        
        public void OnClearAllNodes()
        {
            if (m_GrowthNodes == null)
                return;

            foreach (var node in m_GrowthNodes)
            {
                node.Clear();
            }
            AccountMgr.DefaultGrowthStats.ResetAllZero();
            AccountMgr.DirtyAccountAndAirshipStat();
        }

        // Private 메서드
        private void ClearAllClickableIcons()
        {
            foreach (var clickable in m_ClickableIcons)
            {
                clickable.SetActive(false);
            }
        }

        private void UpdateAirshipAndAccountInfo()
        {
            m_AttackInfoText.text = m_AirshipStats.MaxDamage.ToUnit();
            m_HealthInfoText.text = m_AirshipStats.MaxHealth.ToUnit();
            m_AccountNicknameText.text = AccountMgr.Nickname;
            m_AccountLevelText.text = AccountMgr.CurrentLevel.ToString();
        }

        private void UpdateNodeLevelUpArrowState()
        {
            if (m_GrowthNodes != null)
            {
                foreach (var node in m_GrowthNodes)
                {
                    node.UpdateLevelUpArrowState();
                }
            }
        }
        // Others
    } // Scope by class UIGrowthPanel
} // namespace SkyDragonHunter