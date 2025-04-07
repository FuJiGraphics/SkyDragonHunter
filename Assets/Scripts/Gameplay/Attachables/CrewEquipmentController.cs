using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {
    public class CrewEquipmentController : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private GameObject[] m_EquipSlots;

        // 속성 (Properties)
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
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "[CrewEquipmentController]: 슬롯 범위 초과");
            }

            UnequipSlot(slot);

            if (crewInstance.TryGetComponent<CrewInfoProvider>(out var crewProviderComp))
            {
                crewProviderComp.SetEquipState(true); 
                m_EquipSlots[slot] = crewInstance;
            }
            else
            {
                Debug.LogError("[CrewEquipmentController]: 알 수 없는 오류. CrewInfoProvider를 찾을 수 없습니다.");
            }

            AccountMgr.SaveUserData();
        }

        public void UnequipSlot(int slot)
        {
            if (slot < 0 || slot >= m_EquipSlots.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), "[CrewEquipmentController]: 슬롯 범위 초과");
            }

            if (m_EquipSlots[slot] != null && m_EquipSlots[slot].TryGetComponent<CrewInfoProvider>(out var providerComp))
            {
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

        // Private 메서드
        // Others

    } // Scope by class CrewBoardingController
} // namespace SkyDragonHunter