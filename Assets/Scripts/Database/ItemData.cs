using SkyDragonHunter.Managers;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.Database {

    public enum ItemType
    {
        None = 0,
        Coin,
        Diamond,
        Ticket,
    };

    public enum ItemUnit
    {
        Number,
        AlphaUnit,
    };

    public class ItemData
    {
        // 필드 (Fields)
        // 속성 (Properties)
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public Sprite Icon { get; private set; }
        public bool Usable { get; private set; }
        public ItemType Type { get; private set; }
        public ItemUnit UnitType { get; private set; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public ItemData(int tableId)
        {
            var itemData = DataTableMgr.ItemTable.Get(tableId);
            ID = tableId;
            Name = itemData.Name;
            Desc = itemData.Desc;

            string iconPath = Path.Combine("Prefabs/Icons", itemData.Icon);
            Icon = ResourcesMgr.Load<Image>(iconPath)?.sprite;
            Usable = itemData.Usable;
            Type = (ItemType)itemData.Type;
            UnitType = (ItemUnit)itemData.Unit;
        }

        // Private 메서드
        // Others
    
    } // Scope by class Item
} // namespace SkyDragonHunter