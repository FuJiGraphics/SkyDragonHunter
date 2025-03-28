using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
using SkyDragonHunter.UI;
using UnityEngine;

namespace SkyDragonHunter {
    public class DamageReceiver : MonoBehaviour
    {
        // 필드 (Fields)
        private CharacterStatus m_Stats;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        // Public 메서드
        public void TakeDamage(GameObject attacker, AlphaUnit damage)
        {
            double takeDamage = m_Stats.Shield.Value - damage.Value;
            if (takeDamage >= 0.0)
            {
                m_Stats.Shield = takeDamage;
                return;
            }
            else
            {
                m_Stats.Shield = 0.0;
            }

            takeDamage = System.Math.Abs(takeDamage);
            takeDamage = System.Math.Clamp(m_Stats.Health.Value - takeDamage, 0.0, double.MaxValue);
            m_Stats.Health = takeDamage;

            // 죽음 
            UpdateDestructions(attacker);
        }

        private void UpdateDestructions(GameObject attacker)
        {
            if (m_Stats.Health.Equals(0.0) || m_Stats.Health < 0.0)
            {
                m_Stats.Health = 0.0;
                // 죽는거 호출
                IDestructible[] destructibles = GetComponentsInChildren<IDestructible>();
                foreach (IDestructible destructible in destructibles)
                {
                    destructible.OnDestruction(attacker);
                }

                if (destructibles.Length == 0)
                { 
                    Debug.Log("죽었다");
                    StopAllCoroutines();
                    GameObject.Destroy(gameObject);
                }
            }
        }

        // Private 메서드
        private void Init()
        {
            m_Stats = GetComponent<CharacterStatus>();
        }

        // Others

    } // Scope by class DamageReceiver
} // namespace SkyDragonHunter