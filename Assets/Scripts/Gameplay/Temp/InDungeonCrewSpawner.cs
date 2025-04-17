using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using UnityEngine;

namespace SkyDragonHunter {

    public class InDungeonCrewSpawner : MonoBehaviour
    {
        // Fields
        private CrewPrefabLoader m_PrefabLoader;
        private CrewEquipmentController m_AirshipEquipController;
        [SerializeField] private MountableSlot[] m_MountSlots;

        // Unity Methods
        private void Start()
        {
            Init();            
        }

        // Private Methods
        private void Init()
        {
            m_PrefabLoader = GameMgr.FindObject("CrewPrefabLoader").GetComponent<CrewPrefabLoader>();
            m_AirshipEquipController = GameMgr.FindObject("Airship").GetComponent<CrewEquipmentController>();
            //SetCrews();
            TestSetCrews();
        }

        private void SetCrews()
        {
            foreach (var crewSlot in DungeonMgr.CrewSlots)
            {
                if (crewSlot == 0)
                    continue;

                var crewBT = Instantiate(m_PrefabLoader.GetCrewController(crewSlot), Vector3.zero, Quaternion.identity);
                m_AirshipEquipController.EquipSlot(crewSlot, crewBT.gameObject);
            }
        }

        private void TestSetCrews()
        {
            var testCrewIdArr = new int[4];
            
            for(int i = 0; i < 4; ++i)
            {
                testCrewIdArr[i] = 100001 + i;
            }

            foreach (var crewSlot in testCrewIdArr)
            {
                if (crewSlot == 0)
                    continue;

                var crewBT = Instantiate(m_PrefabLoader.GetCrewController(crewSlot), Vector3.zero, Quaternion.identity);
                m_AirshipEquipController.EquipSlot(crewSlot, crewBT.gameObject);
            }
        }
    } // Scope by class InDungeonCrewSpawner

} // namespace Root