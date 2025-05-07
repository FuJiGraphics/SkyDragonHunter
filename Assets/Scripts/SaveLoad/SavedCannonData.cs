using SkyDragonHunter.Database;
using SkyDragonHunter.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedCannon
    {
        public bool isUnlocked;
        public int id;
        public int level;
        public CanonGrade cannonGrade;
        public CanonType cannonType;
        public bool isEquipped;
        public int count;
    }

    public class SavedCannonData
    {
        public List<SavedCannon> cannons;
        private Dictionary<CanonType, Dictionary<CanonGrade, SavedCannon>> m_CannonDict;

        public SavedCannonData()
        {
            InitData();
        }

        public void InitData()
        {
            var gradeCount = Enum.GetValues(typeof(CanonGrade)).Length;
            var typeCount = Enum.GetValues(typeof(CanonType)).Length;
            var totalLength = gradeCount * typeCount;
            cannons = new List<SavedCannon>(totalLength);
            m_CannonDict = new Dictionary<CanonType, Dictionary<CanonGrade, SavedCannon>>();

            for(int i = 0; i < gradeCount; ++i)
            {
                for(int j = 0; j < typeCount; ++j)
                {
                    int cannonId = 50000;
                    cannonId += (i + 1) * 1000;
                    cannonId += j + 1;

                    SavedCannon cannon = new SavedCannon();
                    cannon.isUnlocked = false;
                    cannon.id = cannonId;
                    cannon.level = 1;
                    cannon.cannonGrade = (CanonGrade)i;
                    cannon.cannonType = (CanonType)j;
                    cannon.isEquipped = false;
                    cannon.count = 0;
                    cannons.Add(cannon);
                    if(!m_CannonDict.ContainsKey(cannon.cannonType))
                    {
                        m_CannonDict.Add(cannon.cannonType, new Dictionary<CanonGrade, SavedCannon>());
                    }
                    if (!m_CannonDict[cannon.cannonType].ContainsKey(cannon.cannonGrade))
                    {
                        m_CannonDict[cannon.cannonType].Add(cannon.cannonGrade, cannon);
                    }
                }
            }

            m_CannonDict[CanonType.Normal][CanonGrade.Normal].count = 5;
            m_CannonDict[CanonType.Repeater][CanonGrade.Normal].count = 1;
        }

        public bool GetSavedCannon(CanonType type, CanonGrade grade, out SavedCannon cannon)
        {
            if(!m_CannonDict.ContainsKey(type))
            {
                cannon = null;
                return false;
            }
            else
            {
                if (m_CannonDict[type].ContainsKey(grade))
                {
                    cannon = m_CannonDict[type][grade];
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
            m_CannonDict = AccountMgr.GetHeldCannonsAsSavedCannons();
            cannons.Clear();

            foreach(var cannonDictByType in m_CannonDict)
            {
                foreach(var cannonKvp in cannonDictByType.Value)
                {
                    cannons.Add(cannonKvp.Value);
                }
            }
        }

        public void ApplySavedData()
        {
            AccountMgr.SetHeldCannons(m_CannonDict);
        }
        public void LateApplySavedData()
        {

        }
    } // Scope by class SavedCanonData

} // namespace Root