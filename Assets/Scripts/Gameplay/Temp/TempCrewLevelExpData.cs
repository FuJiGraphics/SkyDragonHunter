using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SkyDragonHunter.Temp {
    
    public class TempCrewLevelExpData
    {
        private const int maxLevel = 100;

        private CrewTableData crewData;
        private int level;
        private bool isUnlocked;
        private int rank;
        private int count;
        private BigNum accumulatedExp;

        private UnityEvent leveledUpEvent;

        public CrewTableData CrewData => crewData;
        public int Level => level;
        public bool IsUnlocked
        {
            get => isUnlocked;
            set => isUnlocked = value;
        }
        public int Rank => rank;
        public int Count => count;
        public BigNum AccumulatedExp => accumulatedExp;
        public BigNum RequiredExp
        {
            get
            {
                var exp = DataTableMgr.CrewLevelTable.GetRequiredEXP(crewData.UnitGrade, level);
                if(exp == null)
                {
                    Debug.LogError($"ExpData Not Found with [{crewData.UnitGrade} / {level}], returning 0");
                    return 0;
                }
                return exp;
            }
        }

        public CommonStats BasicStat
        {
            get
            {
                CommonStats stat = new CommonStats();
                stat.SetMaxDamage(crewData.UnitbasicATK + crewData.LevelBracketATK * (level - 1));
                stat.SetMaxArmor(crewData.UnitbasicDEF + crewData.LevelBracketDEF * (level - 1));
                stat.SetMaxHealth(crewData.UnitbasicHP + crewData.LevelBracketHP * (level - 1));
                stat.SetMaxResilient(crewData.UnitbasicREC + crewData.LevelBracketREC * (level - 1));                
                stat.ResetAll();
                return stat;
            }
        }

        public CommonStats BasicStatNextLevel
        {
            get
            {
                CommonStats stat = new CommonStats();
                stat.SetMaxDamage(crewData.UnitbasicATK + crewData.LevelBracketATK * level);
                stat.SetMaxArmor(crewData.UnitbasicDEF + crewData.LevelBracketDEF * level);
                stat.SetMaxHealth(crewData.UnitbasicHP + crewData.LevelBracketHP * level);
                stat.SetMaxResilient(crewData.UnitbasicREC + crewData.LevelBracketREC * level);
                stat.ResetAll();
                return stat;
            }
        }        

        // Currently not Used
        private CommonStats HoldingEffectStat
        {
            get
            {
                CommonStats stat = new CommonStats();
                return stat;
            }
        }
        // ~Not Used

        public void ApplySavedCrewData(SavedCrew savedCrew)
        {
            level = savedCrew.level;
            isUnlocked = savedCrew.isUnlocked;
            count = savedCrew.count;            
        }

        public void RegisterLeveledUpEvent(UnityAction action)
        {
            leveledUpEvent.AddListener(action);
        }

        public void ClearLeveledUpEvent()
        {
            leveledUpEvent.RemoveAllListeners();
        }

        public bool AddAccumulatedExp(BigNum exp)
        {
            bool leveledUp = false;
            if (level >= maxLevel)
            {
                return leveledUp;
            }
            accumulatedExp += exp;
            while (accumulatedExp >= RequiredExp)
            {
                LevelUp();
                leveledUp = true;
                if (level == maxLevel)
                    break;
            }
            return leveledUp;
        }

        private void LevelUp()
        {
            accumulatedExp -= RequiredExp;
            level++;
            leveledUpEvent?.Invoke();
            if (level == maxLevel)
                accumulatedExp = 0;            
        }

        public TempCrewLevelExpData(int id)
        {
            var crewTableData = DataTableMgr.CrewTable.Get(id);
            crewData = null;
            
            if ( crewTableData != null )
            {
                crewData = crewTableData;
            }
            leveledUpEvent = new UnityEvent();
            accumulatedExp = 0;
            level = 1;
        }
    } // Scope by class TempCrewLevelExp

    public static class TempCrewLevelExpContainer
    {
        private static Dictionary<int, TempCrewLevelExpData> crewDataDict;

        static TempCrewLevelExpContainer()
        {
            crewDataDict = new Dictionary<int, TempCrewLevelExpData>();
            var crewTable = DataTableMgr.CrewTable;
            var crewDatas = crewTable.Values;
            foreach (var data in crewDatas)
            {
                if (!crewDataDict.ContainsKey(data.ID))
                {
                    TempCrewLevelExpData tempDictVal = new TempCrewLevelExpData(data.ID);
                    crewDataDict.Add(data.ID, tempDictVal);
                }
            }
        }

        public static bool TryGetTempCrewData(int id, out TempCrewLevelExpData crewData)
        {
            var tempCrewData = crewDataDict[id];
            if (tempCrewData != null)
            {
                crewData = tempCrewData;
                return true;
            }
            crewData = null;
            return false;
        }

        public static bool ApplyLoadedCrewData(SavedCrew savedCrew)
        {
            if(!crewDataDict.ContainsKey(savedCrew.crewData.ID))
            {
                Debug.LogError($"No temp crew data found with key [{savedCrew.crewData.ID}]");
                return false;
            }
            crewDataDict[savedCrew.crewData.ID].ApplySavedCrewData(savedCrew);
            return true;
        }
    }
} // namespace Root