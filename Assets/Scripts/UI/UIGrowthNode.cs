using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkyDragonHunter.UI{

    public enum GrowthStatType
    {
        Attack, Defense, Health, Resilient, CriticalMultiplier
    };

    public enum GrowthLevelUpType
    {
        Table, Curve
    }

    [System.Serializable]
    public class UIGrowthNode
    {
        [SerializeField] private int growthTableId;
        [SerializeField] private GrowthLevelUpType levelUpType;
        [SerializeField] private AnimationCurve statCurve;
        [SerializeField] private AnimationCurve costCurve;
        [SerializeField] private Image icon;
        [SerializeField] private Image nextArrowIcon;
        [SerializeField] private Image levelUpIcon;
        [SerializeField] private TextMeshProUGUI statNameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private string levelTextFormat;
        [SerializeField] private TextMeshProUGUI currStatText;
        [SerializeField] private TextMeshProUGUI nextStatText;
        [SerializeField] private TextMeshProUGUI needCoinText;
        [SerializeField] private Button levelUpButton;
        [SerializeField] private UIGrowthLevelUp growthLevelUp;

        private int m_CurrentLevel = 0;
        private BigNum m_BasicStat;
        private BigNum m_BasicCost;
        private BigNum m_StatIncrease;
        private BigNum m_CostIncrease;
        private BigNum m_CurrentStat;
        private BigNum m_NextStat;
        private BigNum m_NeedCoin;
        private bool m_FirstLevelUp = true;

        public GrowthStatType StatType { get; set; }
        public int MaxLevel { get; set; } = 10;
        public bool IsMaxLevel => Level >= MaxLevel;
        
        public Sprite Icon
        {
            get => this.icon.sprite;
            set => this.icon.sprite = value;
        }

        public Sprite NextArrowIcon
        {
            get => nextArrowIcon.sprite;
            set => nextArrowIcon.sprite = value;
        }

        public Sprite LevelUpIcon
        {
            get => levelUpIcon.sprite;
            set => levelUpIcon.sprite = value;
        }

        public string StatName
        {
            get => statNameText.text;
            set => statNameText.text = value;
        }
        public int Level
        {
            get => m_CurrentLevel;
            set
            {
                m_CurrentLevel = value;
                levelText.text = levelTextFormat + m_CurrentLevel.ToString();
            }
        }
        public BigNum BasicStat
        {
            get => m_BasicStat;
            set => m_BasicStat = value;
        }
        public BigNum BasicCost
        {
            get => m_BasicCost;
            set => m_BasicCost = value;
        }
        public BigNum CurrentStat
        {
            get => m_CurrentStat;
            set
            {
                m_CurrentStat = value;
                UpdatetStatText(currStatText, ref m_CurrentStat);
            }
        }
        public BigNum NextStat
        {
            get => m_NextStat;
            set
            {
                m_NextStat = value;
                nextStatText.text = m_NextStat.ToUnit();
                UpdatetStatText(nextStatText, ref m_NextStat);
            }
        }
        public BigNum NeedCoin
        {
            get => m_NeedCoin;
            set
            {
                m_NeedCoin = value;
                needCoinText.text = m_NeedCoin.ToUnit();
            }
        }
        public BigNum StatIncrease
        {
            get => m_StatIncrease;
            set => m_StatIncrease = value;
        }
        public BigNum CostIncrease
        {
            get => m_CostIncrease;
            set => m_CostIncrease = value;
        }

        public void AddLevelUpEvent(UnityAction action)
            => levelUpButton.onClick.AddListener(action);

        public void Init()
        {
            var tableData = DataTableMgr.DefaultGrowthTable.Get(growthTableId);
            Level = 1;
            MaxLevel = tableData.MaxLevel;
            StatType = (GrowthStatType)tableData.StatType;
            StatName = tableData.StatName;
            CurrentStat = new BigNum(0);
            BasicStat = new BigNum(tableData.BasicStat);
            NextStat = new BigNum(tableData.BasicStat);
            StatIncrease = new BigNum(tableData.StatIncrease);
            BasicCost = new BigNum(tableData.BasicCost);
            NeedCoin = new BigNum(tableData.BasicCost);
            CostIncrease = new BigNum(tableData.CostIncrease);
            UpdateLevelUpArrowState();
            DirtyUI();
        }

        public void Clear()
        {
            m_FirstLevelUp = true;
            Init();
            DirtyUI();
            levelUpButton.onClick?.Invoke();
        }

        public void DirtyUI()
        {
            BigNum next = m_CurrentStat + m_NextStat;
            levelText.text = levelTextFormat + m_CurrentLevel.ToString();
            UpdatetStatText(currStatText, ref m_CurrentStat);
            UpdatetStatText(nextStatText, ref next);
            needCoinText.text = m_NeedCoin.ToUnit();
            UpdateLevelUpArrowState();
        }

        public void SetNextStatInfo(int nextIncreaseLevel)
        {
            NextStat = 0;
            for (int i = 0; i < nextIncreaseLevel; ++i)
            {
                NextStat += CalculateNextStat(Level + i);
            }
        }

        public void SetNeedCoin(int nextIncreaseLevel)
        {
            NeedCoin = 0;
            for (int i = 0; i < nextIncreaseLevel; ++i)
            {
                NeedCoin += CalculateNeedCoin(Level + i);
            }
        }

        public void LevelUp(int increase)
        {
            if (increase <= 0)
                return;
            if (IsMaxLevel)
                return;
            if (AccountMgr.Coin < NeedCoin)
                return;

            Level = Mathf.Min(Level + increase, MaxLevel);
            CurrentStat += NextStat;
            AccountMgr.Coin = Math2DHelper.Max((AccountMgr.Coin - NeedCoin), 0);

            if (levelUpType == GrowthLevelUpType.Table)
            {
                SetNextStatInfo(increase);
                SetNeedCoin(increase);
            }
            else
            {
                float t = Mathf.Clamp01((float)Level / MaxLevel);
                float nextT = Mathf.Clamp01((float)(Level + 1) / MaxLevel);

                float statMultiplier = statCurve.Evaluate(t);
                float nextStatMultiplier = statCurve.Evaluate(nextT);
                float costMultiplier = costCurve.Evaluate(t);

                CurrentStat = BasicStat * statMultiplier;
                NextStat = BasicStat * nextStatMultiplier;
                NeedCoin = BasicCost * costMultiplier;
            }

            DirtyUI();
            UpdateLevelUpArrowState();
        }

        public void UpdateLevelUpArrowState()
        {
            if (AccountMgr.Coin >= NeedCoin && !IsMaxLevel)
                growthLevelUp.gameObject.SetActive(true);
            else
                growthLevelUp.gameObject.SetActive(false);
        }

        private void UpdatetStatText(TextMeshProUGUI textUGUI, ref BigNum stats)
        {
            if (StatType == GrowthStatType.Attack)
            {
                textUGUI.text = (AccountMgr.Crystal.IncreaseDamage + stats).ToUnit();
            }
            else if (StatType == GrowthStatType.Health)
            {
                textUGUI.text = (AccountMgr.Crystal.IncreaseHealth + stats).ToUnit();
            }
            else
            {
                textUGUI.text = stats.ToUnit();
            }
        }

        private BigNum CalculateNextStat(int currLevel)
        {
            int level = Mathf.Min(currLevel - 1, MaxLevel);
            BigNum total = BasicStat;

            int fullSections = level / 100;
            int remainder = level % 100;

            for (int i = 0; i < fullSections; i++)
            {
                int weight = i + 1;
                total += StatIncrease * weight * 100;
            }

            int currentWeight = fullSections + 1;
            total += StatIncrease * currentWeight * remainder;

            return total;
        }

        private BigNum CalculateNeedCoin(int currLevel)
        {
            int level = Mathf.Min(currLevel - 1, MaxLevel);
            BigNum total = BasicCost;

            int fullSections = level / 100;
            int remainder = level % 100;

            for (int i = 0; i < fullSections; i++)
            {
                int weight = i + 1; 
                total += CostIncrease * weight * 100;
            }

            int currentWeight = fullSections + 1;
            total += CostIncrease * currentWeight * remainder;

            return total;
        }
    } // class UIGrowthNode
} // namespace Root