using Org.BouncyCastle.Security;
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
        private static int[] s_ClearedStageIndex;

        // Public Fields
        public static int[] m_DungeonStageCounts = { 5, 6, 7 };

        // Temp
        private static int s_SlotCount = 4;
        public static int TicketCount;

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
            s_ClearedStageIndex = new int[3];
        }

        public static void EnterDungeon(DungeonType dungeonType, int stageIndex)
        {
            s_DungeonType = dungeonType;
            s_StageIndex = stageIndex;
            SceneChangeMgr.LoadScene((int)SceneIds.DungeonScene);
        }

        public static bool TryGetStageData(out DungeonType dungeonType, out int stageIndex)
        {
            dungeonType = s_DungeonType;
            stageIndex = s_StageIndex;
            return true;
        }

        public static void OnStageClear()
        {
            if(s_ClearedStageIndex[(int)s_DungeonType] < s_StageIndex)
            {
                s_ClearedStageIndex[(int)s_DungeonType] = s_StageIndex;
            }
            TicketCount--;
        }

        public static int GetClearedStage(DungeonType dungeonType)
        {
            return s_ClearedStageIndex[(int)dungeonType];
        }

        public static void SetClearedStage(DungeonType dungeonType, int clearedStage)
        {
            s_ClearedStageIndex[(int)dungeonType] = clearedStage;
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