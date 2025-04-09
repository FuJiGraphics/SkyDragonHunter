using NPOI.OpenXmlFormats.Dml.Chart;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
                crewIcon.color = Color.white;
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
        [Header("Fortress Equipment Panel Settings")]
        [SerializeField] UIFortressEquipmentPanel m_EquipmentPanel;

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
                        GameObject crewGo = crewInstance;
                        nodeButton.onClick.AddListener(() =>
                        {
                            m_PrevClickedCrew = crewGo;
                        });
                        nodeGo.transform.SetParent(m_UiCrewPickContent.transform);
                        nodeGo.transform.localScale = Vector3.one;
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

        public void UnequipCrew(GameObject crewInstance)
        {
            RemoveDuplicateCrew(crewInstance);
        }

        public void UnequipSlot(int slot)
        {
            if (slot < 0 || slot >= m_MountSlots.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "[UIAssignUnitTofortressPanel]: 슬롯 범위 초과");
            }
            RemoveDuplicateCrew(m_MountSlots[slot].crewInstance);
        }

        public void EquipCrew(UIEquipmentMountSlot targetSlot, GameObject crewInstance)
        {
            if (targetSlot == null || crewInstance == null)
            {
                Debug.LogError("[UIAssignUnitTofortressPanel]: 장착 실패! targetSlot 또는 crewInstance가 null입니다.");
            }
            crewInstance.SetActive(true);
            var inSlot = FindSlotCrew(crewInstance);
            RemoveDuplicateCrew(crewInstance);
            GameObject outInstance = null;
            if (targetSlot.crewInstance != null)
            {
                outInstance = targetSlot.crewInstance;
                m_AirshipEquipController.UnequipSlot(targetSlot.crewInstance);
            }
            m_AirshipEquipController.EquipSlot(targetSlot.slotNumber, m_PrevClickedCrew);
            targetSlot.ResetSlot();
            targetSlot.SetSlot(m_PrevClickedCrew);
            m_EquipmentPanel.mountSlotIcons[targetSlot.slotNumber].sprite = targetSlot.crewIcon.sprite;

            if (inSlot != null && outInstance != null)
            {
                m_AirshipEquipController.EquipSlot(inSlot.slotNumber, outInstance);
                inSlot.ResetSlot();
                inSlot.SetSlot(outInstance);
                m_EquipmentPanel.mountSlotIcons[inSlot.slotNumber].sprite = inSlot.crewIcon.sprite;
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
                    if (m_PrevClickedCrew != null && slot.crewInstance != m_PrevClickedCrew)
                    {
                        EquipCrew(slot, m_PrevClickedCrew);
                        m_PrevClickedCrew = null;
                    }
                });
            }
        }

        private void RemoveDuplicateCrew(GameObject crewInstance)
        {
            if (crewInstance == null)
                return;

            for (int i = 0; i < m_MountSlots.Length; ++i)
            {
                if (m_MountSlots[i].crewInstance == crewInstance)
                {
                    m_AirshipEquipController.UnequipSlot(crewInstance);
                    m_MountSlots[i].ResetSlot();
                    m_EquipmentPanel.mountSlotIcons[i].sprite = null;
                    break;
                }
            }
        }


        private UIEquipmentMountSlot FindSlotCrew(GameObject crewInstance)
        {
            UIEquipmentMountSlot result = null;
            foreach (var slot in m_MountSlots)
            {
                if (slot.crewInstance == crewInstance)
                {
                    result = slot;
                    break;
                }
            }
            return result;
        }

        // Others

    } // Scope by class UIFortressEquipment
} // namespace SkyDragonHunter