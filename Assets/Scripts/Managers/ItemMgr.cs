using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Managers {

    public static class ItemMgr
    {
        // 필드 (Fields)
        private static readonly int s_StartTableID = 101;
        private static readonly int s_EndTableID = 103;
        private static Dictionary<ItemType, Item> s_CollectedItems;
        private static List<Item> s_SortedItems;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public static void Init()
        {
            s_CollectedItems = new Dictionary<ItemType, Item>();
            s_SortedItems = new List<Item>();
            for (int tableId = s_StartTableID; tableId <= s_EndTableID; ++tableId)
            {
                var item = new Item(tableId);
                s_CollectedItems.Add(item.Type, item);
                s_SortedItems.Add(item);
            }
        }

        public static void Release()
        {
            if (s_CollectedItems == null)
                return;
            s_CollectedItems.Clear();
        }

        public static Item GetItem(ItemType type)
        {
            Item result = null;
            if (s_CollectedItems.TryGetValue(type, out var item))
            {
                result = item;
            }
            else
            {
                Debug.LogWarning($"[ItemMgr]: 아이템을 찾을 수 없습니다. {type}");
            }
            return result;
        }

        public static Item[] GetItemList() 
            => s_SortedItems.ToArray();

        // Private 메서드
        // Others

    } // Scope by class Billboard
} // namespace SkyDragonHunter.Utility
