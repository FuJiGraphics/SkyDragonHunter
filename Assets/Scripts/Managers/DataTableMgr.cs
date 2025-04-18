using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

namespace SkyDragonHunter.Managers 
{
    public static class DataTableMgr
    {
        // 속성 (Properties)
        public static Dictionary<string, DataTable> Tables { get; private set; }
        public static CrystalLevelTable CrystalLevelTable => Get<CrystalLevelTable>(DataTableIds.CrystalLevel);
        public static CrewTable CrewTable => Get<CrewTable>(DataTableIds.Crew);
        public static MonsterTable MonsterTable => Get<MonsterTable>(DataTableIds.Monster);
        public static BossTable BossTable => Get<BossTable>(DataTableIds.Boss);
        public static ArtifactTable ArtifactTable => Get<ArtifactTable>(DataTableIds.Artifact);
        public static MasteryNodeTable MasteryNodeTable => Get<MasteryNodeTable>(DataTableIds.MasteryNode);
        public static MasterySocketTable MasterySocketTable => Get<MasterySocketTable>(DataTableIds.MasterySocket);
        public static AilmentTable AilmentTable => Get<AilmentTable>(DataTableIds.Ailment);

        // Static Constructor
        static DataTableMgr()
        {
            Tables = new Dictionary<string, DataTable>();
            LoadTable<CrystalLevelTable>(SkyDragonHunter.Tables.DataTableIds.CrystalLevel);
            LoadTable<CrewTable>(DataTableIds.Crew);
            LoadTable<MonsterTable>(DataTableIds.Monster);
            LoadTable<BossTable>(DataTableIds.Boss);
            LoadTable<MasterySocketTable>(DataTableIds.MasterySocket);
            LoadTable<MasteryNodeTable>(DataTableIds.MasteryNode);
            LoadTable<AilmentTable>(DataTableIds.Ailment);
        }

        public static void InitForGameScene()
        {
            Tables = new Dictionary<string, DataTable>();
            LoadTable<CrystalLevelTable>(DataTableIds.CrystalLevel);
            LoadTable<CrewTable>(DataTableIds.Crew);
            LoadTable<MonsterTable>(DataTableIds.Monster);
            LoadTable<BossTable>(DataTableIds.Boss);
            LoadTable<MasterySocketTable>(DataTableIds.MasterySocket);
            LoadTable<MasteryNodeTable>(DataTableIds.MasteryNode);
            LoadTable<AilmentTable>(DataTableIds.Ailment);
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