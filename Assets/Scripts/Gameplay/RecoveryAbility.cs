using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Utility;
using UnityEngine;

namespace SkyDragonHunter {
    public class RecoveryAbility : MonoBehaviour
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
            m_LastTime = Time.time + 1f;
        }

        private void OnDisable()
        {
            m_LastTime = Time.time + 1f;
        }

        private void Awake()
        {
            m_Stats = GetComponent<CharacterStatus>();
            m_LastTime = Time.time + 1f;
        }
    
        private void Update()
        {
            if (Time.time > m_LastTime)
            {
                m_LastTime = Time.time + 1f;
                Recover(m_Stats.currentReilient);
            }
        }

        // Public 메서드
        public void Recover(AlphaUnit inc)
        {
            if (!m_Stats.IsFullShield)
            {
                m_Stats.SetShield(m_Stats.currentShield + inc);
            }
            else if (!m_Stats.IsFullHP)
            {
                m_Stats.SetHP(m_Stats.currentHP + inc);
            }
        }


        // Private 메서드
        // Others

    } // Scope by class RecoveryAbility
} // namespace SkyDragonHunter