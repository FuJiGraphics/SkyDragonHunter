using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public enum StatType
    {
        ATK,
        HP,
        DEF,
        REG,
        CritDMG,
        BossATK,
        Count,
    }

    public enum Rank
    {
        Legendary,
        Epic,
        Unique,
        Rare,
        Elite,
        Count,
    }

    public class Artifact
    {
        // Const Fields
        private const float legendaryWeight = 0.01f;
        private const float epicWeight = 0.09f;
        private const float uniqueWeight = 0.2f;
        private const float rareWeight = 0.3f;
        private const float eliteWeight = 0.4f;

        // Fields
        public int artifactID;
        public int artifactLevel;
        public Rank artifactRank;
        public StatType ownedStatType;
        public double ownedStatValue;

        public StatType equippedStat1Type;
        public StatType equippedStat2Type;

        public Rank equippedStat1Rank;
        public Rank equippedStat2Rank;

        public double equippedStat1Value;
        public double equippedStat2Value;

        public bool isStat1Locked;
        public bool isStat2Locked;

        // Properties
        public static float TotalWeight => legendaryWeight + epicWeight + uniqueWeight + rareWeight + eliteWeight;

        #region Probabilities Calculated according to the Weight
        public static float LegendaryProbability => legendaryWeight / TotalWeight;
        public static float EpicProbability => epicWeight / TotalWeight;
        public static float UniqueProbability => uniqueWeight / TotalWeight;
        public static float RareProbability => rareWeight / TotalWeight;
        public static float EliteProbability => eliteWeight / TotalWeight;
        #endregion

        #region Actual Guided Probability
        private static float LegendaryVal => LegendaryProbability;
        private static float EpicVal => LegendaryVal + EpicProbability;
        private static float UniqueVal => EpicVal + UniqueProbability;
        private static float RareVal => UniqueVal + RareProbability;
        private static float EliteVal => RareVal + EliteProbability;
        #endregion

        // Static Methods

        /// <summary>
        /// Use this method to get random stat type;
        /// </summary>
        /// <returns></returns>
        public static StatType GetRandomStatType()
        {
            return (StatType)Random.Range(0, (int)StatType.Count);
        }

        /// <summary>
        /// Use this Method to get random rank according to the weight
        /// </summary>
        /// <returns></returns>
        public static Rank GetRandomStatRank()
        {
            var rand = Random.Range(0f, 1f);
            Rank rank = Rank.Elite;

            if (rand < RareVal)
                rank = Rank.Rare;
            if (rand < UniqueVal)
                rank = Rank.Unique;
            if (rand < EpicVal)
                rank = Rank.Epic;
            if (rand < LegendaryVal)
                rank = Rank.Legendary;

            return rank;
        }

        // Public Methods
        public void SetData(int ID, int level = 1)
        {
            var data = DataTableMgr.ArtifactTable.Get(ID);
            if(data == null)
            {
                Debug.LogError($"Artifact ID Not found in table");
            }
            artifactID = data.ID;
            artifactRank = data.ArtifactRank;
            ownedStatType = data.OwnedBonusType;
            ownedStatValue = data.InitialOwnedValue + data.IncreasingOwnedValue * level;
        }
        // Private Methods
    } // Scope by class Artifact

} // namespace Root