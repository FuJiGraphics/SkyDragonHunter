using SkyDragonHunter.Entities;
using SkyDragonHunter.Interfaces;
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
        public void OnEquip(int slotIndex)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnequip(int slotIndex)
        {
            throw new System.NotImplementedException();
        }
    } // Scope by class CrewEquipEventController

} // namespace Root