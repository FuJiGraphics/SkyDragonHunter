using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System.Collections;
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

        public void InitData()
        {
            crews = new List<SavedCrew>();
            var crewTable = DataTableMgr.CrewTable;

            for (int i = 0; i < crewTable.Count; ++i)
            {
                var savedCrew = new SavedCrew();
                savedCrew.isUnlocked = false;
                savedCrew.level = 1;
                savedCrew.accumulatedExp = 0;
                savedCrew.rank = 0;
                savedCrew.count = 0;

                savedCrew.killCount = 0;

                crews.Add(savedCrew);
            }            

        }

        public void UpdateData()
        {

        }
        public void ApplySavedData()
        {

        }
    } // Scope by class SavedCrewData

} // namespace Root