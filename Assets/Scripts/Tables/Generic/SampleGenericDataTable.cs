using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace SkyDragonHunter.Tables
{
    public class SampleData : DataTableData
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class SampleGenericDataTable : DataTable<SampleData>
    {        
    } // Scope by class SampleGenericDataTable

} // namespace Root