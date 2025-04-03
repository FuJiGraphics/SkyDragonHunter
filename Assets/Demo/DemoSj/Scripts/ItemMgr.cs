using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace SkyDragonHunter.Test {

    public static class ItemMgr
    {
        // 필드 (Fields)
        public static Dictionary<ItemType, int> dropCounts = new();
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드

        public static void Add(ItemType type)
        {
            if (!dropCounts.ContainsKey(type))
            {
                dropCounts[type] = 0;
            }
            dropCounts[type]++;
        }
        
        public static void Reset()
        {
            dropCounts.Clear();
        }

        public static Dictionary<ItemType, int> GetDropCounts()
        {
            return dropCounts;
        }
        // Private 메서드
        // Others

    } // Scope by class ItemMgr

} // namespace Root