using SkyDragonHunter.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedCannon
    {
        public CanonGrade cannonGrade;
        public CanonType cannonType;
        public int count;
        public bool isUnlocked;
    }
    public class SavedCannonData
    {
        public List<SavedCannon> cannons;

        public void InitData()
        {
            var gradeCount = Enum.GetValues(typeof(CanonGrade)).Length;
            var typeCount = Enum.GetValues(typeof(CanonType)).Length;
            var totalLength = gradeCount * typeCount;
            cannons = new List<SavedCannon>(totalLength);

            for(int i = 0; i < gradeCount; ++i)
            {
                for(int j = 0; j < typeCount; ++j)
                {
                    SavedCannon cannon = new SavedCannon();
                    cannon.cannonGrade = (CanonGrade)i;
                    cannon.cannonType = (CanonType)j;
                    cannon.count = 0;
                    cannon.isUnlocked = false;
                    cannons.Add(cannon);
                }
            }
        }

        public void UpdateData()
        {

        }
        public void ApplySavedData()
        {

        }
    } // Scope by class SavedCanonData

} // namespace Root