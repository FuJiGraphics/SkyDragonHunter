using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter {
    public class RecoveryAbility : MonoBehaviour
        , IStateResetHandler
    {
        // 필드 (Fields)
        private CharacterStatus m_Stats;
        private float m_LastTime;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            ResetState();
        }

        private void OnDisable()
        {
            ResetState();
        }

        private void Awake()
        {
            m_Stats = GetComponent<CharacterStatus>(); 
            ResetState();
        }
    
        private void Update()
        {
            if (Time.time > m_LastTime)
            {
                ResetState();
                Recover(m_Stats.Resilient);
            }
        }

        // Public 메서드
        public void Recover(AlphaUnit inc)
        {
            if (!m_Stats.IsFullHealth)
            {
                m_Stats.Health = (m_Stats.Health + inc).Value;

                // TODO: 테스트 용
                DrawableMgr.Text(transform.position, "Recover: " + inc.ToUnit() + "++", Color.green);
            }
            else if (!m_Stats.IsFullShield)
            {
                m_Stats.Shield = (m_Stats.Shield + inc).Value;

                // TODO: 테스트 용
                DrawableMgr.Text(transform.position, "Recover: " + inc.ToUnit() + "++", Color.green);
            }
        }

        public void ResetState()
        {
            m_LastTime = Time.time + 1f;
        }

        // Private 메서드
        // Others

    } // Scope by class RecoveryAbility
} // namespace SkyDragonHunter