using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
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
        [SerializeField] private UIGrowthNode[] growthNodes;

        private int m_LevelUpInc = 1;
        private CharacterStatus m_AirshipStats;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            if (growthNodes != null)
            {
                foreach (var node in growthNodes)
                {
                    node.UpdateLevelUpArrowState();
                }
            }
        }

        // Public 메서드
        public void Init()
        {
            foreach (var node in growthNodes)
            {
                UIGrowthNode currNode = node;
                node.Init();
                node.AddLevelUpEvent(() => { OnLevelUp(currNode); });
                node.NextArrowIcon = m_NextArrowIcon;
                node.LevelUpIcon = m_LevelUpIcon;
            }
            m_AirshipStats = GameMgr.FindObject<CharacterStatus>("Airship");
            UpdateAirshipAndAccountInfo();
        }

        public void OnLevelUp(UIGrowthNode node)
        {
            node.LevelUp(m_LevelUpInc);
            switch (node.StatType)
            {
                case GrowthStatType.Attack:
                    AccountMgr.DefaultGrowthStats.SetMaxDamage(node.CurrentStat.Value);
                    AccountMgr.DefaultGrowthStats.SetDamage(node.CurrentStat.Value);
                    break;
                case GrowthStatType.Defense:
                    AccountMgr.DefaultGrowthStats.SetMaxArmor(node.CurrentStat.Value);
                    AccountMgr.DefaultGrowthStats.SetArmor(node.CurrentStat.Value);
                    break;
                case GrowthStatType.Health:
                    AccountMgr.DefaultGrowthStats.SetMaxHealth(node.CurrentStat.Value);
                    AccountMgr.DefaultGrowthStats.SetHealth(node.CurrentStat.Value);
                    break;
                case GrowthStatType.Resilient:
                    AccountMgr.DefaultGrowthStats.SetMaxResilient(node.CurrentStat.Value);
                    AccountMgr.DefaultGrowthStats.SetResilient(node.CurrentStat.Value);
                    break;
                case GrowthStatType.CriticalMultiplier:
                    AccountMgr.DefaultGrowthStats.SetCriticalMultiplier((float)node.CurrentStat.Value);
                    break;
            }
            AccountMgr.DirtyAccountAndAirshipStat();
            UpdateAirshipAndAccountInfo();
            UpdateNodeLevelUpArrowState();
        }

        public void LevelUp1()
        {
            m_LevelUpInc = 1;
            ClearAllClickableIcons();
            m_ClickableIcons[0].SetActive(true);
            UpdateAirshipAndAccountInfo();
            foreach (var node in growthNodes)
            {
                node.SetNeedCoin(m_LevelUpInc);
            }
            UpdateNodeLevelUpArrowState();
        }
        public void LevelUp10()
        {
            m_LevelUpInc = 10;
            ClearAllClickableIcons();
            m_ClickableIcons[1].SetActive(true);
            UpdateAirshipAndAccountInfo();
            foreach (var node in growthNodes)
            {
                node.SetNeedCoin(m_LevelUpInc);
            }
            UpdateNodeLevelUpArrowState();
        }
        public void LevelUp100()
        {
            m_LevelUpInc = 100;
            ClearAllClickableIcons();
            m_ClickableIcons[2].SetActive(true);
            UpdateAirshipAndAccountInfo();
            foreach (var node in growthNodes)
            {
                node.SetNeedCoin(m_LevelUpInc);
            }
            UpdateNodeLevelUpArrowState();
        }
        public void LevelUp1000()
        {
            m_LevelUpInc = 1000;
            ClearAllClickableIcons();
            m_ClickableIcons[3].SetActive(true);
            UpdateAirshipAndAccountInfo();
            foreach (var node in growthNodes)
            {
                node.SetNeedCoin(m_LevelUpInc);
            }
            UpdateNodeLevelUpArrowState();
        }

        public void OnCrystalLevelUp()
        {
            if (growthNodes == null)
                return;

            foreach (var node in growthNodes)
            {
                node.DirtyUI();
            }
            UpdateAirshipAndAccountInfo();
        }
        
        public void OnClearAllNodes()
        {
            if (growthNodes == null)
                return;

            foreach (var node in growthNodes)
            {
                node.Clear();
            }
            AccountMgr.DefaultGrowthStats.ResetAllZero();
            AccountMgr.DirtyAccountAndAirshipStat();
            UpdateAirshipAndAccountInfo();
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
            m_AttackInfoText.text = m_AirshipStats.MaxDamage.ToString();
            m_HealthInfoText.text = m_AirshipStats.MaxHealth.ToString();
            m_AccountNicknameText.text = AccountMgr.Nickname;
            m_AccountLevelText.text = AccountMgr.CurrentLevel.ToString();
        }

        private void UpdateNodeLevelUpArrowState()
        {
            if (growthNodes != null)
            {
                foreach (var node in growthNodes)
                {
                    node.UpdateLevelUpArrowState();
                }
            }
        }

        // Others

    } // Scope by class UIGrowthPanel
} // namespace SkyDragonHunter