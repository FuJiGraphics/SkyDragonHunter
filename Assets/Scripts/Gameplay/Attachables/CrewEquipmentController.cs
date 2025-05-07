using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SkyDragonHunter {
    
    public class CrewEquipmentController : MonoBehaviour
    {
        // 필드 (Fields)
        [Tooltip("Mountable Slots와 EquipSlots의 개수는 1:1 대응 되어야 합니다.")]
        [SerializeField] private MountableSlot[] m_MountableSlots;
        [Tooltip("크기만 지정 가능하며, 실제 슬롯의 값은 null이어야 합니다.")]
        [SerializeField] private GameObject[] m_EquipSlots;
        // TODO: LJH
        private int[] m_EquipIdSlots = new int[4];
        private const string c_CrewPrefabDirectory = "Prefabs/Crews/";
        public int[] EquipIdSlots => m_EquipIdSlots;
        // ~TODO

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
        //public void EquipSlot(int slot, GameObject crewInstance)
        //{
        //    if (crewInstance == null)
        //    {
        //        Debug.LogWarning("[CrewEquipmentController]: 장착 실패! crewInstance가 null입니다.");
        //        return;
        //    }
        //    if (slot < 0 || slot >= m_EquipSlots.Length)
        //        throw new ArgumentOutOfRangeException(nameof(slot), "[CrewEquipmentController]: 슬롯 범위 초과");
        //    if (m_EquipSlots[slot] == crewInstance)
        //        return;
        //
        //    UnequipSlot(slot);
        //    RemoveSlotCrew(crewInstance);
        //
        //    if (crewInstance.TryGetComponent<CrewInfoProvider>(out var crewProviderComp))
        //    {
        //        crewProviderComp.SetEquipState(true);
        //        m_EquipSlots[slot] = crewInstance;
        //        crewInstance.SetActive(true);
        //        if (crewInstance.TryGetComponent<ICrewEquipEventHandler>(out var equipEventController))
        //        {
        //            equipEventController.OnEquip(slot);
        //        }
        //        if (crewInstance.TryGetComponent<NewCrewControllerBT>(out var btComp))
        //        {
        //            btComp.AllocateMountSlot(m_MountableSlots[slot], slot);
        //        }
        //        else
        //        {
        //            Debug.LogError("[CrewEquipmentController]: CrewControllerBT를 찾을 수 없습니다.");
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogError("[CrewEquipmentController]: 알 수 없는 오류. CrewInfoProvider를 찾을 수 없습니다.");
        //    }
        //
        //    AccountMgr.SaveUserData();
        //}

        public void EquipSlot(int slot, int crewId)
        {
            if (!SaveLoadMgr.GameData.savedCrewData.GetSavedCrew(crewId, out var savedCrewData))
            {
                Debug.LogError($"[CrewEquipmentController]: Equipslot '{slot}' failed, Cannot find crew with ID '{crewId}'");
                return;
            }
            if (slot < 0 || slot >= m_EquipSlots.Length)
                throw new ArgumentOutOfRangeException(nameof(slot), "[CrewEquipmentController]: 슬롯 범위 초과");
            if (m_EquipIdSlots[slot] == crewId)
                return;

            UnequipSlot(slot);
            RemoveSlotCrew(crewId);

            var path = Path.Combine(c_CrewPrefabDirectory, crewId.ToString());
            var crewInstance = Addressables.InstantiateAsync(path).WaitForCompletion();
            crewInstance.SetActive(true);
            if (crewInstance.TryGetComponent<ICrewEquipEventHandler>(out var equipEventController))
            {
                equipEventController.OnEquip(slot);
            }
            if (crewInstance.TryGetComponent<NewCrewControllerBT>(out var btComp))
            {
                btComp.AllocateMountSlot(m_MountableSlots[slot], slot);
            }
            else
            {
                Debug.LogError("[CrewEquipmentController]: CrewControllerBT를 찾을 수 없습니다.");
            }
            
            var crewBT = crewInstance.GetComponent<NewCrewControllerBT>();
            SaveLoadMgr.GameData.savedCrewData.GetCrewLevel(crewId, out var level);
            var crewStatus = crewBT.SetDataFromTableWithExistingIDTemp(level);
            var accountInfoHandler = crewInstance.GetComponent<CrewAccountStatInfoHandler>();
            accountInfoHandler.Init(crewStatus);

            m_EquipSlots[slot] = crewInstance;
            m_EquipIdSlots[slot] = crewId;

            AccountMgr.SaveUserData();
        }

        public void UnequipSlot(int slot, bool isSlotIndex = true)
        {
            if(!isSlotIndex)
            {
                UnequipSlotWithCrewId(slot);
                return;
            }
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
                    btComp.m_MountSlot.Dismounting();
                    btComp.MountAction(false);
                }
                else
                {
                    Debug.LogError("[CrewEquipmentController]: CrewControllerBT를 찾을 수 없습니다.");
                }

                if (m_EquipSlots[slot].TryGetComponent<ICrewEquipEventHandler>(out var equipEventController))
                {
                    equipEventController.OnEquip(slot);
                }
                providerComp.SetEquipState(false);
                m_EquipSlots[slot].SetActive(false);
                m_EquipSlots[slot] = null;
            }
            else
            {
                Debug.LogError("[CrewEquipmentController]: 알 수 없는 오류. CrewInfoProvider를 찾을 수 없습니다.");
            }
            AccountMgr.SaveUserData();
        }

        private void UnequipSlotWithCrewId(int crewId)
        {
            for (int i = 0; i < m_EquipIdSlots.Length; ++i)
            {
                if (m_EquipIdSlots[i] == crewId)
                {
                    UnequipSlot(i);
                    break;
                }
            }
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
        }

        public void UnEquipSlotWithIndex(int slot)
        {
            if (slot < 0 || slot >= m_EquipSlots.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "[CrewEquipmentController]: 슬롯 범위 초과");
            }
            if (m_EquipIdSlots[slot] == 0)
            {
                return;
            }
        
            if (m_EquipSlots[slot].TryGetComponent<CrewInfoProvider>(out var providerComp))
            {
                if (m_EquipSlots[slot].TryGetComponent<NewCrewControllerBT>(out var btComp))
                {
                    btComp.m_MountSlot.Dismounting();
                    btComp.MountAction(false);
                }
                else
                {
                    Debug.LogError("[CrewEquipmentController]: CrewControllerBT를 찾을 수 없습니다.");
                }
        
                if (m_EquipSlots[slot].TryGetComponent<ICrewEquipEventHandler>(out var equipEventController))
                {
                    equipEventController.OnEquip(slot);
                }
                providerComp.SetEquipState(false);
                m_EquipSlots[slot].SetActive(false);
                m_EquipSlots[slot] = null;
            }
            else
            {
                Debug.LogError("[CrewEquipmentController]: 알 수 없는 오류. CrewInfoProvider를 찾을 수 없습니다.");
            }
            AccountMgr.SaveUserData();
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

        public void RemoveSlotCrew(int crewId)
        {
            for(int i = 0; i < m_EquipIdSlots.Length; ++i)
            {
                if (m_EquipIdSlots[i] == crewId)
                {
                    m_EquipIdSlots[i] = 0;
                    m_EquipSlots[i] = null;
                    break;
                }
            }
        }
        // Private 메서드
        // Others

    } // Scope by class CrewBoardingController
} // namespace SkyDragonHunter