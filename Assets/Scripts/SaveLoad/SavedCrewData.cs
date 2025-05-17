using Newtonsoft.Json;
using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Temp;
using SkyDragonHunter.Test;
using SkyDragonHunter.UI;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedCrew
    {
        public CrewTableData crewData;

        // 보유 정보
        public bool isUnlocked;
        public int level;
        public BigNum accumulatedExp;
        public int count; // 승급 위한 보유 개수
        public int slotIndex;
        public bool isEquip = false;
    }

    public class SavedCrewData
    {
        public List<SavedCrew> crews;
        private Dictionary<int, SavedCrew> crewDict;

        public SavedCrew GetCrewData(int crewId)
        {
            if(crewDict == null)
            {
                //Debug.LogError($"crewDict Null");
                
                foreach (var crew in crews)
                {
                    if(crew.crewData.ID == crewId)
                    {
                        return crew;
                    }
                }
            }

            if (!crewDict.ContainsKey(crewId))
            {
                Debug.LogError($"no crew found with key {crewId}");
                return null;
            }

            return crewDict[crewId];
        }

        public bool GetCrewLevel(int id, out int level)
        {
            if (crews == null || crews.Count == 0)
            {
                level = 0;
                return false;
            }

            foreach(var crew in crews)
            {
                if(crew.crewData == null)
                {
                    level = 0;
                    return false;
                }

                if(crew.crewData.ID == id)
                {
                    level = crew.level;
                    return true;
                }
            }
            level = 0;
            return false;
        }        

        public void InitData()
        {
            crews = new List<SavedCrew>();
            crewDict = new Dictionary<int, SavedCrew>();
            var crewTable = DataTableMgr.CrewTable;

            foreach(var crewData in crewTable.Values)
            {
                var savedCrew = new SavedCrew();
                savedCrew.crewData = crewData;
                savedCrew.isUnlocked = false;
                savedCrew.level = 1;
                savedCrew.accumulatedExp = 0;
                savedCrew.count = 0;
                savedCrew.isEquip = false;

                bool contains = false;
                foreach(var crew in crews)
                {
                    if (crew.crewData.ID == crewData.ID)
                        contains = true;
                }

                if (!contains)
                {
                    crews.Add(savedCrew);
                    crewDict.Add(savedCrew.crewData.ID, savedCrew);
                }
                else
                    Debug.LogError($"Trying to add duplicated crew ID [{crewData.ID}]");
            }
        }

        public void UpdateSavedData()
        {
            foreach(var crew in crews)
            {
                if (crew == null)
                    DataTableMgr.CrewTable.Get(crew.crewData.ID);
                if (!TempCrewLevelExpContainer.TryGetTempCrewData(crew.crewData.ID, out var tempCrewData))
                {
                    Debug.LogError($"No Temp Crew Data Found with key [{crew.crewData.ID}]");
                    continue;
                }
                crew.level = tempCrewData.Level;
                crew.isUnlocked = tempCrewData.IsUnlocked;
                crew.count = tempCrewData.Count;
                SaveEquipInfo(crew);
            }
        }

        public void ApplySavedData()
        {
            foreach (var crew in crews)
            {
                if (crew == null)
                {
                    Debug.LogWarning($"[ApplySavedData]: SavedCrew is null");
                    continue;
                }

                TempCrewLevelExpContainer.ApplyLoadedCrewData(crew);
                if (crew.isUnlocked)
                {
                    var instance = crew.crewData.GetInstance();
                    if (instance != null)
                    {
                        if (instance.TryGetComponent<NewCrewControllerBT>(out var btComp))
                        {
                            btComp.SetDataFromTableWithExistingIDTemp(crew.level);
                        }
                        AccountMgr.RegisterCrew(instance);
                        instance.GetComponent<CrewAccountStatProvider>().ApplyNewStatus();
                        instance.SetActive(false);
                    }

                    if (crew.isEquip)
                    {
                        EquipCrew(crew.crewData.ID, crew.slotIndex, instance);
                    }
                }
            }
        }

        public void SaveEquipInfo(SavedCrew saveData)
        {
            GameObject airshipInstance = GameMgr.FindObject("Airship");
            if (airshipInstance.TryGetComponent<CrewEquipmentController>(out var equipController))
            {
                var equipSlots = equipController.EquipSlots;
                for (int i = 0; i < equipSlots.Length; ++i)
                {
                    saveData.isEquip = false;
                    saveData.slotIndex = 0;

                    if (equipSlots[i] == null)
                        continue;

                    var equipCrewInfo = equipSlots[i].GetComponent<CrewInfoProvider>();
                    if (equipCrewInfo.ID == saveData.crewData.ID)
                    {
                        saveData.slotIndex = i;
                        saveData.isEquip = true;
                        break;
                    }
                }

            }
        }

        private void EquipCrew(int id, int slot, GameObject crewInstance)
        {
            var equipPanel = GameMgr.FindObject<UIAssignUnitTofortressPanel>("AssignUnitTofortressPanel");
            if (equipPanel != null)
            {
                if (!IsEquipCrew(id))
                {
                    equipPanel.EquipCrew(slot, crewInstance);
                }
            }
            else  // 던전 씬
            {
                GameObject airshipInstance = GameMgr.FindObject("Airship");
                if (airshipInstance.TryGetComponent<CrewEquipmentController>(out var equipController))
                {
                    if (!equipController.HasEquipCrew(id))
                    {
                        equipController.EquipSlot(slot, crewInstance);
                    }
                }
            }
        }

        private bool IsEquipCrew(int id)
        {
            bool result = false;
            GameObject airshipInstance = GameMgr.FindObject("Airship");
            if (airshipInstance.TryGetComponent<CrewEquipmentController>(out var equipController))
            {
                if (equipController.HasEquipCrew(id))
                {
                    result = true;
                }
            }
            return result;
        }
                
    } // Scope by class SavedCrewData

} // namespace Root