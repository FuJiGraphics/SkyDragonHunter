using CsvHelper.Configuration.Attributes;
using SkyDragonHunter.Tables.Generic;

namespace SkyDragonHunter.Tables
{
    public class MasteryNodeData : DataTableData
    {
        [TypeConverter(typeof(IntArrayConverter))]
        public int[] NextNodeIds { get; set; }
        public int SocketID { get; set; }
        public int Level { get; set; }
        public int Position { get; set; }
    }

    public class MasteryNodeTable : DataTable<MasteryNodeData>
    {

    } // Scope by class MasteryNodeTable

} // namespace Root