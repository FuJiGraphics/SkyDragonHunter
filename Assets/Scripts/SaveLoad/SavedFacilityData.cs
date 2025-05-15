using SkyDragonHunter.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedFacility
    {
        public FacilityType type;
        public int level;
        public bool isUpgrading;
        public DateTime upgradeStartTime;
        public DateTime lastAcquiredTime;
    }

    public class SavedFacilityData
    {
        public List<SavedFacility> facilityList;

        public void InitData()
        {
            facilityList = new List<SavedFacility>();
            for(int i = 0; i < Enum.GetValues(typeof(FacilityType)).Length; ++i)
            {
                SavedFacility facility = new SavedFacility();
                facility.type = (FacilityType)i;
                facility.level = 1;
                facility.isUpgrading = false;
                facility.upgradeStartTime = DateTime.MinValue;
                facility.lastAcquiredTime = DateTime.MinValue;
                facilityList.Add(facility);
            }
        }
        public void UpdateSavedData()
        {
            
        }

        public void ApplySavedData()
        {
           
        }       

        public SavedFacility GetSavedFacility(FacilityType type)
        {
            if (facilityList[(int)type].type == type)
                return facilityList[(int)type];
            else
            {
                foreach(SavedFacility facility in facilityList)
                {
                    if (facility.type == type)
                        return facility;
                }
            }

            Debug.LogError($"SavedFacility not found");
            return null;
        }
    } // Scope by class SavedStageData

} // namespace Root