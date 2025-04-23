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
        private AlphaUnit m_BasicStat;
        private AlphaUnit m_BasicCost;
        private AlphaUnit m_StatIncrease;
        private AlphaUnit m_CostIncrease;
        private AlphaUnit m_CurrentStat;
        private AlphaUnit m_NextStat;
        private AlphaUnit m_NeedCoin;
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
        public AlphaUnit BasicStat
        {
            get => m_BasicStat;
            set => m_BasicStat = value;
        }
        public AlphaUnit BasicCost
        {
            get => m_BasicCost;
            set => m_BasicCost = value;
        }
        public AlphaUnit CurrentStat
        {
            get => m_CurrentStat;
            set
            {
                m_CurrentStat = value;
                UpdatetStatText(currStatText, ref m_CurrentStat);
            }
        }
        public AlphaUnit NextStat
        {
            get => m_NextStat;
            set
            {
                m_NextStat = value;
                nextStatText.text = m_NextStat.ToString();
                UpdatetStatText(nextStatText, ref m_NextStat);
            }
        }
        public AlphaUnit NeedCoin
        {
            get => m_NeedCoin;
            set
            {
                m_NeedCoin = value;
                needCoinText.text = m_NeedCoin.ToString();
            }
        }
        public AlphaUnit StatIncrease
        {
            get => m_StatIncrease;
            set => m_StatIncrease = value;
        }
        public AlphaUnit CostIncrease
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
            CurrentStat = new AlphaUnit(0);
            BasicStat = new AlphaUnit(tableData.BasicStat);
            BasicCost = new AlphaUnit(tableData.BasicCost);
            NextStat = new AlphaUnit(tableData.BasicStat);
            NeedCoin = new AlphaUnit(tableData.BasicCost) + new AlphaUnit(tableData.CostIncrease);
            StatIncrease = new AlphaUnit(tableData.StatIncrease);
            CostIncrease = new AlphaUnit(tableData.CostIncrease);
            UpdateLevelUpArrowState();
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
            levelText.text = levelTextFormat + m_CurrentLevel.ToString();
            UpdatetStatText(currStatText, ref m_CurrentStat);
            UpdatetStatText(nextStatText, ref m_NextStat);
            needCoinText.text = m_NeedCoin.ToString();
            UpdateLevelUpArrowState();
        }

        public void SetNeedCoin(int nextIncreaseLevel)
        {
            if (nextIncreaseLevel > 0)
            {
                int nextLevel = Mathf.Min(Level + nextIncreaseLevel - 1, MaxLevel);
                int weight = 1 + (nextLevel / 100);
                AlphaUnit currCostInc = weight * nextLevel * CostIncrease;
                NeedCoin = BasicCost + currCostInc;
                UpdateLevelUpArrowState();
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
            AccountMgr.Coin = Math2DHelper.Max((AccountMgr.Coin - NeedCoin).Value, 0);

            if (levelUpType == GrowthLevelUpType.Table)
            {
                int weight = 1 + (Level / 100);
                int nextWeight = 1 + ((Level + 1) / 100);
                AlphaUnit currStatInc = weight * Level * StatIncrease;
                AlphaUnit nextStatInc = nextWeight * (Level + 1) * StatIncrease;
                AlphaUnit currCostInc = weight * Level * CostIncrease;
                CurrentStat = BasicStat + currStatInc;
                NextStat = BasicStat + nextStatInc;
                NeedCoin = BasicCost + currCostInc;
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

            UpdateLevelUpArrowState();
        }

        public void UpdateLevelUpArrowState()
        {
            if (AccountMgr.Coin >= NeedCoin && !IsMaxLevel)
                growthLevelUp.gameObject.SetActive(true);
            else
                growthLevelUp.gameObject.SetActive(false);
        }

        private void UpdatetStatText(TextMeshProUGUI textUGUI, ref AlphaUnit stats)
        {
            if (StatType == GrowthStatType.Attack)
            {
                textUGUI.text = (AccountMgr.Crystal.IncreaseDamage + stats).ToString();
            }
            else if (StatType == GrowthStatType.Health)
            {
                textUGUI.text = (AccountMgr.Crystal.IncreaseHealth + stats).ToString();
            }
            else
            {
                textUGUI.text = stats.ToString();
            }
        }

    } // class UIGrowthNode
} // namespace Root