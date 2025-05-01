using SkyDragonHunter.Tables.Generic;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Tables
{
    public class SampleData : DataTableData
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"ID:{ID} Name:{Name} Value: {Value}");
            return sb.ToString();
        }
    }

    public class SampleGenericDataTable : DataTable<SampleData>
    {     
        
    } // Scope by class SampleGenericDataTable

} // namespace Root