using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.UI;
using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {
    public class DamageReceiver : MonoBehaviour
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
        public void TakeDamage(GameObject attacker, AlphaUnit damage)
        {
            m_ShieldBarUI?.TakeDamage(damage.Value);
            double takeDamage = m_Stats.currentShield.Value - damage.Value;
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

            // 죽음 
            UpdateDestructions(attacker);
        }

        private void UpdateDestructions(GameObject attacker)
        {
            if (m_Stats.currentHP.Equals(0.0) || m_Stats.currentHP < 0.0)
            {
                m_Stats.currentHP = 0.0;
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

    } // Scope by class DamageReceiver
} // namespace SkyDragonHunter