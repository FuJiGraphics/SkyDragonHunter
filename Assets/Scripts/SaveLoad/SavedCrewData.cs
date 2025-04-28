using Newtonsoft.Json;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedCrewData
    {
        public CrewTableData crewData;

        // 보유 정보
        public bool isUnlocked;
        public int level;
        public BigNum accumulatedExp;
        public int rank; // 별 개수
        public int count; // 승급 위한 보유 개수

        public int killCount;

        public void InitData()
        {

        }
        public void UpdateData()
        {

        }
    } // Scope by class SavedCrewData

} // namespace Root