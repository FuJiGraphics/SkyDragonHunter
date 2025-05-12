using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Temp;
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
    }

    public class SavedCrewData
    {
        public List<SavedCrew> crews;
        
        public bool GetCrewLevel(int id, out int level)
        {
            if (crews == null || crews.Count == 0)
            {
                Debug.LogWarning($"Saved Crews Null, returning 0");
                level = 0;
                return false;
            }

            foreach(var crew in crews)
            {
                if(crew.crewData == null)
                {
                    Debug.LogWarning($"Saved Crew ID [{id}] Null, returning 0");
                    level = 0;
                    return false;
                }

                if(crew.crewData.ID == id)
                {
                    level = crew.level;
                    return true;
                }
            }
            Debug.LogWarning($"Cannot find Crew, returning 0");
            level = 0;
            return false;
        }

        public void InitData()
        {
            crews = new List<SavedCrew>();
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

                bool contains = false;
                foreach(var crew in crews)
                {
                    if (crew.crewData.ID == crewData.ID)
                        contains = true;
                }

                if (!contains)
                    crews.Add(savedCrew);
                else
                    Debug.LogError($"Trying to add duplicated crew ID [{crewData.ID}]");
            }
        }

        public void UpdateSavedData()
        {
            foreach(var crew in crews)
            {
                if(!TempCrewLevelExpContainer.TryGetTempCrewData(crew.crewData.ID, out var tempCrewData))
                {
                    Debug.LogError($"No Temp Crew Data Found with key [{crew.crewData.ID}]");
                    continue;
                }
                crew.level = tempCrewData.Level;
                crew.isUnlocked = tempCrewData.IsUnlocked;
                crew.rank = tempCrewData.Rank;
                crew.count = tempCrewData.Count;
            }
        }

        public void ApplySavedData()
        {
            foreach(var crew in crews)
            {
                TempCrewLevelExpContainer.ApplyLoadedCrewData(crew);
            }
        }
    } // Scope by class SavedCrewData

} // namespace Root