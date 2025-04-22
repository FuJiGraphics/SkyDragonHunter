using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Scriptables;
using SkyDragonHunter.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.Gameplay {

    public class Item : MonoBehaviour
        , IItemType
        , IDescribable
    {
        // 필드 (Fields)
        [Header("Item Table Info")]
        [SerializeField] private int m_TableID;

        // 속성 (Properties)
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public Sprite Icon { get; private set; }
        public bool Usable { get; private set; }
        public ItemType Type { get; private set; }
        public ItemUnit UnitType { get; private set; }
        public int ItemCount { get; set; } = 0;

        // 외부 종속성 필드 (External dependencies field)
        private IItemEffect[] m_ItemEffects;

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            Init();
        }

        // Public 메서드
        public void Init()
        {
            m_ItemEffects = GetComponentsInChildren<IItemEffect>(true);
            var itemData = DataTableMgr.ItemTable.Get(m_TableID);
            Name = itemData.Name;
            Desc = itemData.Desc;
            Icon = ResourcesMgr.Load<Image>(itemData.Icon).sprite;
            Usable = itemData.Usable;
            Type = (ItemType)itemData.Type;
            UnitType = (ItemUnit)itemData.Unit;
            ItemCount = 0;
        }

        public void Use(GameObject target)
        {
            if (m_ItemEffects == null)
                return;
            if (Usable == false)
                return;

            foreach (var effect in m_ItemEffects)
            {
                effect.Execute(target);
            }
        }

        // Private 메서드
        // Others
    
    } // Scope by class Item
} // namespace SkyDragonHunter