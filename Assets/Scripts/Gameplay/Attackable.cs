using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Utility;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace SkyDragonHunter {

    public class Attackable : MonoBehaviour
        , IAttackable
    {
        // 필드 (Fields)
        private CharacterStatus m_Stats;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_Stats = GetComponent<CharacterStatus>();
        }
    
        // Public 메서드
        // Private 메서드
        // Others
        public void OnAttack(GameObject attacker, Attack attack)
        {
            Debug.Log($"attacker: {attacker} -> defender: {gameObject}");

            if (attack.IsCritical)
            {
                DrawableMgr.Text(transform.position, attack.damage.ToString(), Color.red);
            }
            else
            {
                DrawableMgr.Text(transform.position,  attack.damage.ToString());
            }

            double takeDamage = m_Stats.currentShield.Value - attack.damage.Value;
            if (takeDamage >= 0.0)
            {
                m_Stats.SetShield(takeDamage);
                return;
            }
            else
            {
                m_Stats.SetShield(0.0);
            }

            takeDamage *= -1.0;

            takeDamage = m_Stats.currentHP.Value - takeDamage;
            m_Stats.SetHP(takeDamage);

            if (m_Stats.currentHP.Equals(0.0) || m_Stats.currentHP < 0.0)
            {
                m_Stats.currentHP = 0.0;
                // 죽는거 호출
                IDestructible[] destructibles = GetComponentsInChildren<IDestructible>();
                foreach (IDestructible destructible in destructibles)
                {
                    destructible.OnDestruction(attacker);
                }

                Debug.Log("죽었다");
                GameObject.Destroy(gameObject);
            }
        }

    } // Scope by class Attackable
} // namespace SkyDragonHunter