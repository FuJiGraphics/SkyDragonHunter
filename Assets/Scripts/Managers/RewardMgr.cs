using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Tables;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Managers {

    public static class RewardMgr
    {
        // �ʵ� (Fields)
        private static Dictionary<ItemType, int> s_Rewards = new();

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
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

        // Private �޼���
        // Others

    } // Scope by class RewardMgr

} // namespace SkyDragonHunter.Managers
