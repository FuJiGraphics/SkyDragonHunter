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

        // ���� ����
        public bool isUnlocked;
        public int level;
        public BigNum accumulatedExp;
        public int rank; // �� ����
        public int count; // �±� ���� ���� ����

        public int killCount;

        public void InitData()
        {

        }
        public void UpdateData()
        {

        }
    } // Scope by class SavedCrewData

} // namespace Root