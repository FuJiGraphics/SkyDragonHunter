using NPOI.OpenXml4Net.OPC;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using SkyDragonHunter.Test;
using SkyDragonHunter.UI;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedGrowth
    {
        public int id = -1;
        public int incLevel = 0;
        public GrowthStatType type;
        public BigNum stat;
    }

    public class SavedGrowthData
    {
        public Dictionary<int, SavedGrowth> savedGrowthMap;

        public void InitData()
        {
            savedGrowthMap = new();
        }

        public void UpdateSavedData()
        {
            UIGrowthPanel growthPanel = GameMgr.FindObject<UIGrowthPanel>("GrowthPanel");
            if (growthPanel != null)
            {
                var nodeArr = growthPanel.GrowthNodes;
                foreach (var node in nodeArr)
                {
                    if (!savedGrowthMap.ContainsKey(node.ID))
                    {
                        savedGrowthMap.Add(node.ID, new SavedGrowth());
                    }
                    savedGrowthMap[node.ID].id = node.ID;
                    savedGrowthMap[node.ID].incLevel = node.Level - 1;
                    savedGrowthMap[node.ID].type = node.StatType;
                    savedGrowthMap[node.ID].stat = node.CurrentStat;
                }
            }
        }

        public void ApplySavedData()
        {
            UIGrowthPanel growthPanel = GameMgr.FindObject<UIGrowthPanel>("GrowthPanel");
            if (growthPanel != null)
            {
                foreach (var saveData in savedGrowthMap)
                {
                    if (saveData.Value.id != -1 && saveData.Value.incLevel > 0)
                    {
                        var node = growthPanel.FindNode(saveData.Value.id);
                        node?.LevelUp(saveData.Value.incLevel);
                    }
                }
            }
            else // 던전 씬
            {
                foreach (var saveData in savedGrowthMap)
                {
                    if (saveData.Value.id != -1 && saveData.Value.incLevel > 0)
                    {
                        SetAirshipDefaultGrowthStats(saveData.Value.type, saveData.Value.stat);
                    }
                }
            }
        }

        // 던전 씬에서 스탯 적용 되도록 하기 위해서 사용함
        // (던전 씬 전용)
        private void SetAirshipDefaultGrowthStats(GrowthStatType type, BigNum stat)
        {
            switch (type)
            {
                case GrowthStatType.Attack:
                    AccountMgr.DefaultGrowthStats.SetMaxDamage(stat);
                    AccountMgr.DefaultGrowthStats.SetDamage(stat);
                    break;
                case GrowthStatType.Defense:
                    AccountMgr.DefaultGrowthStats.SetMaxArmor(stat);
                    AccountMgr.DefaultGrowthStats.SetArmor(stat);
                    break;
                case GrowthStatType.Health:
                    AccountMgr.DefaultGrowthStats.SetMaxHealth(stat);
                    AccountMgr.DefaultGrowthStats.SetHealth(stat);
                    break;
                case GrowthStatType.Resilient:
                    AccountMgr.DefaultGrowthStats.SetMaxResilient(stat);
                    AccountMgr.DefaultGrowthStats.SetResilient(stat);
                    break;
                case GrowthStatType.CriticalMultiplier:
                    AccountMgr.DefaultGrowthStats.SetCriticalMultiplier((float)stat);
                    break;
            }
            AccountMgr.DirtyAccountAndAirshipStat();
        }

    } // Scope by class SavedGrowthData

} // namespace Root