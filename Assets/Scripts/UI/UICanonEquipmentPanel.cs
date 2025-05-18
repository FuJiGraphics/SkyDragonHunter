using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class ClickedCanonInfo
    {
        public GameObject prevPickedNodeInstance;
        public GameObject prevClickedCanonInstance;
        public CanonDummy prevClickedCanonDummy;

        public bool IsNull => 
            prevPickedNodeInstance == null ||
            prevClickedCanonDummy == null;

        public void Set(GameObject node, CanonDummy dummy)
        {
            prevPickedNodeInstance = node; 
            prevClickedCanonDummy = dummy;
        }

        public void Clear()
        {
            prevPickedNodeInstance = null;
            prevClickedCanonDummy = null;
        }
    }

    public class UICanonEquipmentPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Canon Pick Panel Settings")]
        [SerializeField] private GameObject m_UiCanonPickContent;
        [SerializeField] private GameObject m_UiCanonPickNodePrefab;

        [Header("Canon Other Panels")]
        [SerializeField] private UICanonInfoPanel m_UiCanonInfoPanel;

        private List<GameObject> m_CanonPickNodeObjects;
        private ClickedCanonInfo m_ClickedCanonInfo = new();

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            if (m_ClickedCanonInfo.IsNull)
            {
                m_UiCanonInfoPanel.gameObject.SetActive(false);
            }
            
            if (m_ClickedCanonInfo.IsNull || !m_ClickedCanonInfo.prevClickedCanonDummy.IsUnlock)
            {
                m_UiCanonInfoPanel.EquipButton.interactable = false;
                m_UiCanonInfoPanel.EquipButton.gameObject.SetActive(true);
                m_UiCanonInfoPanel.UnequipButton.gameObject.SetActive(false);
            }
            else
            {
                if (!m_ClickedCanonInfo.IsNull && m_ClickedCanonInfo.prevClickedCanonDummy.IsEquip)
                {
                    m_UiCanonInfoPanel.EquipButton.interactable = false;
                    m_UiCanonInfoPanel.EquipButton.gameObject.SetActive(false);
                    m_UiCanonInfoPanel.UnequipButton.gameObject.SetActive(true);
                }
                else
                {
                    m_UiCanonInfoPanel.EquipButton.interactable = true;
                    m_UiCanonInfoPanel.EquipButton.gameObject.SetActive(true);
                    m_UiCanonInfoPanel.UnequipButton.gameObject.SetActive(false);
                }

            }
        }

        // Public 메서드
        public void Init()
        {
            if (m_CanonPickNodeObjects == null)
            {
                m_CanonPickNodeObjects = new List<GameObject>();
            }
            ReleaseAllNodes();
            LoadCanonInfoFromAccount();
        }

        public void AddCanonNode(CanonDummy canonDummy)
        {
            GameObject nodeGo = Instantiate<GameObject>(m_UiCanonPickNodePrefab);
            m_CanonPickNodeObjects.Add(nodeGo);

            var canonInstance = canonDummy.GetCanonInstance();
            if (nodeGo.TryGetComponent<UICanonSlot>(out var canonNodeSlot))
            {
                if (canonInstance.TryGetComponent<ICanonInfoProvider>(out var provider))
                {
                    canonNodeSlot.CanonIcon.sprite = provider.Icon;
                    canonNodeSlot.CanonIcon.color = provider.Color;
                    canonNodeSlot.CanonDummy = canonDummy;
                    canonNodeSlot.ShowIdleIconImage();
                    canonNodeSlot.UIUpdateUnlockState();
                }
            }

            if (nodeGo.TryGetComponent<Button>(out var nodeButton))
            {
                nodeButton.onClick.AddListener(() =>
                {
                    if (m_ClickedCanonInfo.prevPickedNodeInstance != null &&
                    m_ClickedCanonInfo.prevPickedNodeInstance.TryGetComponent<UICanonSlot>(out var prevSlot))
                    {
                        prevSlot.ShowIdleIconImage();
                        prevSlot.UIUpdateUnlockState();
                    }
                    m_ClickedCanonInfo.Set(nodeButton.gameObject, canonDummy);
                    m_UiCanonInfoPanel.gameObject.SetActive(true);
                    m_UiCanonInfoPanel.ShowInfo(canonDummy);
                    if (m_ClickedCanonInfo.prevPickedNodeInstance.TryGetComponent<UICanonSlot>(out var nextSlot))
                    {
                        nextSlot.ShowDownIconImage();
                        nextSlot.UIUpdateUnlockState();
                    };
                });
                nodeGo.transform.SetParent(m_UiCanonPickContent.transform);
                nodeGo.transform.localScale = Vector3.one;
            }
        }

        public void OnEquip()
        {
            if (m_ClickedCanonInfo.IsNull)
                return;

            if (!m_ClickedCanonInfo.prevClickedCanonDummy.IsUnlock)
            {
                DrawableMgr.Dialog("Alert", "장착 실패! 캐논이 잠겨있습니다.");
                return;
            }

            GameObject airshipInstance = GameMgr.FindObject("Airship");
            if (airshipInstance == null)
                return;

            if (airshipInstance.TryGetComponent<CanonExecutor>(out var executor))
            {
                executor.Unequip();
                executor.Equip(m_ClickedCanonInfo.prevClickedCanonDummy);
                if (m_ClickedCanonInfo.prevPickedNodeInstance.TryGetComponent<Image>(out var image))
                {
                    image.color = Color.white;
                }
                m_ClickedCanonInfo.prevClickedCanonDummy.IsEquip = true;

                var infoUiPanel = GameMgr.FindObject<UIFortressEquipmentPanel>("UIFortressEquipmentPanel");
                var canonInstance = m_ClickedCanonInfo.prevClickedCanonDummy.GetCanonInstance();
                if (infoUiPanel != null && 
                    canonInstance.TryGetComponent<ICanonInfoProvider>(out var provider))
                {
                    infoUiPanel.SetCanonIcon(0, provider.Icon);
                    infoUiPanel.SetCanonIconColor(0, provider.Color);
                }
            }
            else
            {
                Debug.LogWarning("[UICanonEquipmentPanel]: CanonExecutor 찾을 수 없습니다.");
            }

            m_ClickedCanonInfo.Clear();
            SaveLoadMgr.CallSaveGameData();
        }

        public void OnUnequip()
        {
            if (m_ClickedCanonInfo.IsNull)
                return;

            GameObject airshipInstance = GameMgr.FindObject("Airship");
            if (airshipInstance == null)
                return;

            if (airshipInstance.TryGetComponent<CanonExecutor>(out var executor))
            {
                executor.Unequip();
                if (m_ClickedCanonInfo.prevPickedNodeInstance.TryGetComponent<Image>(out var image))
                {
                    image.color = Color.white;
                }
                m_ClickedCanonInfo.prevClickedCanonDummy.IsEquip = false;

                var infoUiPanel = GameMgr.FindObject<UIFortressEquipmentPanel>("UIFortressEquipmentPanel");
                infoUiPanel?.ResetCanonIcon(0);
            }
            else
            {
                Debug.LogWarning("[UICanonEquipmentPanel]: CanonExecutor 찾을 수 없습니다.");
            }

            m_ClickedCanonInfo.Clear();
            SaveLoadMgr.CallSaveGameData();
        }

        public void OnClickedOtherPanel()
        {
            if (m_ClickedCanonInfo.IsNull)
                return;

            ClearClickedPanel();
        }

        public void ClearClickedPanel()
        {
            m_UiCanonInfoPanel.gameObject.SetActive(false);

            if (m_ClickedCanonInfo.prevPickedNodeInstance != null &&
                    m_ClickedCanonInfo.prevPickedNodeInstance.TryGetComponent<Image>(out var prevImage))
            {
                prevImage.color = Color.white;
            }
            m_ClickedCanonInfo.Clear();
        }

        // Private 메서드
        private void LoadCanonInfoFromAccount()
        {
            var canonDummys = AccountMgr.HeldCanons;
            if (canonDummys != null)
            {
                foreach (var canonDummy in canonDummys)
                {
                    AddCanonNode(canonDummy);
                }
            }
            DirtyAllNodes();
        }

        private void DirtyAllNodes()
        {
            if (m_CanonPickNodeObjects == null)
                return;

            foreach (var node in m_CanonPickNodeObjects)
            {
                if (node.TryGetComponent<UICanonSlot>(out var slot))
                {
                    slot.ShowIdleIconImage();
                    slot.UIUpdateUnlockState();
                }
            }
        }

        private void ReleaseAllNodes()
        {
            if (m_CanonPickNodeObjects == null)
                return;

            foreach (var node in m_CanonPickNodeObjects)
            {
                GameObject.Destroy(node);
            }
            m_CanonPickNodeObjects.Clear();
        }

        // Others

    } // Scope by class UICannonEquipmentPanel
} // namespace SkyDragonHunter