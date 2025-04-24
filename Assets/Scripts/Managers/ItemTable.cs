using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Managers {

    public static class ItemTable
    {
        // 필드 (Fields)
        private static readonly int s_StartTableID = 101;
        private static readonly int s_EndTableID = 103;
        private static Dictionary<ItemType, ItemData> s_ItemMap;
        private static List<ItemData> s_SortedItems;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public static void Init()
        {
            s_ItemMap = new Dictionary<ItemType, ItemData>();
            s_SortedItems = new List<ItemData>();
            for (int tableId = s_StartTableID; tableId <= s_EndTableID; ++tableId)
            {
                var item = new ItemData(tableId);
                s_ItemMap.Add(item.Type, item);
                s_SortedItems.Add(item);
            }
        }

        public static void Release()
        {
            if (s_ItemMap == null)
                return;
            s_ItemMap.Clear();
        }

        public static ItemData Get(ItemType type)
            => s_ItemMap[type];

        public static ItemData[] ToList() 
            => s_SortedItems.ToArray();

        // Private 메서드
        // Others

    } // Scope by class Billboard
} // namespace SkyDragonHunter.Utility
