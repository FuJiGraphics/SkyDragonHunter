using SkyDragonHunter.Tables.Generic;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Tables
{
    public class DefaultGrowthData : DataTableData
    {
        public int StatType { get; set; }
        public string StatName { get; set; }
        public string BasicStat { get; set; }
        public string BasicCost { get; set; }
        public string StatIncrease { get; set; }
        public string CostIncrease { get; set; }
        public int MaxLevel { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }
    }

    public class DefaultGrowthTable : DataTable<DefaultGrowthData>
    {

    } // Scope by class DefaultGrowthData

} // namespace Root