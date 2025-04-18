using SkyDragonHunter.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkyDragonHunter.Managers 
{    
    public enum DungeonType
    {
        Type1,
        Type2,
        Type3,
        Count,
    }

    public static class DungeonMgr
    {
        // Private Fields
        private static DungeonType s_DungeonType = DungeonType.Type1;
        private static int s_StageIndex;
        private static List<int> s_CrewSlots;

        // Temp
        private static int s_SlotCount = 4;

        // Properties
        public static List<int> CrewSlots => s_CrewSlots;
        
        // Public Methods
        static DungeonMgr()
        {
            Init();
        }

        public static void Init()
        {
            s_CrewSlots = new List<int>(s_SlotCount);
        }

        public static void EnterDungeon(DungeonType dungeonType, int stageIndex)
        {
            s_DungeonType = dungeonType;
            s_StageIndex = stageIndex;
            SceneManager.LoadScene((int)SceneIds.DungeonScene);
        }

        public static bool TryGetStageData(out DungeonType dungeonType, out int stageIndex)
        {
            dungeonType = s_DungeonType;
            stageIndex = s_StageIndex;
            return true;
        }

        public static void RegisterCrew(int index, int id)
        {
            s_CrewSlots[index] = id;
        }

        public static void UnregisterCrew(int index)
        {
            s_CrewSlots[index] = 0;
        }
    } // Scope by class DungeonMgr

} // namespace Root