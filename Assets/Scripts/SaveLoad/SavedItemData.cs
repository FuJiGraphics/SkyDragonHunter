using CsvHelper.TypeConversion;
using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedItem
    {
        public ItemType type;
        public BigNum count;
    }

    public class SavedItemData
    {
        public List<SavedItem> items;

        public void InitData()
        {
            items = new List<SavedItem>();
            var itemTypeCount = Enum.GetValues(typeof(ItemType)).Length;

            for(int i = 1; i < itemTypeCount; ++i) // including None
            {
                var newSavedItem = new SavedItem();
                newSavedItem.type = (ItemType)i;
                newSavedItem.count = 0;
                items.Add(newSavedItem);

                if (ItemType.CrewTicket == (ItemType)i)
                {
                    newSavedItem.count = 1;
                }
            }

            
            //RegisterEvent();
        }

        public void RegisterEvent()
        {
            AccountMgr.RemoveItemCountChangedEvent(OnItemCountChangedEvent);
            AccountMgr.AddItemCountChangedEvent(OnItemCountChangedEvent);
        }

        public void OnItemCountChangedEvent(ItemType itemType)
        {
            SaveLoadMgr.UpdateSaveData(SaveDataTypes.Item);
        }

        public void UpdateSavedData()
        {
            foreach(var item in items)
            {
                var newCount = AccountMgr.ItemCount(item.type);
                item.count = newCount;
            }
        }

        public void ApplySavedData()
        {
            foreach (var item in items)
            {
                AccountMgr.SetItemCount(item.type, item.count);
            }
        }
    } // Scope by class SavedItemData

} // namespace Root