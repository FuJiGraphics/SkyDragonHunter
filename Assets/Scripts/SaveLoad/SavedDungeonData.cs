using SkyDragonHunter.Managers;
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

            for (int i = 0;  i < dungeons.Count; ++i)
            {
                var savedDungeon = new SavedDungeon();
                savedDungeon.dungeonType = (DungeonType)i;
                savedDungeon.clearedStage = 0;
                savedDungeon.clearedCount = 0;
            }

        }

        public void UpdateData()
        {

        }
        public void ApplySavedData()
        {

        }
    } // Scope by class SavedDungeonData

} // namespace Root