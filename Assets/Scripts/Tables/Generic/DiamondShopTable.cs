using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using SkyDragonHunter.UI;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {

    public class DiamondShopData : DataTableData
    {
        public int ItemID { get; set; }                     // 아이템 데이터 아이디
        public int ItemAmount { get; set; }                 // 아이템 수량
        public BigNum Price { get; set; }                   // 아이템 가격
        public ShopRefreshType BuyLimitType { get; set; }  // 아이템 구매 수량
        public int ItemBuyLimit { get; set; }          // 아이템 구매 제한 수량
        public float GenWeight { get; set; }

        public ItemType GetItemType()
            => DataTableMgr.ItemTable.Get(ItemID).Type;
    }

    public class DiamondShopTable : DataTable<DiamondShopData>
    {
        public List<DiamondShopData> GetCategorizedItemList(ShopRefreshType category)
        {
            List<DiamondShopData> result = new();
            foreach (var goldShopData in m_dict.Values)
            {
                if (goldShopData.BuyLimitType == category)
                    result.Add(goldShopData);
            }
            return result;
        }

        public int GetWeightedRandomItemID(ShopRefreshType category)
        {
            var list = GetCategorizedItemList(category);
            if (list == null || list.Count == 0)
            {
                Debug.LogError($"No data found with category {category}, returning '0'");
                return 0;
            }

            var weightList = new List<float>();
            float totalWeight = 0f;
            for (int i = 0; i < list.Count; ++i)
            {
                totalWeight += list[i].GenWeight;
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
    } // Scope by class DiamondShopTable

} // namespace Root