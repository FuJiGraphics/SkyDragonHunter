using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class UIInventoryPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UIItemNode m_ItemNodePrefab;
        [SerializeField] private Transform m_Content;

        [SerializeField] private TextMeshProUGUI m_CoinText;
        [SerializeField] private TextMeshProUGUI m_DiamondText;
        [SerializeField] private TextMeshProUGUI m_SpoilsText;

        [SerializeField] private Image m_SelectIcon;
        [SerializeField] private TextMeshProUGUI m_SelectTitleText;
        [SerializeField] private TextMeshProUGUI m_SelectSubTitleText;
        [SerializeField] private TextMeshProUGUI m_SelectDescText;
        [SerializeField] private TextMeshProUGUI m_SelectCountText;

        private List<GameObject> m_GenNodes;

        // 속성 (Properties)
        public string CoinText 
        { 
            get => m_CoinText.text; 
            set => m_CoinText.text = value; 
        }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            foreach (var itemNode in m_GenNodes)
            {
                Destroy(itemNode);
            }
            m_GenNodes.Clear();
        }

        // Public 메서드
        // Private 메서드
        private void Init()
        {
            m_GenNodes = new List<GameObject>();
            var items = DataTableMgr.ItemTable.ToArray();
            foreach (var item in items)
            {
                AddItemNode(item);
            }
            m_CoinText.text = AccountMgr.Coin.ToUnit();

            if (m_GenNodes[0].TryGetComponent<UIItemNode>(out var itemNode))
            {
                itemNode.ClickButton.onClick?.Invoke();
            }
        }
        
        private void AddItemNode(ItemData item)
        {
            var itemNode = Instantiate<UIItemNode>(m_ItemNodePrefab);
            itemNode.Init(item);
            itemNode.ClickButton.onClick.AddListener(
                () => 
                {
                    ClearSelectOutline();
                    OnClickItemNode(itemNode);
                    itemNode.SetSelectState(true);
                });
            m_GenNodes.Add(itemNode.gameObject);
            itemNode.transform.SetParent(m_Content);
        }

        private void OnClickItemNode(UIItemNode clickedNode)
        {
            m_SelectIcon.sprite = clickedNode.ItemIcon;
            m_SelectTitleText.text = clickedNode.ItemName;
            m_SelectDescText.text = clickedNode.ItemDesc;
            if (clickedNode.UnitType == ItemUnit.Number)
                m_SelectCountText.text = AccountMgr.ItemCount(clickedNode.ItemType).ToString();
            else
                m_SelectCountText.text = AccountMgr.ItemCount(clickedNode.ItemType).ToUnit();
        }
        
        private void ClearSelectOutline()
        {
            foreach (var itemGo in m_GenNodes)
            {
                if (itemGo.TryGetComponent<UIItemNode>(out var itemNode))
                {
                    itemNode.SetSelectState(false);
                }
            }
        }

        // Others
    
    } // Scope by class UIInventoryPanel
} // namespace SkyDragonHunter