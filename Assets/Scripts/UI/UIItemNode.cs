using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIItemNode : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private Image m_ItemIcon;
        [SerializeField] private TextMeshProUGUI m_ItemCountText;
        [SerializeField] private Button m_ItemClickButton;
        [SerializeField] private GameObject m_SelectOutline;

        private ItemData m_Item;

        // 속성 (Properties)
        public Sprite ItemIcon => m_ItemIcon.sprite;
        public string ItemName => m_Item.Name;
        public string ItemDesc => m_Item.Desc;
        public ItemType ItemType => m_Item.Type;
        public ItemUnit UnitType => m_Item.Unit;
        public Button ClickButton => m_ItemClickButton;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void Init(ItemData item)
        {
            m_Item = item;
            m_ItemIcon.sprite = item.Icon;
            switch (item.Unit)
            {
                case ItemUnit.Number:
                    m_ItemCountText.text = AccountMgr.ItemCount(ItemType).ToString();
                    break;
                case ItemUnit.AlphaUnit:
                    m_ItemCountText.text = AccountMgr.ItemCount(ItemType).ToUnit();
                    break;
            }
        }

        public void SetSelectState(bool enabled)
            => m_SelectOutline?.SetActive(enabled);

        // Private 메서드
        // Others

    } // Scope by class UIItemNode
} // namespace SkyDragonHunter