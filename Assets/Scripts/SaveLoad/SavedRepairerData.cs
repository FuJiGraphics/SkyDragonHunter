using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using SkyDragonHunter.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedRepairer
    {
        public RepairGrade RepairerGrade;
        public RepairType RepairerType;
        public int id;
        public int count;
        public int level;
        public bool isUnlocked;
        public bool isEquipped;
    }

    public class SavedRepairerData
    {
        public List<SavedRepairer> repairers;
        private Dictionary<RepairGrade, Dictionary<RepairType, SavedRepairer>> repairerDict;

        public void InitData()
        {
            var gradeCount = Enum.GetValues(typeof(RepairGrade)).Length;
            var typeCount = Enum.GetValues(typeof(RepairType)).Length;
            var totalLength = gradeCount * typeCount;
            repairers = new List<SavedRepairer>(totalLength);
            repairerDict = new Dictionary<RepairGrade, Dictionary<RepairType, SavedRepairer>>();
            for (int i = 0; i < gradeCount; ++i)
            {
                if (!repairerDict.ContainsKey((RepairGrade)i))
                    repairerDict.Add((RepairGrade)i, new());
                for (int j = 0; j < typeCount; ++j)
                {
                    SavedRepairer Repairer = new SavedRepairer();
                    Repairer.RepairerGrade = (RepairGrade)i;
                    Repairer.RepairerType = (RepairType)j;
                    Repairer.count = 0;
                    Repairer.level = 1;
                    Repairer.isUnlocked = false;
                    Repairer.isEquipped = false;
                    repairers.Add(Repairer);
                    if (!repairerDict[(RepairGrade)i].ContainsKey((RepairType)j))
                        repairerDict[(RepairGrade)i].Add((RepairType)j, Repairer);
                }
            }
            // repairerDict[RepairGrade.Normal][RepairType.Normal].count = 5;
            // repairerDict[RepairGrade.Normal][RepairType.Elite].count = 1;

            repairerDict[RepairGrade.Normal][RepairType.Normal].count = 1;
        }

        public void UpdateSavedData()
        {
            var RepairerDummies = AccountMgr.HeldRepairs;
            foreach (var RepairerDummy in RepairerDummies)
            {
                var savedRepairer = repairerDict[RepairerDummy.Grade][RepairerDummy.Type];
                if (savedRepairer == null)
                {
                    Debug.LogError($"Cannot find savedRepairer with the keys [{RepairerDummy.Grade}/{RepairerDummy.Type}]");
                    continue;
                }
                savedRepairer.count = RepairerDummy.Count;
                savedRepairer.level = RepairerDummy.Level;
                savedRepairer.id = RepairerDummy.ID;
                savedRepairer.isUnlocked = RepairerDummy.IsUnlock;
                savedRepairer.isEquipped = RepairerDummy.IsEquip;
            }
        }

        public void ApplySavedData()
        {
            repairerDict = new Dictionary<RepairGrade, Dictionary<RepairType, SavedRepairer>>();
            foreach (var repairer in repairers)
            {
                if (!repairerDict.ContainsKey(repairer.RepairerGrade))
                    repairerDict.Add(repairer.RepairerGrade, new());
                if (!repairerDict[repairer.RepairerGrade].ContainsKey(repairer.RepairerType))
                    repairerDict[repairer.RepairerGrade].Add(repairer.RepairerType, repairer);
            }

            AccountMgr.ClearRegisterRepairs();
            RepairDummy[] repairDummys = RepairTableTemplate.GetAllRepairDummyTypes();
            foreach (var repair in repairDummys)
            {
                // TODO: LJH
                var savedRepairer = SaveLoadMgr.GameData.savedRepairerData.GetSavedRepairer(repair.Grade, repair.Type);
                if (savedRepairer != null)
                {
                    //repair.ID = savedCannon.id;
                    repair.Count = savedRepairer.count;
                    repair.Level = savedRepairer.level;
                    repair.IsUnlock = savedRepairer.isUnlocked;
                    repair.IsEquip = savedRepairer.isEquipped;
                    if (savedRepairer.isEquipped)
                    {
                        var infoUiPanel = GameMgr.FindObject<UIFortressEquipmentPanel>("UIFortressEquipmentPanel");
                        if (infoUiPanel != null)
                        {
                            infoUiPanel.SetRepairIcon(0, repair.Icon);
                            infoUiPanel.SetRepairIconColor(0, repair.Color);
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Cannot find saved cannon with keys [{repair.Grade}/{repair.Type}]");
                }
                // ~TODO
                AccountMgr.RegisterRepair(repair);
            }
        }

        public SavedRepairer GetSavedRepairer(RepairGrade grade, RepairType type)
        {
            if (repairerDict == null)
            {
                Debug.LogError($"RepairerDcit null");
                return null;
            }

            if (!repairerDict.ContainsKey(grade))
                return null;
            if (!repairerDict[grade].ContainsKey(type))
                return null;

            return repairerDict[grade][type];
        }
    } // Scope by class SavedRepairData

} // namespace Root