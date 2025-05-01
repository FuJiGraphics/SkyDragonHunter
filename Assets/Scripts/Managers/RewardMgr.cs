using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Tables;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Managers {

    public static class RewardMgr
    {
        // 필드 (Fields)
        private static Dictionary<ItemType, int> s_Rewards = new();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public static void Add(ItemType type)
        {
            if (!s_Rewards.ContainsKey(type))
            {
                s_Rewards[type] = 0;
            }
            s_Rewards[type]++;
        }

        public static void Reset()
        {
            s_Rewards.Clear();
        }

        public static Dictionary<ItemType, int> GetRewards()
        {
            return s_Rewards;
        }

        // Private 메서드
        // Others

    } // Scope by class RewardMgr

} // namespace SkyDragonHunter.Managers
