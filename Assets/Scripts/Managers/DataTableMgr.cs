using SkyDragonHunter.Tables;
using SkyDragonHunter.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public static AdditionalStatTable AdditionalStatTable => Get<AdditionalStatTable>(DataTableIds.AdditionalStat);
        public static MasteryNodeTable MasteryNodeTable => Get<MasteryNodeTable>(DataTableIds.MasteryNode);
        public static MasterySocketTable MasterySocketTable => Get<MasterySocketTable>(DataTableIds.MasterySocket);
        public static AilmentTable AilmentTable => Get<AilmentTable>(DataTableIds.Ailment);
        public static DefaultGrowthTable DefaultGrowthTable => Get<DefaultGrowthTable>(DataTableIds.DefaultGrowth);
        public static ItemTable ItemTable => Get<ItemTable>(DataTableIds.Item);
        public static StageTable StageTable => Get<StageTable>(DataTableIds.Stage);
        public static AFKRewardTable AFKRewardTable => Get<AFKRewardTable>(DataTableIds.AFKReward);
        public static WaveTable WaveTable => Get<WaveTable>(DataTableIds.Wave);
        public static RepairTableTemplate RepairTable => Get<RepairTableTemplate>(DataTableIds.Repair);
        public static GoldShopTable GoldShopTable => Get<GoldShopTable>(DataTableIds.GoldShop);
        public static DiamondShopTable DiamondShopTable => Get<DiamondShopTable>(DataTableIds.DiamondShop);
        public static RerollShopTable RerollShopTable => Get<RerollShopTable>(DataTableIds.RerollShop);

        // Static Constructor
        static DataTableMgr()
        {
            Tables = new Dictionary<string, DataTable>();
            LoadTable<CrystalLevelTable>(DataTableIds.CrystalLevel);
            LoadTable<CrewTable>(DataTableIds.Crew);
            LoadTable<MonsterTable>(DataTableIds.Monster);
            LoadTable<BossTable>(DataTableIds.Boss);
            LoadTable<MasterySocketTable>(DataTableIds.MasterySocket);
            LoadTable<MasteryNodeTable>(DataTableIds.MasteryNode);
            LoadTable<AilmentTable>(DataTableIds.Ailment);
            LoadTable<DefaultGrowthTable>(DataTableIds.DefaultGrowth);
            LoadTable<ItemTable>(DataTableIds.Item);
            LoadTable<StageTable>(DataTableIds.Stage);
            LoadTable<AFKRewardTable>(DataTableIds.AFKReward);
            LoadTable<WaveTable>(DataTableIds.Wave);
            LoadTable<RepairTableTemplate>(DataTableIds.Repair);
            LoadTable<GoldShopTable>(DataTableIds.GoldShop);
            LoadTable<DiamondShopTable>(DataTableIds.DiamondShop);
            LoadTable<RerollShopTable>(DataTableIds.RerollShop);
            // LoadTable<ArtifactTable>(DataTableIds.Artifact);
            // LoadTable<AdditionalStatTable>(DataTableIds.AdditionalStat);
        }

        public static void ForceAwake()
        {
            // does nothing, tool to trigger static constructor
        }

        public static void InitOnSceneLoaded(Scene scene)
        {
            switch ((SceneIds)scene.buildIndex)
            {
                case SceneIds.GameScene:
                    InitForGameScene();
                    break;
                case SceneIds.DungeonScene:
                    InitForDungeonScene();
                    break;
            }
        }

        private static void InitForGameScene()
        {
            if (Tables != null && Tables.Count != 0)
                return;
            if (Tables == null)
                Tables = new Dictionary<string, DataTable>();
            else
                Tables.Clear();
            LoadTable<CrystalLevelTable>(DataTableIds.CrystalLevel);
            LoadTable<CrewTable>(DataTableIds.Crew);
            LoadTable<MonsterTable>(DataTableIds.Monster);
            LoadTable<BossTable>(DataTableIds.Boss);
            LoadTable<MasterySocketTable>(DataTableIds.MasterySocket);
            LoadTable<MasteryNodeTable>(DataTableIds.MasteryNode);
            LoadTable<AilmentTable>(DataTableIds.Ailment);
            LoadTable<DefaultGrowthTable>(DataTableIds.DefaultGrowth);
            LoadTable<ItemTable>(DataTableIds.Item);
            LoadTable<StageTable>(DataTableIds.Stage);
            LoadTable<AFKRewardTable>(DataTableIds.AFKReward);
            LoadTable<WaveTable>(DataTableIds.Wave);
            LoadTable<RepairTableTemplate>(DataTableIds.Repair);
            LoadTable<GoldShopTable>(DataTableIds.GoldShop);
            LoadTable<DiamondShopTable>(DataTableIds.DiamondShop);
            LoadTable<RerollShopTable>(DataTableIds.RerollShop);
            // LoadTable<ArtifactTable>(DataTableIds.Artifact);
            // LoadTable<AdditionalStatTable>(DataTableIds.AdditionalStat);
        }

        private static void InitForDungeonScene()
        {
            if (Tables != null && Tables.Count != 0)
                return;
            if (Tables == null)
                Tables = new Dictionary<string, DataTable>();
            else
                Tables.Clear();
            LoadTable<CrystalLevelTable>(DataTableIds.CrystalLevel);
            LoadTable<CrewTable>(DataTableIds.Crew);
            LoadTable<MonsterTable>(DataTableIds.Monster);
            LoadTable<BossTable>(DataTableIds.Boss);
            LoadTable<MasterySocketTable>(DataTableIds.MasterySocket);
            LoadTable<MasteryNodeTable>(DataTableIds.MasteryNode);
            LoadTable<AilmentTable>(DataTableIds.Ailment);
            LoadTable<DefaultGrowthTable>(DataTableIds.DefaultGrowth);
            LoadTable<ItemTable>(DataTableIds.Item);
            LoadTable<StageTable>(DataTableIds.Stage);
            LoadTable<AFKRewardTable>(DataTableIds.AFKReward);
            LoadTable<WaveTable>(DataTableIds.Wave);
            LoadTable<RepairTableTemplate>(DataTableIds.Repair);
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

        public static void Release()
        {
            Tables.Clear();
        }
    } // Scope by class DataTableManager

} // namespace Root