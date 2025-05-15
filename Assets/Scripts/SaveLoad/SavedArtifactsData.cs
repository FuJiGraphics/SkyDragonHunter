using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedArtifact
    {
        public string uuid;
        public int id;
        public ArtifactGrade artifactGrade;
        public BigNum constantStatValue;
        public ArtifactHoldStatType constantStatType;
        public int[] additionalStatIds;
        public bool isEquip;
        public int equipSlot;
    }

    public class SavedArtifactsData
    {
        public Dictionary<string, SavedArtifact> savedArtifactMap;

        public void InitData()
        {
            savedArtifactMap = new();
        }

        public void UpdateSavedData()
        {
            var artifactList = AccountMgr.Artifacts;
            foreach (var artifact in artifactList)
            {
                var newData = artifact.GenerateSaveData();
                if (savedArtifactMap.ContainsKey(newData.uuid))
                { 
                    savedArtifactMap.Remove(newData.uuid);
                }
                savedArtifactMap.Add(newData.uuid, newData);
            }
        }

        public void ApplySavedData()
        {
            UITreasureEquipmentSlotPanel.EquipList?.Clear();            
            foreach (var artifact in savedArtifactMap)
            {
                var equipPanel = GameMgr.FindObject<UITreasureEquipmentPanel>("TreasureEquipmentPanel");
                var infoPanel = GameMgr.FindObject<UIFortressEquipmentPanel>("UIFortressEquipmentPanel");
                var saveData = artifact.Value;
                if (AccountMgr.TryGetArtifact(saveData.uuid, out var target))
                {
                    equipPanel?.AddSlot(target);
                    AddSlot(target);
                }
                else
                {
                    var newArtifact = new ArtifactDummy(saveData);
                    AccountMgr.AddArtifact(newArtifact);
                    AddSlot(newArtifact);
                }
            }
        }

        private void AddSlot(ArtifactDummy target)
        {
            if (target.IsEquip && target.CurrentSlot != -1)
            {
                var slotPanel = GameMgr.FindObject<UITreasureEquipmentSlotPanel>("UITreasureEquipmentSlotPanel");
                if (slotPanel != null)
                {
                    slotPanel.Slots[target.CurrentSlot].SetSlot(target);
                    AccountMgr.SetArtifactSlot(target, target.CurrentSlot);
                    UITreasureEquipmentSlotPanel.EquipList.Add(target);
                }
            }
        }


    } // Scope by class SavedArtifactsData
} // namespace Root