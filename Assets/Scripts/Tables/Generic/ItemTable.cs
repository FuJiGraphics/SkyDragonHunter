using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Tables.Generic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.Tables {

    public enum ItemType
    {
        None = 0,
        Coin,
        Diamond,
        Spoils,
        CrewTicket,
        CanonTicket,
        RepairTicket,
        Food,
        Gear,
        RepairExp,
        WaveDungeonTicket,
        BossDungeonTicket,
        SandbagDungeonTicket,
        Wood,
        Brick,
        Steel,
        MasteryLevelUp,
        ExpansionMaterial,      // ���� ���
        GrindingStone,          // ������
    };

    public enum ItemUnit
    {
        Number,
        AlphaUnit,
    };

    public enum ItemGrade
    {
        Rare, Unique, Epic, Legend
    }

    public class ItemData : DataTableData
    {
        public string Name { get; set; }
        public string IconName { get; set; }
        public string Desc { get; set; }
        public ItemType Type { get; set; }
        public ItemUnit Unit { get; set; }
        public bool Usable { get; set; }
        public ItemGrade Grade { get; set; }

        private Image m_CacheIconImage = null;

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        public Sprite Icon
        {
            get
            {
                if (m_CacheIconImage == null)
                {
                    m_CacheIconImage = ResourcesMgr.Load<GameObject>(IconName)?.GetComponent<Image>();
                }
                return m_CacheIconImage.sprite;
            }
        }
    }

    public class ItemTable : DataTable<ItemData>
    {
        private Dictionary<ItemType, ItemData> m_ItemMap;

        // �Ӽ� (Properties)
        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)
        // Public �޼���
        public void Init()
        {
            if (m_ItemMap != null)
                return;

            m_ItemMap = new Dictionary<ItemType, ItemData>();
            var sortedList = base.ToArray();
            foreach (var element in sortedList)
            {
                m_ItemMap.Add(element.Type, element);
            }
        }

        public ItemData Get(ItemType type)
        {
            Init();
            return m_ItemMap[type];
        }

    } // Scope by class ItemTable
} // namespace Root