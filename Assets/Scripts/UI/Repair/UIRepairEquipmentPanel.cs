using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    public class ClickedRepairInfo
    {
        public GameObject prevPickedNodeInstance;
        public GameObject prevClickedRepairInstance;
        public RepairDummy prevClickedRepairDummy;

        public bool IsNull =>
            prevPickedNodeInstance == null ||
            prevClickedRepairDummy == null;

        public void Set(GameObject node, RepairDummy dummy)
        {
            prevPickedNodeInstance = node;
            prevClickedRepairDummy = dummy;
        }

        public void Clear()
        {
            prevPickedNodeInstance = null;
            prevClickedRepairDummy = null;
        }
    }

    public class UIRepairEquipmentPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Repair Pick Panel Settings")]
        [SerializeField] private GameObject m_UiRepairPickContent;
        [SerializeField] private GameObject m_UiRepairPickNodePrefab;

        [Header("Repair Other Panels")]
        [SerializeField] private UIRepairInfoPanel m_UiRepairInfoPanel;

        private List<GameObject> m_RepairPickNodeObjects;
        private ClickedRepairInfo m_ClickedRepairInfo;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_ClickedRepairInfo = new();
        }

        private void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            if (m_ClickedRepairInfo.IsNull)
            {
                m_UiRepairInfoPanel.gameObject.SetActive(false);
            }

            if (m_ClickedRepairInfo.IsNull || !m_ClickedRepairInfo.prevClickedRepairDummy.IsUnlock)
            {
                m_UiRepairInfoPanel.EquipButton.interactable = false;
                m_UiRepairInfoPanel.EquipButton.gameObject.SetActive(true);
                m_UiRepairInfoPanel.UnequipButton.gameObject.SetActive(false);
            }
            else
            {
                if (!m_ClickedRepairInfo.IsNull && m_ClickedRepairInfo.prevClickedRepairDummy.IsEquip)
                {
                    m_UiRepairInfoPanel.EquipButton.interactable = false;
                    m_UiRepairInfoPanel.EquipButton.gameObject.SetActive(false);
                    m_UiRepairInfoPanel.UnequipButton.gameObject.SetActive(true);
                }
                else
                {
                    m_UiRepairInfoPanel.EquipButton.interactable = true;
                    m_UiRepairInfoPanel.EquipButton.gameObject.SetActive(true);
                    m_UiRepairInfoPanel.UnequipButton.gameObject.SetActive(false);
                }

            }
        }

        // Public 메서드
        public void Init()
        {
            if (m_RepairPickNodeObjects == null)
            {
                m_RepairPickNodeObjects = new List<GameObject>();
            }
            ReleaseAllNodes();
            LoadRepairInfoFromAccount();
        }

        public void AddRepairNode(RepairDummy RepairDummy)
        {
            GameObject nodeGo = Instantiate<GameObject>(m_UiRepairPickNodePrefab);
            m_RepairPickNodeObjects.Add(nodeGo);

            if (nodeGo.TryGetComponent<UIRepairSlot>(out var RepairNodeSlot))
            {
                RepairNodeSlot.RepairIcon.sprite = RepairDummy.Icon;
                RepairNodeSlot.RepairDummy = RepairDummy;
                RepairNodeSlot.ShowIdleIconImage();
                RepairNodeSlot.UIUpdateUnlockState();
            }

            if (nodeGo.TryGetComponent<Button>(out var nodeButton))
            {
                nodeButton.onClick.AddListener(() =>
                {
                    if (m_ClickedRepairInfo.prevPickedNodeInstance != null &&
                    m_ClickedRepairInfo.prevPickedNodeInstance.TryGetComponent<UIRepairSlot>(out var prevSlot))
                    {
                        prevSlot.ShowIdleIconImage();
                        prevSlot.UIUpdateUnlockState();
                    }
                    m_ClickedRepairInfo.Set(nodeButton.gameObject, RepairDummy);
                    m_UiRepairInfoPanel.gameObject.SetActive(true);
                    m_UiRepairInfoPanel.ShowInfo(RepairDummy);
                    if (m_ClickedRepairInfo.prevPickedNodeInstance.TryGetComponent<UIRepairSlot>(out var nextSlot))
                    {
                        nextSlot.ShowDownIconImage();
                        nextSlot.UIUpdateUnlockState();
                    };
                });
                nodeGo.transform.SetParent(m_UiRepairPickContent.transform);
                nodeGo.transform.localScale = Vector3.one;
            }
        }

        public void OnEquip()
        {
            if (m_ClickedRepairInfo.IsNull)
                return;

            if (!m_ClickedRepairInfo.prevClickedRepairDummy.IsUnlock)
            {
                DrawableMgr.Dialog("Alert", "장착 실패! 수리공이 잠겨있습니다.");
                return;
            }

            GameObject airshipInstance = GameMgr.FindObject("Airship");
            if (airshipInstance == null)
                return;

            if (airshipInstance.TryGetComponent<RepairExecutor>(out var executor))
            {
                executor.Unequip();
                executor.Equip(m_ClickedRepairInfo.prevClickedRepairDummy);
                if (m_ClickedRepairInfo.prevPickedNodeInstance.TryGetComponent<Image>(out var image))
                {
                    image.color = Color.white;
                }
                m_ClickedRepairInfo.prevClickedRepairDummy.IsEquip = true;

                var infoUiPanel = GameMgr.FindObject<UIFortressEquipmentPanel>("UIFortressEquipmentPanel");
                if (infoUiPanel != null)
                {
                    infoUiPanel.SetRepairIcon(0, m_ClickedRepairInfo.prevClickedRepairDummy.Icon);
                    infoUiPanel.SetRepairIconColor(0, m_ClickedRepairInfo.prevClickedRepairDummy.Color);
                }
            }
            else
            {
                Debug.LogWarning("[UIRepairEquipmentPanel]: RepairExecutor 찾을 수 없습니다.");
            }

            m_ClickedRepairInfo.Clear();
            SaveLoadMgr.CallSaveGameData();
        }

        public void OnUnequip()
        {
            if (m_ClickedRepairInfo.IsNull)
                return;

            GameObject airshipInstance = GameMgr.FindObject("Airship");
            if (airshipInstance == null)
                return;

            if (airshipInstance.TryGetComponent<RepairExecutor>(out var executor))
            {
                executor.Unequip();
                if (m_ClickedRepairInfo.prevPickedNodeInstance.TryGetComponent<Image>(out var image))
                {
                    image.color = Color.white;
                }
                m_ClickedRepairInfo.prevClickedRepairDummy.IsEquip = false;

                var infoUiPanel = GameMgr.FindObject<UIFortressEquipmentPanel>("UIFortressEquipmentPanel");
                if (infoUiPanel != null)
                {
                    infoUiPanel.ResetRepairIcon(0);
                }
            }
            else
            {
                Debug.LogWarning("[UIRepairEquipmentPanel]: RepairExecutor 찾을 수 없습니다.");
            }

            m_ClickedRepairInfo.Clear();
            SaveLoadMgr.CallSaveGameData();
        }

        public void OnClickedOtherPanel()
        {
            if (m_ClickedRepairInfo.IsNull)
                return;

            ClearClickedPanel();
        }

        public void ClearClickedPanel()
        {
            m_UiRepairInfoPanel.gameObject.SetActive(false);

            if (m_ClickedRepairInfo.prevPickedNodeInstance != null &&
                    m_ClickedRepairInfo.prevPickedNodeInstance.TryGetComponent<Image>(out var prevImage))
            {
                prevImage.color = Color.white;
            }
            m_ClickedRepairInfo.Clear();
        }

        // Private 메서드
        private void LoadRepairInfoFromAccount()
        {
            var RepairDummys = AccountMgr.HeldRepairs;
            if (RepairDummys != null)
            {
                foreach (var RepairDummy in RepairDummys)
                {
                    AddRepairNode(RepairDummy);
                }
            }
            DirtyAllNodes();
        }

        private void DirtyAllNodes()
        {
            if (m_RepairPickNodeObjects == null)
                return;

            foreach (var node in m_RepairPickNodeObjects)
            {
                if (node.TryGetComponent<UIRepairSlot>(out var slot))
                {
                    slot.ShowIdleIconImage();
                    slot.UIUpdateUnlockState();
                }
            }
        }

        private void ReleaseAllNodes()
        {
            if (m_RepairPickNodeObjects == null)
                return;

            foreach (var node in m_RepairPickNodeObjects)
            {
                GameObject.Destroy(node);
            }
            m_RepairPickNodeObjects.Clear();
        }

        // Others

    } // Scope by class UIRepairEquipmentPanel
} // namespace SkyDragonHunter