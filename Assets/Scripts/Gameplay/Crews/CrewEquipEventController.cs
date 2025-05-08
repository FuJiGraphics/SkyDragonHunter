using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using UnityEngine;

namespace SkyDragonHunter {

    public class CrewEquipEventController : MonoBehaviour
        , ICrewEquipEventHandler
    {
        private NewCrewControllerBT m_CrewController;

        public void Awake()
        {
            m_CrewController = GetComponent<NewCrewControllerBT>();
        }

        // Public 메서드
        public void OnEquip(int slotIndex, GameObject crewInstance)
        {
            DungeonMgr.RegisterCrew(slotIndex, m_CrewController.ID);
        }

        public void OnUnequip(int slotIndex)
        {
            DungeonMgr.UnregisterCrew(slotIndex);
        }
    } // Scope by class CrewEquipEventController

} // namespace Root