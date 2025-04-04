using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
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
                DrawableMgr.Text(transform.position, "Recover: " + inc.ToString() + "++", Color.green);
            }
            else if (!m_Stats.IsFullShield)
            {
                m_Stats.Shield = (m_Stats.Shield + inc).Value;

                // TODO: 테스트 용
                DrawableMgr.Text(transform.position, "Recover: " + inc.ToString() + "++", Color.green);
            }
        }

        // Private 메서드
        // Others

    } // Scope by class RecoveryAbility
} // namespace SkyDragonHunter