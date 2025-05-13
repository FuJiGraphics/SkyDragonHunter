using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class RerollShopData : DataTableData
    {
        public int ItemID { get; set; }
        public int ItemAmount { get; set; }
        public BigNum Price { get; set; }
        public float RerollRate { get; set; }
        public int AffinityLevel { get; set; }

        public ItemType GetItemType()
            => DataTableMgr.ItemTable.Get(ItemID).Type;
    }

    public class RerollShopTable : DataTable<RerollShopData>
    {
        public List<RerollShopData> GetItemListWithinFavorabilityLevel(int favorabilityLevel)
        {
            if(favorabilityLevel < 1 && favorabilityLevel > 5)
            {
                Debug.LogError($"Favorability level out of range");
                return null;
            }
            List<RerollShopData> result = new();
            foreach (var rerollShopData in m_dict.Values)
            {
                if (rerollShopData.AffinityLevel <= favorabilityLevel)
                    result.Add(rerollShopData);                
            }
            return result;
        }

        public int GetWeightedRandomItemID(int favorabilityLevel)
        {
            var list = GetItemListWithinFavorabilityLevel(favorabilityLevel);
            if (list == null || list.Count == 0)
            {
                Debug.LogError($"No data found with favorability level [{favorabilityLevel}], returning '0'");
                return 0;
            }

            var weightList = new List<float>();
            float totalWeight = 0f;
            for (int i = 0; i < list.Count; ++i)
            {
                totalWeight += list[i].RerollRate;
                weightList.Add(totalWeight);
            }
            var randomVal = Random.Range(0f, totalWeight);
            int index = 0;
            for (int i = 0; i < weightList.Count; ++i)
            {
                index = i;
                if (randomVal > weightList[i])
                    continue;
                else
                    break;
            }
            return list[index].ID;
        }
    } // Scope by class RerollShopTable

} // namespace Root