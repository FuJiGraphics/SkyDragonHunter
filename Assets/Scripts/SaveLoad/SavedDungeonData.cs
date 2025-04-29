using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedDungeon
    {
        public DungeonType DungeonType;
        public int clearedStage;
        public int clearedCount;
    }

    public class SavedDungeonData : MonoBehaviour
    {
        public List<SavedDungeon> dungeons;

        public void InitData()
        {
            dungeons = new List<SavedDungeon>();

            for (int i = 0;  i < dungeons.Count; ++i)
            {
                var savedDungeon = new SavedDungeon();
                savedDungeon.DungeonType = (DungeonType)i;
                savedDungeon.clearedStage = 0;
                savedDungeon.clearedCount = 0;
            }

        }

        public void UpdateData()
        {

        }
    } // Scope by class SavedDungeonData

} // namespace Root