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

            RegisterEvent();
        }

        public void RegisterEvent()
        {
            AccountMgr.AddItemCountChangedEvent(OnItemCountChangedEvent);
        }

        public void OnItemCountChangedEvent(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.None:
                    break;
                case ItemType.Coin:
                    UpdateData();
                    SaveLoadMgr.SaveGameData();
                    break;
                case ItemType.Diamond:
                    break;
                case ItemType.Ticket:
                    break;
            }
        }

        public void UpdateData()
        {
            foreach( var item in items)
            {
                if(item.itemData.Type == ItemType.Coin)
                {
                    item.count = AccountMgr.Coin;
                }
            }

        }

        public void ApplySavedData()
        {
            foreach (var item in items)
            {
                if (item.itemData.Type == ItemType.Coin)
                {
                    AccountMgr.Coin = item.count;
                }
            }
        }
    } // Scope by class SavedItemData

} // namespace Root