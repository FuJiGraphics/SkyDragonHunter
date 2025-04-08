using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    [System.Serializable]
    public class UIEquipmentMountSlot
    {
        public int slotNumber;
        public Image slotNumberIcon;
        public GameObject crewInstance;
        public Button mountSlotButton;
        public Image crewIcon;
        public TextMeshProUGUI title;
        public TextMeshProUGUI crewName;
        public Image activeSkillIcon;
        public Image passiveSkillIcon;
        public Image damageIcon;
        public TextMeshProUGUI damageText;
        public Image armorIcon;
        public TextMeshProUGUI armorText;

        public void ResetSlot()
        {
            slotNumberIcon.sprite = null;
            crewIcon.sprite = null;
            title.text = "빈 슬롯";
            crewName.text = "빈 슬롯";
            activeSkillIcon.sprite = null;
            passiveSkillIcon.sprite = null;
            damageIcon.sprite = null;
            damageText.text = "0";
            armorIcon.sprite = null;
            armorText.text = "0";
            crewInstance = null;
        }

        public void SetSlot(GameObject crewGo)
        {
            if (crewGo.TryGetComponent<ICrewInfoProvider>(out var provider))
            {
                crewIcon.sprite = provider.Icon;
                title.text = provider.Title;
                crewName.text = provider.Name;

                if (crewGo.TryGetComponent<CharacterStatus>(out var stat))
                {
                    damageText.text = stat.Damage.ToString();
                    armorText.text = stat.Armor.ToString();
                }
                else
                {
                    Debug.LogWarning("[UIEquipmentMountSlot]: Crew의 CharacterStatus를 찾을 수 없습니다.");
                }

                // TODO: 추후 Active skil, Passive skill icon 등록 필요함
            }
            else
            {
                Debug.LogWarning("[UIEquipmentMountSlot]: Crew의 ICrewInfoProvider를 찾을 수 없습니다.");
            }
            crewInstance = crewGo;
        }
    }

    public class UIAssignUnitTofortressPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Mount Slot Settings")]
        [SerializeField] private Image m_DamageIcon;
        [SerializeField] private Image m_ArmorIcon;
        [SerializeField] private UIEquipmentMountSlot[] m_MountSlots;

        [Header("Crew Pick Panel Settings")]
        [SerializeField] private GameObject m_UiCrewPickContent;
        [SerializeField] private GameObject m_UiCrewPickNodePrefab;

        private List<GameObject> m_CrewPickNodeObjects;
        private GameObject m_PrevClickedCrew;
        private CrewEquipmentController m_AirshipEquipController; 

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            AdjustCallbackFuntionToMountSlot();
        }

        private void Start()
        {
            GameObject airshipGo = GameMgr.FindObject("Airship");
            m_AirshipEquipController = airshipGo.GetComponent<CrewEquipmentController>();
        }

        // Public 메서드
        public void SetSlot(int slot, GameObject crewGo)
        {
            if (slot < 0 || slot >= m_MountSlots.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "[UIAssignUnitTofortressPanel]: 슬롯 범위 초과");
            }
            m_MountSlots[slot].SetSlot(crewGo);
        }

        public void ResetSlot(int slot)
        {
            if (slot < 0 || slot >= m_MountSlots.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "[UIAssignUnitTofortressPanel]: 슬롯 범위 초과");
            }
            m_MountSlots[slot].ResetSlot();
        }

        public void AddCrewNode(GameObject crewInstance)
        {
            if (m_CrewPickNodeObjects == null)
            {
                m_CrewPickNodeObjects = new List<GameObject>();
            }

            if (crewInstance.TryGetComponent<ICrewInfoProvider>(out var provider))
            {
                GameObject nodeGo = Instantiate<GameObject>(m_UiCrewPickNodePrefab);
                m_CrewPickNodeObjects.Add(nodeGo);
                if (nodeGo.TryGetComponent<Image>(out var nodeIcon))
                {
                    nodeIcon.sprite = provider.Icon;
                }
                if (nodeGo.TryGetComponent<Button>(out var nodeButton))
                {
                    GameObject airshipGo = GameMgr.FindObject("Airship");
                    if (airshipGo.TryGetComponent<CrewEquipmentController>(out var equipController))
                    {
                        nodeButton.onClick.AddListener(() =>
                        {
                            m_PrevClickedCrew = crewInstance;
                        });
                        nodeGo.transform.SetParent(m_UiCrewPickContent.transform);
                    }
                    else
                    {
                        Debug.LogWarning("[UIAssignUnitTofortressPanel]: CrewEquipmentController를 찾을 수 없습니다.");
                    }
                }
            }
            else
            {
                Debug.LogWarning("[UICrewInfoPanel]: ICrewInfoProvider를 찾을 수 없습니다.");
            }
        }

        // Private 메서드
        private void AdjustCallbackFuntionToMountSlot()
        {
            foreach (var slot in m_MountSlots)
            {
                UIEquipmentMountSlot currentSlot = slot;
                slot.mountSlotButton.onClick.AddListener(() =>
                {
                    if (m_PrevClickedCrew != null)
                    {
                        m_PrevClickedCrew.SetActive(true);

                        if (slot.crewInstance != null)
                        {
                            m_AirshipEquipController.UnequipSlot(slot.crewInstance);
                        }
                        m_AirshipEquipController.EquipSlot(slot.slotNumber, m_PrevClickedCrew);
                        RemoveDuplicateCrew(m_PrevClickedCrew);
                        slot.SetSlot(m_PrevClickedCrew);
                        m_PrevClickedCrew = null;
                    }
                });
            }
        }

        private void RemoveDuplicateCrew(GameObject crewInstance)
        {
            foreach (var slot in m_MountSlots)
            {
                if (slot.crewInstance == crewInstance)
                {
                    slot.ResetSlot();
                    break;
                }
            }
        }

        // Others

    } // Scope by class UIFortressEquipment
} // namespace SkyDragonHunter