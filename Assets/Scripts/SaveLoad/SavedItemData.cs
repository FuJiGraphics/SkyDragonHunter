using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System.Collections.Generic;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedItem
    {
        public ItemData itemData;
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
            SaveLoadMgr.UpdateSaveData(SaveDataTypes.Item);
            SaveLoadMgr.SaveGameData();
        }

        public void UpdateData()
        {
            foreach( var item in items)
            {
                switch (item.itemData.Type)
                {
                    case ItemType.None:
                        break;
                    case ItemType.Coin:
                        item.count = AccountMgr.Coin;
                        break;
                    case ItemType.Diamond:
                        item.count = AccountMgr.Diamond;
                        break;
                    case ItemType.WaveDungeonTicket:
                        item.count = AccountMgr.WaveDungeonTicket;
                        break;
                }
            }
        }

        public void ApplySavedData()
        {
            foreach (var item in items)
            {
                switch (item.itemData.Type)
                {
                    case ItemType.None:
                        break;
                    case ItemType.Coin:
                        AccountMgr.Coin = item.count;
                        break;
                    case ItemType.Diamond:
                        AccountMgr.Diamond = item.count;
                        break;
                    case ItemType.WaveDungeonTicket:
                        AccountMgr.WaveDungeonTicket = item.count;
                        break;
                }
            }
        }
    } // Scope by class SavedItemData

} // namespace Root