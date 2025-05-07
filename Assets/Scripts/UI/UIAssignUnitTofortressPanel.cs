using NPOI.OpenXmlFormats.Dml.Chart;
using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Test;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI {

    [System.Serializable]
    public class UIEquipmentMountSlot
    {
        public int slotNumber;
        public GameObject crewInstance;
        // TODO: LJH
        public int crewId;
        // ~TODO
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
        public GameObject detailsAndUnequipGo;
        public Button details;
        public Button unequip;

        public void ResetSlot()
        {
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

            // TODO: LJH
            crewId = 0;
            // ~TODO
        }

        //public void SetSlot(GameObject crewGo)
        //{
        //    if (crewGo.TryGetComponent<ICrewInfoProvider>(out var provider))
        //    {
        //        crewIcon.sprite = provider.Icon;
        //        crewIcon.color = Color.white;
        //        title.text = provider.Title;
        //        crewName.text = provider.Name;
        //
        //        if (crewGo.TryGetComponent<CharacterStatus>(out var stat))
        //        {
        //            damageText.text = stat.Damage.ToUnit();
        //            armorText.text = stat.Armor.ToUnit();
        //        }
        //        else
        //        {
        //            Debug.LogWarning("[UIEquipmentMountSlot]: Crew의 CharacterStatus를 찾을 수 없습니다.");
        //        }
        //
        //        // TODO: 추후 Active skil, Passive skill icon 등록 필요함
        //    }
        //    else
        //    {
        //        Debug.LogWarning("[UIEquipmentMountSlot]: Crew의 ICrewInfoProvider를 찾을 수 없습니다.");
        //    }
        //    crewInstance = crewGo;
        //}

        public void SetSlot(int crewId)
        {
            if (!SaveLoadMgr.GameData.savedCrewData.GetSavedCrew(crewId, out var savedCrewData))
            {
                Debug.LogError($"[UIAssignUnitToFortressPanel]: Set Slot failed, cannot find crew with id '{crewId}'");
                return;
            }
            var crewIconSprite = savedCrewData.crewData.IconSprite;
            if (crewIconSprite != null)
            {
                crewIcon.sprite = crewIconSprite;
            }
            else
            {
                Debug.LogError($"[UIAssignUnitToFortressPanel]: Crew Icon Sprite null on id '{crewId}'");
            }
            crewIcon.color = Color.white;
            title.text = savedCrewData.crewData.UnitType.ToString();
            crewName.text = savedCrewData.crewData.UnitName;

            damageText.text = savedCrewData.CurrentDamage().ToUnit();
            armorText.text = savedCrewData.CurrentHP().ToUnit();

            this.crewId = crewId;
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
        [SerializeField] private GameObject m_UiAllEquipArrows;

        private List<GameObject> m_CrewPickNodeObjects;
        private GameObject m_PrevClickedCrew;

        // TODO: LJH
        private int m_PrevClickedCrewId;
        // ~TODO
        private CrewEquipmentController m_AirshipEquipController;
        private List<CrewNode> m_CrewListNodeObjects;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            m_UiAllEquipArrows.SetActive(false);
            ClearAllNodeDetailsAndUnequipPanels();
            ClearAllNodeDetails();
            m_PrevClickedCrew = null;

            // TODO: LJH
            m_PrevClickedCrewId = 0;
            // ~TODO
        }

        // TODO: LJH
        private void Awake()
        {
            m_CrewListNodeObjects = new List<CrewNode>();
        }
        // ~TODO

        // Public 메서드
        public void Init()
        {
            AdjustCallbackFuntionToMountSlot();
            GameObject airshipGo = GameMgr.FindObject("Airship");
            m_AirshipEquipController = airshipGo.GetComponent<CrewEquipmentController>();
        }

        //public void SetSlot(int slot, GameObject crewGo)
        //{
        //    if (slot < 0 || slot >= m_MountSlots.Length)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(slot), "[UIAssignUnitTofortressPanel]: 슬롯 범위 초과");
        //    }
        //    m_MountSlots[slot].SetSlot(crewGo);
        //}

        public void ResetSlot(int slot)
        {
            if (slot < 0 || slot >= m_MountSlots.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "[UIAssignUnitTofortressPanel]: 슬롯 범위 초과");
            }
            m_MountSlots[slot].ResetSlot();
        }

        //public void AddCrewNode(GameObject crewInstance)
        //{
        //    if (m_CrewPickNodeObjects == null)
        //    {
        //        m_CrewPickNodeObjects = new List<GameObject>();
        //    }
        //    if (m_CrewListNodeObjects == null)
        //    {
        //        m_CrewListNodeObjects = new List<CrewNode>();
        //    }
        //
        //    if (crewInstance.TryGetComponent<ICrewInfoProvider>(out var provider))
        //    {
        //        GameObject nodeGo = Instantiate<GameObject>(m_UiCrewPickNodePrefab);
        //        m_CrewPickNodeObjects.Add(nodeGo);
        //        nodeGo.name = provider.Name + "(Slot)";
        //
        //        m_CrewListNodeObjects.Add(new CrewNode { crewNode = nodeGo, crewInstance = crewInstance });
        //        if (nodeGo.TryGetComponent<UICrewInfoNode>(out var crewInfoNode))
        //        {
        //            GameObject target = crewInstance;
        //            crewInfoNode.UIDetailsButton.onClick.AddListener(
        //                () => { OnClickedNodeDetails(target); });
        //        }
        //        if (nodeGo.TryGetComponent<Image>(out var nodeIcon))
        //        {
        //            nodeIcon.sprite = provider.Icon;
        //        }
        //        if (nodeGo.TryGetComponent<Button>(out var nodeButton))
        //        {
        //            GameObject airshipGo = GameMgr.FindObject("Airship");
        //            if (airshipGo.TryGetComponent<CrewEquipmentController>(out var equipController))
        //            {
        //                GameObject crewGo = crewInstance;
        //                nodeButton.onClick.AddListener(() =>
        //                {
        //                    ClearAllNodeDetailsAndUnequipPanels();
        //                    m_PrevClickedCrew = crewGo;
        //                    ClearAllNodeDetails();
        //                    crewInfoNode.UIDetails.SetActive(true);
        //                    m_UiAllEquipArrows.SetActive(true);
        //                });
        //                nodeGo.transform.SetParent(m_UiCrewPickContent.transform);
        //                nodeGo.transform.localScale = Vector3.one;
        //            }
        //            else
        //            {
        //                Debug.LogWarning("[UIAssignUnitTofortressPanel]: CrewEquipmentController를 찾을 수 없습니다.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogWarning("[UICrewInfoPanel]: ICrewInfoProvider를 찾을 수 없습니다.");
        //    }
        //}

        public void AddCrewNode(int crewID)
        {
            if (m_CrewPickNodeObjects == null)
            {
                m_CrewPickNodeObjects = new List<GameObject>();
            }
            if (m_CrewListNodeObjects == null)
            {
                m_CrewListNodeObjects = new List<CrewNode>();
            }

            if (!SaveLoadMgr.GameData.savedCrewData.GetSavedCrew(crewID, out var savedCrewData))
            {
                Debug.LogError($"[UIAssignUnitToFortressPanel]: Cannot find crew with id '{crewID}'");
                return;
            }
            var nodeGo = Instantiate(m_UiCrewPickNodePrefab);
            m_CrewPickNodeObjects.Add(nodeGo);
            nodeGo.name = savedCrewData.crewData.UnitName + "(Slot)";

            m_CrewListNodeObjects.Add(new CrewNode { crewNode = nodeGo, crewId = crewID });

            if (nodeGo.TryGetComponent<UICrewInfoNode>(out var crewInfoNode))
            {               
                crewInfoNode.UIDetailsButton.onClick.AddListener(
                    () => { OnClickedNodeDetails(crewID); });
            }
            if (nodeGo.TryGetComponent<Image>(out var nodeIcon))
            {
                var crewIconSprite = DataTableMgr.CrewTable.Get(crewID).IconSprite;
                if(crewIconSprite == null)
                {
                    Debug.LogError($"[UIAssignUnitFortressPanel]: crew icon sprite could not be found with id '{crewID}'");
                }
                else
                {
                    nodeIcon.sprite = crewIconSprite;
                }
            }
            if (nodeGo.TryGetComponent<Button>(out var nodeButton))
            {
                GameObject airshipGo = GameMgr.FindObject("Airship");
                if (airshipGo.TryGetComponent<CrewEquipmentController>(out var equipController))
                {                    
                    nodeButton.onClick.AddListener(() =>
                    {
                        ClearAllNodeDetailsAndUnequipPanels();
                        m_PrevClickedCrewId = crewID;
                        ClearAllNodeDetails();
                        crewInfoNode.UIDetails.SetActive(true);
                        m_UiAllEquipArrows.SetActive(true);
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

        public void UnequipCrew(GameObject crewInstance)
        {
            RemoveDuplicateCrew(crewInstance);
        }

        public void UnequipCrew(int crewId)
        {
            RemoveDuplicateCrew(crewId);
        }

        public void UnequipSlot(int slot)
        {
            if (slot < 0 || slot >= m_MountSlots.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "[UIAssignUnitTofortressPanel]: 슬롯 범위 초과");
            }
            RemoveDuplicateCrew(m_MountSlots[slot].crewInstance);
        }

        //public void EquipCrew(UIEquipmentMountSlot targetSlot, GameObject crewInstance)
        //{
        //    if (targetSlot == null || crewInstance == null)
        //    {
        //        Debug.LogError("[UIAssignUnitTofortressPanel]: 장착 실패! targetSlot 또는 crewInstance가 null입니다.");
        //    }
        //    crewInstance.SetActive(true);
        //    var inSlot = FindSlotCrew(crewInstance);
        //    RemoveDuplicateCrew(crewInstance);
        //    GameObject outInstance = null;
        //    if (targetSlot.crewInstance != null)
        //    {
        //        outInstance = targetSlot.crewInstance;
        //        m_AirshipEquipController.UnequipSlot(targetSlot.crewInstance);
        //    }
        //    m_AirshipEquipController.EquipSlot(targetSlot.slotNumber, m_PrevClickedCrew);
        //    targetSlot.ResetSlot();
        //    targetSlot.SetSlot(m_PrevClickedCrew);
        //    m_EquipmentPanel.mountSlotIcons[targetSlot.slotNumber].sprite = targetSlot.crewIcon.sprite;
        //
        //    if (inSlot != null && outInstance != null)
        //    {
        //        m_AirshipEquipController.EquipSlot(inSlot.slotNumber, outInstance);
        //        inSlot.ResetSlot();
        //        inSlot.SetSlot(outInstance);
        //        m_EquipmentPanel.mountSlotIcons[inSlot.slotNumber].sprite = inSlot.crewIcon.sprite;
        //    }
        //}

        public void EquipCrew(UIEquipmentMountSlot targetSlot, int crewId)
        {
            if (targetSlot == null || crewId == 0)
            {
                Debug.LogError("[UIAssignUnitTofortressPanel]: 장착 실패! targetSlot null 또는 crewId가 0입니다.");
            }
            //crewInstance.SetActive(true);
            var inSlot = FindSlotCrew(crewId);
            RemoveDuplicateCrew(crewId);
            int outId = 0;
            //if (targetSlot.crewInstance != null)
            //{
            //    outInstance = targetSlot.crewInstance;
            //    m_AirshipEquipController.UnequipSlot(targetSlot.crewInstance);
            //}
            if (targetSlot.crewId != 0)
            {
                outId = targetSlot.crewId;
                m_AirshipEquipController.UnequipSlot(targetSlot.crewId, false);
            }

            m_AirshipEquipController.EquipSlot(targetSlot.slotNumber, m_PrevClickedCrewId);
            targetSlot.ResetSlot();
            targetSlot.SetSlot(m_PrevClickedCrewId);
            m_EquipmentPanel.mountSlotIcons[targetSlot.slotNumber].sprite = targetSlot.crewIcon.sprite;

            if (inSlot != null && outId != 0)
            {
                m_AirshipEquipController.EquipSlot(inSlot.slotNumber, outId);
                inSlot.ResetSlot();
                inSlot.SetSlot(outId);
                m_EquipmentPanel.mountSlotIcons[inSlot.slotNumber].sprite = inSlot.crewIcon.sprite;
            }
        }

        //public void EquipCrew(int slot, GameObject crewInstance)
        //{
        //    string findName = crewInstance.GetComponent<ICrewInfoProvider>().Name + "(Slot)";
        //    GameObject targetButton = null;
        //    foreach (var nodeGo in m_CrewPickNodeObjects)
        //    {
        //        if (nodeGo.name == findName)
        //        {
        //            targetButton = nodeGo;
        //            break;
        //        }
        //    }
        //    if (targetButton == null)
        //    {
        //        Debug.LogError($"[UIAssignUnitTofortressPanel]: 장착 실패. crewInstance를 찾을 수 없습니다. / {crewInstance.name}");
        //        return;
        //    }
        //    targetButton?.GetComponent<Button>().onClick?.Invoke();
        //    m_MountSlots[slot].mountSlotButton.onClick.Invoke();
        //}

        public void EquipCrew(int slot, int crewId)
        {
            if(!SaveLoadMgr.GameData.savedCrewData.GetSavedCrew(crewId, out var savedCrewData))
            {
                Debug.LogError($"[UIAssignUnitToFortressPanel]: EquipCrewFailed, no crew found with id '{crewId}'");
                return;
            }
            string findName = savedCrewData.crewData.UnitName + "(Slot)";
            GameObject targetButton = null;
            foreach(var nodeGo in m_CrewPickNodeObjects)
            {
                if(nodeGo.name == findName)
                {
                    targetButton = nodeGo;
                }
            }
            if (targetButton == null)
            {
                Debug.LogError($"[UIAssignUnitTofortressPanel]: 장착 실패. crewInstance를 찾을 수 없습니다. / {savedCrewData.crewData.UnitName}");
                return;
            }
            targetButton?.GetComponent<Button>().onClick?.Invoke();
            m_MountSlots[slot].mountSlotButton.onClick.Invoke();
        }

        // Private 메서드
        private void AdjustCallbackFuntionToMountSlot()
        {
            //foreach (var slot in m_MountSlots)
            //{
            //    UIEquipmentMountSlot currentSlot = slot;
            //
            //    currentSlot.details.onClick.AddListener(() =>
            //    {
            //        ClearAllNodeDetailsAndUnequipPanels();
            //        if (currentSlot.crewInstance != null)
            //        {
            //            OnClickedNodeDetails(currentSlot.crewInstance);
            //        }
            //        else
            //        {
            //            Debug.LogError("[UIAssignUnitTofortressPanel]: Crew Instance is null!");
            //        }
            //    });
            //
            //    currentSlot.unequip.onClick.AddListener(() =>
            //    {
            //        ClearAllNodeDetailsAndUnequipPanels();
            //        if (currentSlot.crewInstance != null)
            //        {
            //            UnequipCrew(currentSlot.crewInstance);
            //        }
            //        else
            //        {
            //            Debug.LogError("[UIAssignUnitTofortressPanel]: Crew Instance is null!");
            //        }
            //    });
            //
            //    // 장착 슬롯 버튼 이벤트 추가하는 곳
            //    slot.mountSlotButton.onClick.AddListener(() =>
            //    {
            //        ClearAllNodeDetails();
            //        m_UiAllEquipArrows.SetActive(false);
            //        if (m_PrevClickedCrew == null && slot.crewInstance != null)
            //        {
            //            bool active = currentSlot.detailsAndUnequipGo.activeSelf;
            //            ClearAllNodeDetailsAndUnequipPanels();
            //            currentSlot.detailsAndUnequipGo.SetActive(!active);
            //        }
            //        else if (m_PrevClickedCrew != null && slot.crewInstance != m_PrevClickedCrew)
            //        {
            //            ClearAllNodeDetailsAndUnequipPanels();
            //            EquipCrew(slot, m_PrevClickedCrew);
            //            m_PrevClickedCrew = null;
            //        }
            //        else
            //        {
            //            m_PrevClickedCrew = null;
            //        }
            //    });
            //}

            foreach (var slot in m_MountSlots)
            {
                UIEquipmentMountSlot currentSlot = slot;

                currentSlot.details.onClick.AddListener(() =>
                {
                    ClearAllNodeDetailsAndUnequipPanels();
                    if (currentSlot.crewId != 0)
                    {
                        OnClickedNodeDetails(currentSlot.crewId);
                    }
                    else
                    {
                        Debug.LogError($"Crew Id {currentSlot.crewId}");
                    }

                });

                currentSlot.unequip.onClick.AddListener(() =>
                {
                    ClearAllNodeDetailsAndUnequipPanels();
                    if (currentSlot.crewId != 0)
                    {
                        UnequipCrew(currentSlot.crewId);
                    }
                    else
                    {
                        Debug.LogError($"Crew Id {currentSlot.crewId}");
                    }
                });

                slot.mountSlotButton.onClick.AddListener(() =>
                {
                    ClearAllNodeDetails();
                    m_UiAllEquipArrows.SetActive(false);
                    if(m_PrevClickedCrewId == 0 && slot.crewId != 0)
                    {
                        bool active = currentSlot.detailsAndUnequipGo.activeSelf;
                        ClearAllNodeDetailsAndUnequipPanels();
                        currentSlot.detailsAndUnequipGo.SetActive(!active);
                    }
                    else if (m_PrevClickedCrewId != 0 && slot.crewId != m_PrevClickedCrewId)
                    {
                        ClearAllNodeDetailsAndUnequipPanels();
                        EquipCrew(slot, m_PrevClickedCrewId);
                    }



                });
            }
        }

        private void ClearAllNodeDetailsAndUnequipPanels()
        {
            foreach (var slot in m_MountSlots)
            {
                slot.detailsAndUnequipGo.SetActive(false);
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
        private void RemoveDuplicateCrew(int crewID)
        {
            if (crewID == 0)
                return;

            for (int i = 0; i < m_MountSlots.Length; ++i)
            {
                if (m_MountSlots[i].crewId == crewID)
                {
                    m_AirshipEquipController.UnequipSlot(crewID, false);
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

        private UIEquipmentMountSlot FindSlotCrew(int crewId)
        {
            UIEquipmentMountSlot result = null;
            foreach (var slot in m_MountSlots)
            {
                if (slot.crewId == crewId)
                {
                    result = slot;
                    break;
                }
            }
            return result;
        }

        private void ClearAllNodeDetails()
        {            
            foreach (var node in m_CrewListNodeObjects)
            {
                if (node.crewNode.TryGetComponent<UICrewInfoNode>(out var crewNode))
                {
                    crewNode.UIDetails.SetActive(false);
                }
            }
        }

        //private void OnClickedNodeDetails(GameObject targetCrew)
        //{
        //    var ui = GameMgr.FindObject<UiMgr>("UiMgr");
        //    ui.AllPanelsOff();
        //    ui.characterInfoPanel.SetActive(true);
        //    if (ui.characterInfoPanel.TryGetComponent<UICrewInfoPanel>(out var crewInfoPanel))
        //    {
        //        crewInfoPanel.SetClickNode(targetCrew);
        //    }
        //}

        private void OnClickedNodeDetails(int targetCrewId)
        {
            var ui = GameMgr.FindObject<UiMgr>("UiMgr");
            ui.AllPanelsOff();
            ui.characterInfoPanel.SetActive(true);
            if (ui.characterInfoPanel.TryGetComponent<UICrewInfoPanel>(out var crewInfoPanel))
            {
                crewInfoPanel.SetClickNode(targetCrewId);
            }
        }

        // Others

    } // Scope by class UIFortressEquipment
} // namespace SkyDragonHunter