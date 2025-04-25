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

    public class UICanonEquipmentPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Canon Pick Panel Settings")]
        [SerializeField] private GameObject m_UiCanonPickContent;
        [SerializeField] private GameObject m_UiCanonPickNodePrefab;
        [SerializeField] private Button m_UiCanonEquipButton;
        [SerializeField] private Button m_UiCanonUnequipButton;

        private List<GameObject> m_CanonPickNodeObjects;
        private GameObject m_PrevPickedNodeInstance;
        private GameObject m_PrevClickedCanonInstance;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            m_UiCanonEquipButton.onClick.AddListener(Equip);
            m_UiCanonUnequipButton.onClick.AddListener(Unequip);
            LoadCanonInfoFromAccount();
        }

        // Public 메서드
        public void AddCanonNode(GameObject canonInstance)
        {
            if (m_CanonPickNodeObjects == null)
            {
                m_CanonPickNodeObjects = new List<GameObject>();
            }

            if (canonInstance.TryGetComponent<ICanonInfoProvider>(out var provider))
            {
                GameObject nodeGo = Instantiate<GameObject>(m_UiCanonPickNodePrefab);
                m_CanonPickNodeObjects.Add(nodeGo);
                if (nodeGo.TryGetComponent<UICanonSlot>(out var canonNodeSlot))
                {
                    canonNodeSlot.CanonIcon.sprite = provider.Icon;
                }
                if (nodeGo.TryGetComponent<Button>(out var nodeButton))
                {
                    nodeButton.onClick.AddListener(() => 
                    {
                        if (m_PrevPickedNodeInstance != null && 
                        m_PrevPickedNodeInstance.TryGetComponent<Image>(out var prevImage))
                        {
                            prevImage.color = Color.white;
                        }
                        m_PrevPickedNodeInstance = nodeButton.gameObject;
                        m_PrevClickedCanonInstance = canonInstance;
                        if (m_PrevPickedNodeInstance.TryGetComponent<Image>(out var nextImage))
                        {
                            nextImage.color = Color.red;
                        }
                    });
                    nodeGo.transform.SetParent(m_UiCanonPickContent.transform);
                    nodeGo.transform.localScale = Vector3.one;
                }
            }
            else
            {
                Debug.LogWarning("[UICanonEquipmentPanel]: ICanonInfoProvider를 찾을 수 없습니다.");
            }
        }

        public void Equip()
        {
            if (m_PrevClickedCanonInstance == null || m_PrevPickedNodeInstance == null)
                return;

            GameObject airshipInstance = GameMgr.FindObject("Airship");
            if (airshipInstance == null)
                return;

            if (airshipInstance.TryGetComponent<CanonExecutor>(out var executor))
            {
                executor.Equip(m_PrevClickedCanonInstance);
                if (m_PrevPickedNodeInstance.TryGetComponent<Image>(out var image))
                {
                    image.color = Color.white;
                }
            }
            else
            {
                Debug.LogWarning("[UICanonEquipmentPanel]: CanonExecutor 찾을 수 없습니다.");
            }

            m_PrevClickedCanonInstance = null;
            m_PrevPickedNodeInstance = null;
        }

        public void Unequip()
        {
            GameObject airshipInstance = GameMgr.FindObject("Airship");
            if (airshipInstance == null)
                return;

            if (airshipInstance.TryGetComponent<CanonExecutor>(out var executor))
            {
                executor.Unequip();
                if (m_PrevPickedNodeInstance.TryGetComponent<Image>(out var image))
                {
                    image.color = Color.white;
                }
            }
            else
            {
                Debug.LogWarning("[UICanonEquipmentPanel]: CanonExecutor 찾을 수 없습니다.");
            }
        }

        // Private 메서드
        private void LoadCanonInfoFromAccount()
        {
            var canonDummys = AccountMgr.HeldCanons;
            if (canonDummys != null)
            {
                foreach (var canonDummy in canonDummys)
                {
                    GameObject canonInstance = canonDummy.GetCanonInstance();
                    AddCanonNode(canonInstance);
                }
            }
        }
        // Others

    } // Scope by class UICannonEquipmentPanel
} // namespace SkyDragonHunter