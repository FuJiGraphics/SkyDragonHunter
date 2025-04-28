using Newtonsoft.Json;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
        public void UpdateData()
        {

        }
    } // Scope by class SavedItemData

} // namespace Root