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

        public int killCount;
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

                savedCrew.killCount = 0;

                crews.Add(savedCrew);
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

        public void UpdateData()
        {
            var tempUserDataGO = GameMgr.FindObject("TempUserData");
            var tempUserData = tempUserDataGO.GetComponent<TempUserData>();
            if (tempUserData == null)
            {
                Debug.LogError($"[SavedCrewData] Update Failed, Temp User Data Null");
                return;
            }

            var crewGOs = tempUserData.crewDataPrefabs;
            





        }
        public void ApplySavedData()
        {

        }
    } // Scope by class SavedCrewData

} // namespace Root