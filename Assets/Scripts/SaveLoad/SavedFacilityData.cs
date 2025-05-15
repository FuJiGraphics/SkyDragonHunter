using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
            var facilityMgrGO = GameMgr.FindObject($"FacilitySystemMgr");
            FacilitySystemMgr facilityMgr;
            if( facilityMgrGO != null )
            {
                facilityMgr = facilityMgrGO.GetComponent<FacilitySystemMgr>();
            }
            else
            {                
                facilityMgr = GameObject.FindObjectOfType<FacilitySystemMgr>();
            }
            if(facilityMgr == null)
            {
                Debug.LogWarning($"Update Facility data failed, FacilityMgr null");
                return;
            }

            foreach ( var facility in facilityMgr.facilityList)
            {
                var savedFacility = GetSavedFacility(facility.type);
                savedFacility.level = facility.level;
                savedFacility.isUpgrading = facility.isUpgrading;
                if(savedFacility.isUpgrading)
                {
                    savedFacility.upgradeStartTime = facility.upgradeStartedTime;
                }
                else
                {
                    savedFacility.upgradeStartTime = DateTime.UtcNow;
                }
                savedFacility.lastAcquiredTime = facility.lastAccquiredTime;
                Debug.LogWarning($"facility {facility.type} saved!");
            }
        }

        public void ApplySavedData()
        {
            var facilityMgrGO = GameMgr.FindObject($"FacilitySystemMgr");
            FacilitySystemMgr facilityMgr;
            if (facilityMgrGO != null)
            {
                facilityMgr = facilityMgrGO.GetComponent<FacilitySystemMgr>();
            }
            else
            {
                facilityMgr = GameObject.FindObjectOfType<FacilitySystemMgr>();
            }
            if (facilityMgr == null)
            {
                Debug.LogWarning($"Apply Facility data failed, FacilityMgr null");
                return;
            }

            foreach (var facility in facilityMgr.facilityList)
            {
                var savedFacility = GetSavedFacility(facility.type);
                facility.level = savedFacility.level;
                facility.isUpgrading = savedFacility.isUpgrading;
                if (facility.isUpgrading)
                {
                    facility.upgradeStartedTime = savedFacility.upgradeStartTime;
                }
                else
                {
                    facility.upgradeStartedTime = DateTime.UtcNow;
                }
                facility.lastAccquiredTime = savedFacility.lastAcquiredTime;
                Debug.LogWarning($"facility {facility.type} applied!");
            }
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