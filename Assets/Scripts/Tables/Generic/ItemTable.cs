using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Tables.Generic;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.Tables
{

    public enum ItemType
    {
        None = 0,
        [Description("���")]Coin,
        [Description("���̾�")]Diamond,
        [Description("����ǰ")]Spoils,
        [Description("�ܿ� ")]CrewTicket,
        [Description("�ܿ� �̱��")]CanonTicket,
        [Description("������ �̱��")]RepairTicket,
        [Description("����")]Food,
        [Description("��Ϲ���")]Gear,
        [Description("������ ����ġ")]RepairExp,
        [Description("���̺� ���� �����")]WaveDungeonTicket,
        [Description("���� ���� �����")]BossDungeonTicket,
        [Description("����� ���� �����")]SandbagDungeonTicket,
        [Description("����")]Wood,
        [Description("����")]Brick,
        [Description("ö")]Steel,
        [Description("�����͸� ���� ���")]MasteryLevelUp,
        [Description("���� ���")]ExpansionMaterial,      // ���� ���
        [Description("������")]GrindingStone,          // ������
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
                #region Resources Only
                // if (m_CacheIconImage == null)
                // {
                //     m_CacheIconImage = ResourcesMgr.Load<GameObject>(IconName)?.GetComponent<Image>();
                // }
                // return m_CacheIconImage.sprite;
                #endregion

                return ResourcesMgr.Load<Sprite>(IconName);
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