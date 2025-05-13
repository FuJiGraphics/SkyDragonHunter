using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using SkyDragonHunter.UI;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Tables {
    
    public class GoldShopData : DataTableData
    {
        public int ItemID { get; set; }                     // ������ ������ ���̵�
        public int ItemAmount { get; set; }                 // ������ ����
        public BigNum Price { get; set; }                   // ������ ����
        public ShopCategory BuyLimitType { get; set; }  // ������ ���� ����
        public int ItemBuyLimit { get; set; }          // ������ ���� ���� ����
        public float GenWeight { get; set; }

        public ItemType GetItemType() 
            => DataTableMgr.ItemTable.Get(ItemID).Type;
    }

    public class GoldShopTable : DataTable<GoldShopData>
    {
        public List<GoldShopData> GetCategorizedItemList(ShopCategory category)
        {
            List<GoldShopData> result = new();
            foreach(var goldShopData in m_dict.Values)
            {
                if (goldShopData.BuyLimitType == category)
                    result.Add(goldShopData);
            }
            return result;
        }

        public int GetWeightedRandomItemID(ShopCategory category)
        {
            var list = GetCategorizedItemList(category);
            if(list == null || list.Count == 0)
            {
                Debug.LogError($"No data found with category {category}, returning '0'");
                return 0;
            }

            var weightList = new List<float>();
            float totalWeight = 0f;
            for(int i = 0; i < list.Count; ++i)
            {
                totalWeight += list[i].GenWeight;
                weightList.Add(totalWeight);
            }
            var randomVal = Random.Range(0f, totalWeight);
            int index = 0;
            for(int i = 0; i < weightList.Count; ++i)
            {
                index = i;
                if (randomVal > weightList[i])
                    continue;
                else
                    break;
            }

            return list[index].ID;
        }
    } // Scope by class GoldShopTable

} // namespace Root