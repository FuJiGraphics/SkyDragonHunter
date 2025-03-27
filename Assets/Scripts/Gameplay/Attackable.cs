using NPOI.SS.Formula.Functions;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.UI;
using SkyDragonHunter.Utility;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace SkyDragonHunter {

    public class Attackable : MonoBehaviour
        , IAttackable
    {
        // 필드 (Fields)
        private CharacterStatus m_Stats;
        private UIHealthBar m_ShieldBarUI;
        private UIHealthBar m_HealthBarUI;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            Init();
        }

        // Public 메서드
        // Private 메서드
        private void Init()
        {
            m_Stats = GetComponent<CharacterStatus>();
            var bars = GetComponentsInChildren<UIHealthBar>();
            foreach (var bar in bars)
            {
                if (bar.name == "UIShieldBar")
                {
                    m_ShieldBarUI = bar;
                }
                else if (bar.name == "UIHealthBar")
                {
                    m_HealthBarUI = bar;
                }
            }
        }

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

            m_ShieldBarUI?.TakeDamage(attack.damage.Value);
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

            takeDamage = System.Math.Abs(takeDamage);
            m_HealthBarUI?.TakeDamage(takeDamage);

            takeDamage = System.Math.Clamp(m_Stats.currentHP.Value - takeDamage, 0.0, double.MaxValue);
            m_Stats?.SetHP(takeDamage);

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
                if(destructibles.Length == 0)
                GameObject.Destroy(gameObject);
            }
        }

    } // Scope by class Attackable
} // namespace SkyDragonHunter