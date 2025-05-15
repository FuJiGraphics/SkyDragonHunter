using SkyDragonHunter.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.UI {

    public class FacilityPanelController : MonoBehaviour
    {
        // 필드 (Fields)
        [Header("Facility Control Panel")]
        [SerializeField] private FacilitySystemMgr systemMgr;
        [SerializeField] private List<FacilitySlotHandler> slotHandlers;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            OnOpenFacilityPanel();
        }

        private void Update()
        {
            //systemMgr.UpdateTimers(Time.deltaTime);
            foreach (var slot in slotHandlers)
                slot.UpdateUI();
        }

        // Public 메서드
        public void OnOpenFacilityPanel()
        {
            foreach (var slot in slotHandlers)
            {
                var data = systemMgr.GetFacility(slot.GetFacilityType());
                var saved = SaveLoadMgr.GameData.savedFacilityData.GetSavedFacility(data.type);
                data.level = saved.level;
                data.isUpgrading = saved.isUpgrading;
                if(data.isUpgrading)
                {
                    data.upgradeStartedTime = saved.upgradeStartTime;
                }
                else
                {
                    data.upgradeStartedTime = DateTime.UtcNow;
                }
                data.lastAccquiredTime = saved.lastAcquiredTime == DateTime.MinValue ? DateTime.UtcNow : saved.lastAcquiredTime;
                slot.Initialize(data);
            }
        }

        public void OnAllSlotAcquire()
        {
            foreach (var slot in slotHandlers)
            {
                var data = systemMgr.GetFacility(slot.GetFacilityType());
                slot.OnClickAcquire();
            }
        }
        // Private 메서드
        // Others

    } // Scope by class FacilityPanelController

} // namespace Root