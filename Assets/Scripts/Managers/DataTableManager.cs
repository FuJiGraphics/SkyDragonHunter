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
        public static SampleGenericDataTable SampleTable => Get<SampleGenericDataTable>(DataTableIds.Sample);
        public static CrystalLevelTable CrystalLevelTable => Get<CrystalLevelTable>(DataTableIds.CrystalLevel);

        // Static Constructor
        static DataTableManager()
        {
            Tables = new Dictionary<string, DataTable>();

            var sampleTable = new SampleGenericDataTable();
            var sampleTableId = DataTableIds.Sample;
            sampleTable.Load(sampleTableId);
            Tables.Add(sampleTableId, sampleTable);

            var crystalLevelTable = new CrystalLevelTable();
            var crystalLevelId = DataTableIds.CrystalLevel;
            crystalLevelTable.Load(crystalLevelId);
            Tables.Add(crystalLevelId, crystalLevelTable);
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
    } // Scope by class DataTableManager

} // namespace Root