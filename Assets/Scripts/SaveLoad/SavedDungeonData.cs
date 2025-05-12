using SkyDragonHunter.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedDungeon
    {
        public DungeonType dungeonType;
        public int clearedStage;
        public int clearedCount;
    }

    public class SavedDungeonData
    {
        public List<SavedDungeon> dungeons;

        public void InitData()
        {
            dungeons = new List<SavedDungeon>();

            for (int i = 0;  i < (int)DungeonType.Count; ++i)
            {
                var savedDungeon = new SavedDungeon();
                savedDungeon.dungeonType = (DungeonType)i;
                savedDungeon.clearedStage = 0;
                savedDungeon.clearedCount = 0;
                dungeons.Add(savedDungeon);
            }
        }

        public void UpdateSavedData()
        {
            foreach(var dungeon in dungeons)
            {
                var clearedStage = DungeonMgr.GetClearedStage(dungeon.dungeonType);
                dungeon.clearedStage = clearedStage;
            }
        }

        public void ApplySavedData()
        {
            foreach (var dungeon in dungeons)
            {
                DungeonMgr.SetClearedStage(dungeon.dungeonType, dungeon.clearedStage);
            }
        }
    } // Scope by class SavedDungeonData

} // namespace Root