using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedCannon
    {
        public CanonGrade cannonGrade;
        public CanonType cannonType;
        public int id;
        public int count;
        public int level;
        public bool isUnlocked;
        public bool isEquipped;
    }

    public class SavedCannonData
    {
        public List<SavedCannon> cannons;
        private Dictionary<CanonGrade, Dictionary<CanonType, SavedCannon>> cannonDict;

        public void InitData()
        {
            var gradeCount = Enum.GetValues(typeof(CanonGrade)).Length;
            var typeCount = Enum.GetValues(typeof(CanonType)).Length;
            var totalLength = gradeCount * typeCount;
            cannons = new List<SavedCannon>(totalLength);
            cannonDict = new Dictionary<CanonGrade, Dictionary<CanonType, SavedCannon>>();
            for(int i = 0; i < gradeCount; ++i)
            {
                if(!cannonDict.ContainsKey((CanonGrade)i))
                    cannonDict.Add((CanonGrade)i, new());
                for(int j = 0; j < typeCount; ++j)
                {
                    SavedCannon cannon = new SavedCannon();
                    cannon.cannonGrade = (CanonGrade)i;
                    cannon.cannonType = (CanonType)j;
                    cannon.count = 0;
                    cannon.level = 1;
                    cannon.isUnlocked = false;
                    cannon.isEquipped = false;
                    cannons.Add(cannon);
                    if (!cannonDict[(CanonGrade)i].ContainsKey((CanonType)j))
                        cannonDict[(CanonGrade)i].Add((CanonType)j, cannon);
                }
            }
            // cannonDict[CanonGrade.Normal][CanonType.Normal].count = 5;
            // cannonDict[CanonGrade.Normal][CanonType.Repeater].count = 1;
            cannonDict[CanonGrade.Normal][CanonType.Normal].count = 1;
        }

        public void UpdateSavedData()
        {
            var cannonDummies = AccountMgr.HeldCanons;
            foreach(var cannonDummy in cannonDummies)
            {
                var savedCannon = cannonDict[cannonDummy.Grade][cannonDummy.Type];
                if(savedCannon == null)
                {
                    Debug.LogError($"Cannot find savedCannon with the keys [{cannonDummy.Grade}/{cannonDummy.Type}]");
                    continue;
                }
                savedCannon.count = cannonDummy.Count;
                savedCannon.level = cannonDummy.Level;
                savedCannon.id = cannonDummy.ID;
                savedCannon.isUnlocked = cannonDummy.IsUnlock;
                savedCannon.isEquipped = cannonDummy.IsEquip;
            }
        }

        public void ApplySavedData()
        {
            cannonDict = new Dictionary<CanonGrade, Dictionary<CanonType, SavedCannon>>();
            foreach (var cannon in cannons)
            {
                if (!cannonDict.ContainsKey(cannon.cannonGrade))
                    cannonDict.Add(cannon.cannonGrade, new());
                if (!cannonDict[cannon.cannonGrade].ContainsKey(cannon.cannonType))
                    cannonDict[cannon.cannonGrade].Add(cannon.cannonType, cannon);
            }

            AccountMgr.ClearResgisterCanons();
            CanonDummy[] canonDummys = CanonTable.GetAllCanonDummyTypes();
            foreach (var canonDummy in canonDummys)
            {
                var savedCannon = SaveLoadMgr.GameData.savedCannonData.GetSavedCannon(canonDummy.Grade, canonDummy.Type);
                if (savedCannon != null)
                {
                    canonDummy.Count = savedCannon.count;
                    canonDummy.Level = savedCannon.level;
                    canonDummy.IsUnlock = savedCannon.isUnlocked;
                    canonDummy.IsEquip = savedCannon.isEquipped;
                    if (canonDummy.IsEquip)
                    {
                        var infoUiPanel = GameMgr.FindObject<UIFortressEquipmentPanel>("UIFortressEquipmentPanel");
                        var canonInstance = canonDummy.GetCanonInstance();
                        if (infoUiPanel != null &&
                            canonInstance.TryGetComponent<ICanonInfoProvider>(out var provider))
                        {
                            infoUiPanel.SetCanonIcon(0, provider.Icon);
                            infoUiPanel.SetCanonIconColor(0, provider.Color);
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Cannot find saved cannon with keys [{canonDummy.Grade}/{canonDummy.Type}]");
                }
                AccountMgr.RegisterCanon(canonDummy);
            }
        }

        public SavedCannon GetSavedCannon(CanonGrade grade, CanonType type)
        {
            if(cannonDict == null)
            {
                Debug.LogError($"CannonDcit null");
                return null;
            }

            if (!cannonDict.ContainsKey(grade))
                return null;
            if (!cannonDict[grade].ContainsKey(type))
                return null;

            return cannonDict[grade][type];
        }        
    } // Scope by class SavedCanonData

} // namespace Root