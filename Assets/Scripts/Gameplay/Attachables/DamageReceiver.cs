using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Structs;
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
        public void TakeDamage(GameObject attacker, BigNum damage)
        {
            //// TODO: LJH
            if (m_Stats.Health <= 0)
                return;
            //// ~TODO

            if (TryGetComponent<DamageBlink>(out var blinkComp))
            {
                blinkComp.OnHit();
            }

            // TODO: AlphaUnit Convert
            if (m_Stats.Shield >= damage)
            {
                m_Stats.Shield -= damage;
                return;
            }
            else
            {
                m_Stats.Shield = 0;
                damage -= m_Stats.Shield;
            }

            m_Stats.Health -= damage;

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