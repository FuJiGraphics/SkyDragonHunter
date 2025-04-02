using SkyDragonHunter.Tables;
using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Managers 
{
    public static class DataTableManager
    {
        // 속성 (Properties)
        public static Dictionary<string, DataTable> Tables {  get; private set; }
        public static CrystalLevelTable CrystalLevelTable => Get<CrystalLevelTable>(DataTableIds.CrystalLevel);
        public static CrewTable CrewTable => Get<CrewTable>(DataTableIds.Crew);
        public static MonsterTable MonsterTable => Get<MonsterTable>(DataTableIds.Monster);
        public static BossTable BossTable => Get<BossTable>(DataTableIds.Boss);

        // Static Constructor
        static DataTableManager()
        {
            Tables = new Dictionary<string, DataTable>();
            LoadTable<CrystalLevelTable>(DataTableIds.CrystalLevel);
            LoadTable<CrewTable>(DataTableIds.Crew);
            LoadTable<MonsterTable>(DataTableIds.Monster);
            LoadTable<BossTable>(DataTableIds.Boss);
        }

        public static void InitForGameScene()
        {
            Tables = new Dictionary<string, DataTable>();
            LoadTable<CrystalLevelTable>(DataTableIds.CrystalLevel);
            LoadTable<CrewTable>(DataTableIds.Crew);
            LoadTable<MonsterTable>(DataTableIds.Monster);
            LoadTable<BossTable>(DataTableIds.Boss);
        }

        // Public 메서드
        public static T Get<T>(string id) where T : DataTable
        {
            if(!Tables.ContainsKey(id))
            {
                Debug.LogError($"Table id {id} does not exist");
                return null;
            }
            return Tables[id] as T;
        }

        public static void LoadTable<T>(string fileName) where T : DataTable, new()
        {
            var table = new T();
            var id = fileName;
            if (Tables.ContainsKey(id))
            {
                Debug.LogError($"Table Id {id} 충돌");
            }
            table.Load(id);
            Tables.Add(id, table);
        }

        public static void UnloadAll()
        {
            Tables.Clear();
        }
    } // Scope by class DataTableManager

} // namespace Root