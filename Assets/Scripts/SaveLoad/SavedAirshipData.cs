using SkyDragonHunter.Entities;
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

        public void UpdateData()
        {
            UpdateCrewMountData();
            
        }

        public void ApplySavedData()
        {
            ApplyCrewMountData();


        }

        // private methods

        private void UpdateCrewMountData()
        {
            var airshipGo = GameMgr.FindObject("Airship");
            var crewEquipController = airshipGo.GetComponent<CrewEquipmentController>();
            if (crewEquipController == null)
            {
                Debug.LogError($"[SavedAirshipData] Update Failed, crewEquipController Null");
                return;
            }

            var equipSlots = crewEquipController.EquipSlots;
            for (int i = 0; i < equipSlots.Length; ++i)
            {
                if (equipSlots[i] == null)
                {
                    mountedCrewIDs[i] = 0;
                    continue;
                }

                var crewBT = equipSlots[i].GetComponent<NewCrewControllerBT>();
                if (crewBT == null)
                {
                    Debug.LogError($"[SavedAirSHipData] Error occured when updating crew slot index[{i}]");
                    continue;
                }

                mountedCrewIDs[i] = crewBT.ID;
                Debug.Log($"[SavedAirshipData] Crew Mount Data Updated, [Slot: {i} / ID: {crewBT.ID}]");
            }
        }

        private void ApplyCrewMountData()
        {
            



        }
    } // Scope by class SavedAirshipData

} // namespace Root