using SkyDragonHunter.Tables.Generic;

namespace SkyDragonHunter.Tables
{
    public class MasterySocketData : DataTableData
    {
        public int SocketID { get; set; }
        public int NextSocketID { get; set; }       // 비활성상태 -1
        public int StatType { get; set; }
        public string Stat { get; set; }
    }

    public class MasterySocketTable : DataTable<MasterySocketData>
    {

    } // Scope by class MasterySocketTable
} // namespace Root