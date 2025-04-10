using SkyDragonHunter.Tables.Generic;

namespace SkyDragonHunter.Tables
{
    public class MasterySocketData : DataTableData
    {
        public int NextSocketID { get; set; }       // ��Ȱ������ -1
        public int StatType { get; set; }
        public string Stat { get; set; }
    }

    public class MasterySocketTable : DataTable<MasterySocketData>
    {

    } // Scope by class MasterySocketTable
} // namespace Root