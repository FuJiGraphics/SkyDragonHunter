using SkyDragonHunter.Database;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        [SerializeField] private Button m_UiCanonEquipButton;
        [SerializeField] private Button m_UiCanonUnequipButton;

        private List<GameObject> m_CanonPickNodeObjects;
        private ClickedCanonInfo m_ClickedCanonInfo;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_UiCanonEquipButton.onClick.AddListener(Equip);
            m_UiCanonUnequipButton.onClick.AddListener(Unequip);
            m_ClickedCanonInfo = new();
        }

        private void OnEnable()
        {
            if (m_CanonPickNodeObjects == null)
            {
                m_CanonPickNodeObjects = new List<GameObject>();
            }
            ReleaseAllNodes();
            LoadCanonInfoFromAccount();
        }

        // Public 메서드
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
                }
                // Icon만 등록함
                GameObject.Destroy(canonInstance);
            }

            if (nodeGo.TryGetComponent<Button>(out var nodeButton))
            {
                nodeButton.onClick.AddListener(() =>
                {
                    if (m_ClickedCanonInfo.prevPickedNodeInstance != null &&
                    m_ClickedCanonInfo.prevPickedNodeInstance.TryGetComponent<Image>(out var prevImage))
                    {
                        prevImage.color = Color.white;
                    }
                    m_ClickedCanonInfo.Set(nodeButton.gameObject, canonDummy);
                    if (m_ClickedCanonInfo.prevPickedNodeInstance.TryGetComponent<Image>(out var nextImage))
                    {
                        nextImage.color = Color.red;
                    }
                });
                nodeGo.transform.SetParent(m_UiCanonPickContent.transform);
                nodeGo.transform.localScale = Vector3.one;
            }
        }

        public void Equip()
        {
            if (m_ClickedCanonInfo.IsNull)
                return;

            GameObject airshipInstance = GameMgr.FindObject("Airship");
            if (airshipInstance == null)
                return;

            if (airshipInstance.TryGetComponent<CanonExecutor>(out var executor))
            {
                executor.Equip(m_ClickedCanonInfo.prevClickedCanonDummy);
                if (m_ClickedCanonInfo.prevPickedNodeInstance.TryGetComponent<Image>(out var image))
                {
                    image.color = Color.white;
                }
                m_ClickedCanonInfo.prevClickedCanonDummy.IsEquip = true;
            }
            else
            {
                Debug.LogWarning("[UICanonEquipmentPanel]: CanonExecutor 찾을 수 없습니다.");
            }

            m_ClickedCanonInfo.Clear();

            // 세이브
            AccountMgr.SaveUserData();
        }

        public void Unequip()
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
            }
            else
            {
                Debug.LogWarning("[UICanonEquipmentPanel]: CanonExecutor 찾을 수 없습니다.");
            }

            m_ClickedCanonInfo.Clear();

            // 세이브
            AccountMgr.SaveUserData();
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