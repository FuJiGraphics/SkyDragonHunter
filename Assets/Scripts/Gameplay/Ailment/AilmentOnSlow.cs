using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace SkyDragonHunter.Gameplay{

    public class AilmentOnSlow : MonoBehaviour
        , IAilmentLifecycleHandler
    {
        // 필드 (Fields)
        [SerializeField] float m_SlowMultiplier = 0.5f;

        private ISlowable[] m_Slowables = null;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void OnEnter(GameObject receiver)
        {
            DrawableMgr.TopText(receiver.transform.position, "Slow!!!!!", Color.yellow);
            m_Slowables = receiver.GetComponentsInChildren<ISlowable>();
            foreach (var slowable in m_Slowables)
            {
                slowable.OnSlowBegin(m_SlowMultiplier);
            }
        }

        public void OnStay()
        {

        }

        public void OnExit()
        {
            foreach (var slowable in m_Slowables)
            {
                slowable.OnSlowEnd(m_SlowMultiplier);
            }
        }

        // Private 메서드
        // Others

    } // Scope by class AilmentOnSlow
} // namespace SkyDragonHunter