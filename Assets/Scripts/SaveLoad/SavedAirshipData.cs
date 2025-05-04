using SkyDragonHunter.Entities;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Test;
using SkyDragonHunter.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SkyDragonHunter {

    public class SavedAirshipData
    {
        // equipped info
        public int[] mountedCrewIDs;
        public int equippedCannonID;
        public int equippedRepairerID;
        public int equippedArtifactInstanceID; // Hash ID

        private const string c_CrewPrefabAddressableDirectory = "Prefabs/Crews/";

        public SavedAirshipData()
        {
            InitData();
        }

        public void InitData()
        {
            mountedCrewIDs = new int[4];
            equippedCannonID = 0;
            equippedRepairerID = 0;
            equippedArtifactInstanceID = 0;
        }

        public void UpdateSavedData()
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

            var equipIdSlots = crewEquipController.EquipIdSlots;
            for (int i = 0; i < equipIdSlots.Length; ++i)
            {
                mountedCrewIDs[i] = equipIdSlots[i];
                continue;
            }

            // Above Logics should do
            
            //var equipSlots = crewEquipController.EquipSlots;            
            //for (int i = 0; i < equipSlots.Length; ++i)
            //{
            //    if (equipSlots[i] == null)
            //    {
            //        mountedCrewIDs[i] = 0;
            //        continue;
            //    }
            //
            //    var crewBT = equipSlots[i].GetComponent<NewCrewControllerBT>();
            //    if (crewBT == null)
            //    {
            //        Debug.LogError($"[SavedAirSHipData] Error occured when updating crew slot index[{i}]");
            //        continue;
            //    }
            //
            //    mountedCrewIDs[i] = crewBT.ID;
            //    Debug.Log($"[SavedAirshipData] Crew Mount Data Updated, [Slot: {i} / ID: {crewBT.ID}]");
            //}
        }

        private void ApplyCrewMountData()
        {
            Debug.Log($"[SavedAirshipData] Start applying Crew Mount Data");
            var equipController = GameMgr.FindObject<CrewEquipmentController>("Airship");
            var panelGo = GameMgr.FindObject("AssignUnitTofortressPanel");
            UIAssignUnitTofortressPanel assignUnitTofortressPanel = panelGo?.GetComponent<UIAssignUnitTofortressPanel>();

            for(int i = 0; i < mountedCrewIDs.Length; ++i)
            {
                if (mountedCrewIDs[i] == 0)
                {
                    Debug.Log($"Mount Slot [{i}] empty");
                    continue;
                }
                equipController.EquipSlot(i, mountedCrewIDs[i]);
                assignUnitTofortressPanel?.EquipCrew(i, mountedCrewIDs[i]);


                //var path = Path.Combine(c_CrewPrefabAddressableDirectory, mountedCrewIDs[i].ToString());
                //var crewGo = Addressables.InstantiateAsync(path).WaitForCompletion();
                //if (crewGo == null)
                //{
                //    Debug.LogError($"Mount Slot [{i}] ID [{mountedCrewIDs[i]}] CrewGo Null");
                //    continue;
                //}
                //
                //var crewBT = crewGo.GetComponent<NewCrewControllerBT>();
                //if(crewBT == null)
                //{
                //    Debug.LogError($"Mount Slot [{i}] ID [{mountedCrewIDs[i]}] CrewBT Null");
                //    continue;
                //}
                //
                //if (!SaveLoadMgr.GameData.savedCrewData.GetCrewLevel(mountedCrewIDs[i], out var level))
                //{
                //    Debug.LogError($"Error Occured finding crewLevel from MountSlot [{i}]");
                //}
                //crewBT.SetDataFromTableWithExistingIDTemp(level);
            }
        }
    } // Scope by class SavedAirshipData

} // namespace Root