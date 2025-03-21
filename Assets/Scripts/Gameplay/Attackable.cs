using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
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
            AlphaUnit takeDamage = (m_Stats.currentHP - attack.damage);
            m_Stats.currentHP -= takeDamage;
            if (m_Stats.currentHP.Equals(0.0) && m_Stats.currentHP < 0.0)
            {
                m_Stats.currentHP = 0.0;
                // 죽는거 호출
                IDestructible[] destructibles = GetComponentsInChildren<IDestructible>();
                foreach (IDestructible destructible in destructibles)
                {
                    destructible.OnDestruction(attacker);
                }
            }
        }

    } // Scope by class Attackable
} // namespace SkyDragonHunter