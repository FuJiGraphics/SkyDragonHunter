using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using System.Numerics;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

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
            //// TODO: LJH
            if (m_Stats.Health <= 0)
                return;
            //// ~TODO
            
            // ----- 약화 상태 이상 계산
            if (TryGetComponent<Exposable>(out var exposableComp))
            {
                damage *= exposableComp.ExposeDamageMultiplier;
            }
            // -----

            BigInteger takeDamage = m_Stats.Shield.Value - damage.Value;
            if (takeDamage >= 0)
            {
                m_Stats.Shield = takeDamage;
                return;
            }
            else
            {
                m_Stats.Shield = 0;
            }

            takeDamage = Math2DHelper.Abs(takeDamage);
            takeDamage = Math2DHelper.Clamp(m_Stats.Health.Value - takeDamage, 0, m_Stats.Health.Value);
            m_Stats.Health = takeDamage;

            // 죽음 
            UpdateDestructions(attacker);
        }

        private void UpdateDestructions(GameObject attacker)
        {
            if (m_Stats.Health <= 0)
            {
                m_Stats.Health = 0;
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