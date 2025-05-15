using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class SavedAirshipData
    {
        // equipped info
        public int[] mountedCrewIDs;
        public int equippedCannonID;
        public int equippedRepairerID;
        public int equippedArtifactInstanceID; // Hash ID

        public void InitData()
        {
            mountedCrewIDs = new int[4];
            equippedCannonID = 0;
            equippedRepairerID = 0;
            equippedArtifactInstanceID = 0;
        }
        public void UpdateSavedData()
        {            
            var airshipGo = GameMgr.FindObject($"Airship");            
            if (airshipGo == null)
            {
                // Debug.LogError($"Apply AirshipData Failed, cannot find airship GameObject");
                return;
            }
            if (!airshipGo.TryGetComponent<CrewEquipmentController>(out var equipController))
            {
                // Debug.LogError($"Apply AirshipData Failed, cannot find crewEquipController");
                return;
            }
            var equipSlots = equipController.EquipSlots;

            for(int i = 0; i < equipSlots.Length; ++i)
            {
                if (equipSlots[i] == null)
                {
                    // Debug.LogWarning($"[SavedAirshipData] crew null in mounted slot index[{i}]");
                    mountedCrewIDs[i] = 0;
                    continue;
                }

                var equipCrewBT = equipSlots[i].GetComponent<NewCrewControllerBT>();
                if(equipCrewBT == null)
                {
                    // Debug.LogError($"[SavedAirshipData] Cannot find CrewControllerBT in mounted slot index[{i}]");
                    continue;
                }
                mountedCrewIDs[i] = equipCrewBT.ID;
            }
        }

        public void ApplySavedData()
        {
            // Applied directly according to the AccountMgr.
        }
    } // Scope by class SavedAirshipData

} // namespace Root