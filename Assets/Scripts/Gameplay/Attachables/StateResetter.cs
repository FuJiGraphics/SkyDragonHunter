using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class StateResetter : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private IStateResetHandler[] m_ResetHandlers;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            m_ResetHandlers = GetComponentsInChildren<IStateResetHandler>();
        }

        // Public 메서드
        public void ResetAllState()
        {
            if (m_ResetHandlers == null)
                return;

            foreach (var handler in m_ResetHandlers)
            {
                handler.ResetState();
            }
        }

        // Private 메서드
        // Others

    } // Scope by class StateResetter
} // namespace SkyDragonHunter