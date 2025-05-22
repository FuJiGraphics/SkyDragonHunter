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
        [Description("골드")]Coin,
        [Description("다이아")]Diamond,
        [Description("전리품")]Spoils,
        [Description("단원 ")]CrewTicket,
        [Description("단원 뽑기권")]CanonTicket,
        [Description("수리공 뽑기권")]RepairTicket,
        [Description("음식")]Food,
        [Description("톱니바퀴")]Gear,
        [Description("수리공 경험치")]RepairExp,
        [Description("웨이브 던전 입장권")]WaveDungeonTicket,
        [Description("보스 던전 입장권")]BossDungeonTicket,
        [Description("샌드백 던전 입장권")]SandbagDungeonTicket,
        [Description("목재")]Wood,
        [Description("벽돌")]Brick,
        [Description("철")]Steel,
        [Description("마스터리 레벨 재료")]MasteryLevelUp,
        [Description("증축 재료")]ExpansionMaterial,      // 증축 재료
        [Description("연마석")]GrindingStone,          // 연마석
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

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
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

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
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