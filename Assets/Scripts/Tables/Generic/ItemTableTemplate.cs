using Newtonsoft.Json;
using SkyDragonHunter.Database;
using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Tables.Generic;
using System.IO;
using UnityEngine;

namespace SkyDragonHunter.Tables
{
    [JsonConverter(typeof(ItemTableDataConverter))]
    public class ItemTableData : DataTableData
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public ItemType Type { get; set; }
        public ItemUnit Unit { get; set; }
        public string IconFileName { get; set; }
        public bool Usable { get; set; }

        // Non Serialization target
        private const string IconFilePath = "Prefabs/Icons";
        public Sprite IconSprite
        {
            get
            {
                string iconPath = Path.Combine(IconFilePath, IconFileName);
                return ResourcesMgr.Load<Sprite>(iconPath);
            }
        }
    }

    public class ItemTableTemplate : DataTable<ItemTableData>
    {

    } // Scope by class ItemTable
} // namespace Root