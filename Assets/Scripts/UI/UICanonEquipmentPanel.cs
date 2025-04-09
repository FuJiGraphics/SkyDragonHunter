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

        private List<GameObject> m_CanonPickNodeObjects;
        private GameObject m_PrevClicked_Canon;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            
        }
    
        private void Update()
        {
            
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
                    GameObject airshipGo = GameMgr.FindObject("Airship");
                    nodeButton.onClick.AddListener(() => 
                    {
                        GameObject airship = airshipGo;
                        if (airship.TryGetComponent<CanonExecutor>(out var canonExecutor))
                        {
                            canonExecutor.Equip(canonInstance);
                        }
                        else
                        {
                            Debug.LogWarning("[UICanonEquipmentPanel]: Button을 찾을 수 없습니다.");
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

        // Private 메서드
        // Others

    } // Scope by class UICannonEquipmentPanel
} // namespace SkyDragonHunter