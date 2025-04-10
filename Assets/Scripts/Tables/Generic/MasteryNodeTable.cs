using SkyDragonHunter.Tables.Generic;

namespace SkyDragonHunter.Tables
{
    public class MasteryNodeData : DataTableData
    {
        public int NodeID { get; set; }
        public int[] NextNodeIds { get; set; }
        public string SocketID { get; set; }
        public string Level { get; set; }
        public string Position { get; set; }
    }

    public class MasteryNodeTable : DataTable<MasteryNodeData>
    {

    } // Scope by class MasteryNodeTable

} // namespace Root