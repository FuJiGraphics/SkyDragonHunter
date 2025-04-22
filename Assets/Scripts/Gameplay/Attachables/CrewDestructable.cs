using SkyDragonHunter.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class CrewDestructable : Destructable
    {
        // 필드 (Fields)
        private NewCrewControllerBT m_CrewController;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_CrewController = GetComponent<NewCrewControllerBT>();
        }

        // Public 메서드
        // Private 메서드
        // Others
        public override void OnDestruction(GameObject attacker)
        {
            if(m_CrewController.IsMounted)
            {
                return;
            }

            m_CrewController.transform.position = m_CrewController.MountSlotPosition;
            m_CrewController.exhaustionRemainingTime = m_CrewController.ExhaustionTime;
            m_CrewController.MountAction(true);
        }

    } // Scope by class CrewDestructable

} // namespace Root