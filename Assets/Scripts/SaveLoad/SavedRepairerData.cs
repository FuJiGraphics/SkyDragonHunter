using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedRepairer
    {
        public bool isUnlocked;
        public int id;
        public int level;
        public RepairGrade repairerGrade;
        public RepairType repairerType;
        public bool isEquipped;
        public int count;
    }

    public class SavedRepairerData
    {
        public List<SavedRepairer> repairers;
        private Dictionary<RepairType, Dictionary<RepairGrade, SavedRepairer>> m_RepairerDict;

        public SavedRepairerData()
        {
            InitData();
        }

        public void InitData()
        {
            var gradeCount = Enum.GetValues(typeof(RepairGrade)).Length;
            var typeCount = Enum.GetValues(typeof(RepairType)).Length;
            var totalLength = gradeCount * typeCount;
            repairers = new List<SavedRepairer>(totalLength);
            m_RepairerDict = new Dictionary<RepairType, Dictionary<RepairGrade, SavedRepairer>>();

            for (int i = 0; i < gradeCount; ++i)
            {
                for (int j = 0; j < typeCount; ++j)
                {
                    int repairerId = 60000;
                    repairerId += (i + 1) * 1000;
                    repairerId += j + 1;

                    SavedRepairer repairer = new SavedRepairer();
                    repairer.isUnlocked = false;
                    repairer.id = repairerId;
                    repairer.level = 1;
                    repairer.repairerGrade = (RepairGrade)i;
                    repairer.repairerType = (RepairType)j;
                    repairer.count = 0;
                    repairers.Add(repairer);
                    if (!m_RepairerDict.ContainsKey(repairer.repairerType))
                    {
                        m_RepairerDict.Add(repairer.repairerType, new Dictionary<RepairGrade, SavedRepairer>());
                    }
                    if (!m_RepairerDict[repairer.repairerType].ContainsKey(repairer.repairerGrade))
                    {
                        m_RepairerDict[repairer.repairerType].Add(repairer.repairerGrade, repairer);
                    }
                }
            }

            m_RepairerDict[RepairType.Normal][RepairGrade.Normal].count = 5;
            m_RepairerDict[RepairType.Elite][RepairGrade.Normal].count = 1;
        }

        public bool GetSavedRepairer(RepairType type, RepairGrade grade, out SavedRepairer cannon)
        {
            if (!m_RepairerDict.ContainsKey(type))
            {
                cannon = null;
                return false;
            }
            else
            {
                if (m_RepairerDict[type].ContainsKey(grade))
                {
                    cannon = m_RepairerDict[type][grade];
                    return true;
                }
                else
                {
                    cannon = null;
                    return false;
                }
            }
        }

        public void UpdateSavedData()
        {
            m_RepairerDict = AccountMgr.GetHeldRepairersAsSavedRepairers();
            repairers.Clear();

            foreach (var cannonDictByType in m_RepairerDict)
            {
                foreach (var cannonKvp in cannonDictByType.Value)
                {
                    repairers.Add(cannonKvp.Value);
                }
            }
        }

        public void ApplySavedData()
        {
            AccountMgr.SetHeldRepairers(m_RepairerDict);
        }

        public void LateApplySavedData()
        {

        }
    } // Scope by class SavedRepairerData

} // namespace Root