using CsvHelper.TypeConversion;
using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.UI;
using System;
using System.Collections.Generic;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedMastery
    {
        public int id;
        public int level;
    }

    public class SavedMasteryData
    {
        public List<SavedMastery> savedMasterySocketList;

        public void InitData()
        {
            savedMasterySocketList = new List<SavedMastery>();
        }

        public void UpdateSavedData()
        {
            savedMasterySocketList.Clear();
            var masteryPanel = GameMgr.FindObject<UIMasteryPanel>("UIMasteryPanel");
            if (masteryPanel != null)
            {
                foreach (var nodeList in masteryPanel.NodeMap)
                {
                    foreach (var node in nodeList.Value)
                    {
                        SavedMastery newSaveData = new();
                        newSaveData.id = node.ID;
                        newSaveData.level = node.CurrentLevel;
                        savedMasterySocketList.Add(newSaveData);
                    }
                }
            }
        }

        public void ApplySavedData()
        {
            var masteryPanel = GameMgr.FindObject<UIMasteryPanel>("UIMasteryPanel");
            if (masteryPanel != null)
            {
                foreach (var saveData in savedMasterySocketList)
                {
                    masteryPanel.SetNodeLevel(saveData.id, saveData.level);
                }
            }
        }

    } // Scope by class SavedItemData

} // namespace Root