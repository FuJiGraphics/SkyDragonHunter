using SkyDragonHunter.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedRepairer
    {
        public CanonGrade cannonGrade;
        public CanonType cannonType;
        public int count;
        public bool isUnlocked;
    }
    public class SavedRepairerData
    {
        public List<SavedRepairer> repairers;

        public void InitData()
        {
            var gradeCount = Enum.GetValues(typeof(CanonGrade)).Length;
            var typeCount = Enum.GetValues(typeof(CanonType)).Length;
            var totalLength = gradeCount * typeCount;
            repairers = new List<SavedRepairer>(totalLength);

            for (int i = 0; i < gradeCount; ++i)
            {
                for (int j = 0; j < typeCount; ++j)
                {
                    SavedRepairer repairer = new SavedRepairer();
                    repairer.cannonGrade = (CanonGrade)i;
                    repairer.cannonType = (CanonType)j;
                    repairer.count = 0;
                    repairer.isUnlocked = false;
                    repairers.Add(repairer);
                }
            }

        }
        public void UpdateData()
        {

        }
    } // Scope by class SavedRepairerData

} // namespace Root