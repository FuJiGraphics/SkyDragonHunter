using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;
using Spine;
using System;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {
    
    public class CrewEquipmentController : MonoBehaviour
    {
        // 필드 (Fields)
        [Tooltip("Mountable Slots와 EquipSlots의 개수는 1:1 대응 되어야 합니다.")]
        [SerializeField] private MountableSlot[] m_MountableSlots;
        [Tooltip("크기만 지정 가능하며, 실제 슬롯의 값은 null이어야 합니다.")]
        [SerializeField] private GameObject[] m_EquipSlots;

        // 속성 (Properties)
        public GameObject[] EquipSlots => m_EquipSlots;
        public bool IsEquip
        {
            get
            {
                bool result = false;
                foreach (var slot in m_MountableSlots)
                {
                    if (!slot.IsEmpty())
                    {
                        result = true;
                        break;
                    }
                }
                return result;
            }
        }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void EquipSlot(int slot, GameObject crewInstance)
        {
            if (crewInstance == null)
            {
                Debug.LogWarning("[CrewEquipmentController]: 장착 실패! crewInstance가 null입니다.");
                return;
            }
            if (slot < 0 || slot >= m_EquipSlots.Length)
                throw new ArgumentOutOfRangeException(nameof(slot), "[CrewEquipmentController]: 슬롯 범위 초과");
            if (m_EquipSlots[slot] == crewInstance)
                return;

            UnequipSlot(slot);
            RemoveSlotCrew(crewInstance);

            if (crewInstance.TryGetComponent<CrewInfoProvider>(out var crewProviderComp))
            {
                crewProviderComp.SetEquipState(true); 
                m_EquipSlots[slot] = crewInstance;
                crewInstance.SetActive(true);

                var equipEvents = crewInstance.GetComponents<ICrewEquipEventHandler>();
                if (equipEvents != null)
                {
                    foreach (var equipEvent in equipEvents)
                    {
                        equipEvent.OnEquip(slot, crewInstance);
                    }
                }
                if (crewInstance.TryGetComponent<NewCrewControllerBT>(out var btComp))
                {
                    btComp.AllocateMountSlot(m_MountableSlots[slot], slot);

                    // 스킬 슬롯에 등록
                    var uiSkillButtons = GameMgr.FindObject<UISkillButtons>("UISkillButtons");
                    uiSkillButtons.Equip(slot, btComp.skillExecutor);
                }
                else
                {
                    Debug.LogError("[CrewEquipmentController]: CrewControllerBT를 찾을 수 없습니다.");
                }
            }
            else
            {
                Debug.LogError("[CrewEquipmentController]: 알 수 없는 오류. CrewInfoProvider를 찾을 수 없습니다.");
            }
            SaveLoadMgr.CallSaveGameData();
        }

        public void UnequipSlot(int slot)
        {
            if (slot < 0 || slot >= m_EquipSlots.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "[CrewEquipmentController]: 슬롯 범위 초과");
            }
            if (m_EquipSlots[slot] == null)
            {
                return;
            }

            if (m_EquipSlots[slot].TryGetComponent<CrewInfoProvider>(out var providerComp))
            {
                if (m_EquipSlots[slot].TryGetComponent<NewCrewControllerBT>(out var btComp))
                {
                    var uiSkillButtons = GameMgr.FindObject<UISkillButtons>("UISkillButtons");
                    uiSkillButtons.Unequip(slot);
                    btComp.m_MountSlot.Dismounting();
                    btComp.MountAction(false);
                }
                else
                {
                    Debug.LogError("[CrewEquipmentController]: CrewControllerBT를 찾을 수 없습니다.");
                }

                var equipEvents = m_EquipSlots[slot].GetComponents<ICrewEquipEventHandler>();
                if (equipEvents != null)
                {
                    foreach (var equipEvent in equipEvents)
                    {
                        equipEvent.OnUnequip(slot);
                    }
                }
                providerComp.SetEquipState(false);
                m_EquipSlots[slot].SetActive(false);
                m_EquipSlots[slot] = null;
            }
            else
            {
                Debug.LogError("[CrewEquipmentController]: 알 수 없는 오류. CrewInfoProvider를 찾을 수 없습니다.");
            }
            SaveLoadMgr.CallSaveGameData();
        }

        public void UnequipSlot(GameObject crewInstance)
        {
            for (int i = 0; i < m_EquipSlots.Length; ++i)
            {
                if (m_EquipSlots[i] == crewInstance)
                {
                    UnequipSlot(i);
                    break;
                }
            }
            SaveLoadMgr.CallSaveGameData();
        }

        public GameObject GetSlotCrew(int slot)
        {
            if (slot < 0 || slot >= m_EquipSlots.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "[CrewEquipmentController]: 슬롯 범위 초과");
            }
            return m_EquipSlots[slot];
        }

        public void RemoveSlotCrew(GameObject crewInstance)
        {
            for (int i = 0; i < m_EquipSlots.Length; ++i)
            {
                if (m_EquipSlots[i] == crewInstance)
                {
                    m_EquipSlots[i] = null;
                    break;
                }
            }
        }

        public bool HasEquipCrew(int id)
        {
            bool result = false;
            for (int i = 0; i < m_EquipSlots.Length; ++i)
            {
                if (m_EquipSlots[i] != null &&
                    m_EquipSlots[i].TryGetComponent<NewCrewControllerBT>(out var btComp))
                {
                    if (btComp.ID == id)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        // Private 메서드
        // Others

    } // Scope by class CrewBoardingController
} // namespace SkyDragonHunter