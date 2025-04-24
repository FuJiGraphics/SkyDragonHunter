using SkyDragonHunter.Tables.Generic;

namespace SkyDragonHunter.Tables
{
    public class ItemData : DataTableData
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Type { get; set; }
        public int Unit { get; set; }
        public string Icon { get; set; }
        public bool Usable { get; set; }
    }

    public class ItemTableTemplate : DataTable<ItemData>
    {

    } // Scope by class ItemTable
} // namespace Root