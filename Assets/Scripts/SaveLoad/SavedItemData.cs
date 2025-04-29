using SkyDragonHunter.Database;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections.Generic;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedItem
    {
        public ItemTableData itemData;
        public BigNum count;
    }

    public class SavedItemData
    {
        public List<SavedItem> items;

        public void InitData()
        {
            items = new List<SavedItem>();

            var itemTable = DataTableMgr.ItemTable;
            var itemTableDatas = itemTable.Values;

            foreach (var itemTableData in itemTableDatas)
            {
                var savedItem = new SavedItem();
                savedItem.itemData = itemTableData;
                savedItem.count = 0;
                items.Add(savedItem);
            }
        }
        public void UpdateData()
        {

        }
    } // Scope by class SavedItemData

} // namespace Root