using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Test;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedCrew
    {
        public CrewTableData crewData;

        // 보유 정보
        public bool isUnlocked;
        public int level;
        public BigNum accumulatedExp;
        public int rank; // 별 개수
        public int count; // 승급 위한 보유 개수
        public bool isMounted;

        public int killCount;

        public BigNum CurrentDamage() => crewData.UnitbasicATK + crewData.LevelBracketATK * level;
        public BigNum CurrentHP() => crewData.UnitbasicHP + crewData.LevelBracketHP * level;
        public BigNum CurrentArmor() => crewData.UnitbasicDEF + crewData.LevelBracketDEF * level;
        public BigNum CurrentREG() => crewData.LevelBracketREC + crewData.LevelBracketREC * level;
    }

    public class SavedCrewData
    {
        public List<SavedCrew> crews;
        private Dictionary<int, SavedCrew> m_CrewDict;

        public SavedCrewData()
        {
            InitData();
        }

        public bool GetSavedCrew(int id, out SavedCrew crew)
        {
            if(m_CrewDict == null)
            {
                Debug.LogError($"CrewDict Null");
                crew = null;
                return false;
            }

            if (m_CrewDict[id] == null)
            {
                Debug.LogError($"Crew with id '{id}' is null");
                crew = null;
                return false;
            }

            crew = m_CrewDict[id];
            return true;
        }

        public bool GetCrewLevel(int id, out int level)
        {
            if (m_CrewDict == null || m_CrewDict.Count == 0)
            {
                Debug.LogWarning($"Saved Crews Null, returning 0");
                level = 0;
                return false;
            }

            if (m_CrewDict.ContainsKey(id))
            {
                level = m_CrewDict[id].level;
                return true;
            }
            else
            {
                Debug.LogWarning($"Cannot find crew with id '{id}', returning 0");
                level = 0;
                return false;
            }

            //foreach(var crew in crews)
            //{
            //    if(crew.crewData == null)
            //    {
            //        Debug.LogWarning($"Saved Crew ID [{id}] Null, returning 0");
            //        level = 0;
            //        return false;
            //    }
            //
            //    if(crew.crewData.ID == id)
            //    {
            //        level = crew.level;
            //        return true;
            //    }
            //}
            //Debug.LogWarning($"Cannot find Crew, returning 0");
            //level = 0;
            //return false;
        }

        public void InitData()
        {
            crews = new List<SavedCrew>();
            m_CrewDict = new Dictionary<int, SavedCrew>();

            var crewTable = DataTableMgr.CrewTable;
            foreach(var crewData in crewTable.Values)
            {
                var savedCrew = new SavedCrew();
                savedCrew.crewData = crewData;
                savedCrew.isUnlocked = false;
                savedCrew.level = 1;
                savedCrew.accumulatedExp = 0;
                savedCrew.rank = 0;
                savedCrew.count = 0;
                savedCrew.isMounted = false;

                savedCrew.killCount = 0;

                crews.Add(savedCrew);
                m_CrewDict.Add(crewData.ID, savedCrew);
            }

            //for (int i = 0; i < crewTable.Count; ++i)
            //{
            //    var savedCrew = new SavedCrew();
            //    savedCrew.isUnlocked = false;
            //    savedCrew.level = 1;
            //    savedCrew.accumulatedExp = 0;
            //    savedCrew.rank = 0;
            //    savedCrew.count = 0;
            //
            //    savedCrew.killCount = 0;
            //
            //    crews.Add(savedCrew);
            //}            

        }

        public void UpdateSavedData()
        {
            //AccountMgr.
            

        }

        public void ApplySavedData()
        {
            ApplyCrewDatasToUI();
        }

        private void ApplyCrewDatasToUI()
        {
            foreach ( var key in DataTableMgr.CrewTable.Keys)
            {
                //var crewData = DataTableMgr.CrewTable.Get(key);
                AccountMgr.RegisterCrew(key);
            }
        }
    } // Scope by class SavedCrewData

} // namespace Root