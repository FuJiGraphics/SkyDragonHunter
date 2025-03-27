using UnityEngine;
using SkyDragonHunter.Utility;
using SkyDragonHunter.Managers;
using SkyDragonHunter.UI;

namespace SkyDragonHunter.Gameplay {
    public class CharacterStatus : MonoBehaviour
    {
        // 필드 (Fields)
        public AlphaUnit maxHP = 100.0;            // 최대 HP
        public AlphaUnit currentHP = 100.0;        // 현재 HP

        public AlphaUnit maxShield = 100.0;        // 최대 방어막
        public AlphaUnit currentShield = 100.0;    // 현재 방어막

        public AlphaUnit maxDamage = 30.0;         // 최대 공격력
        public AlphaUnit currentDamage = 30.0;     // 현재 공격력

        public AlphaUnit maxArmor = 10.0;          // 최대 방어력
        public AlphaUnit currentArmor = 10.0;      // 현재 방어력

        public AlphaUnit maxReilient;              // 최대 회복력
        public AlphaUnit currentReilient;          // 현재 회복력
        
        public float freezeMultiplier = 1.0f;      // 빙결 속도 배율
        public float poisonMultiplier = 1.0f;      // 독 데미지 배율

        // 속성 (Properties)
        public bool IsFullHP => currentHP.Equals(maxHP);
        public bool IsFullShield => currentShield.Equals(currentShield);
        public bool IsFullDamage => currentDamage.Equals(currentDamage);
        public bool IsFullArmor => currentArmor.Equals(currentArmor);
        public bool IsFullReilient => currentReilient.Equals(currentReilient);

        public void SetHP(AlphaUnit value) => currentHP = (value.Value > maxHP) ? maxHP : (value.Value <= 0.0) ? 0.0 : value.Value;
        public void SetShield(AlphaUnit value) => currentShield = (value.Value > maxShield) ? maxShield : (value.Value <= 0.0) ? 0.0 : value.Value;
        public void SetDamage(AlphaUnit value) => currentDamage = (value.Value > maxDamage) ? maxDamage : (value.Value <= 0.0) ? 0.0 : value.Value;
        public void SetArmor(AlphaUnit value) => currentArmor = (value.Value > maxArmor) ? maxArmor : (value.Value <= 0.0) ? 0.0 : value.Value;
        public void SetReilient(AlphaUnit value) => currentReilient = (value.Value > maxReilient) ? maxReilient : (value.Value <= 0.0) ? 0.0 : value.Value;

        // 외부 종속성 필드 (External dependencies field)
        private UIHealthBar m_ShieldBarUI;
        private UIHealthBar m_HealthBarUI;

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
            currentHP = maxHP;
            currentShield = maxShield;
            currentDamage = maxDamage;
            currentArmor = maxArmor;
            currentReilient = maxReilient;
            var bars = GetComponentsInChildren<UIHealthBar>();
            foreach (var bar in bars)
            {
                if (bar.name == "UIShieldBar")
                {
                    m_ShieldBarUI = bar;
                    if (m_ShieldBarUI != null &&
                        !Math2DHelper.Equals(m_ShieldBarUI.maxHealth, maxShield.Value))
                    {
                        m_HealthBarUI.maxHealth = maxShield.Value;
                        m_HealthBarUI.ResetHP();
                    }
                }
                else if (bar.name == "UIHealthBar")
                {
                    m_HealthBarUI = bar;
                    if (m_HealthBarUI != null &&
                        !Math2DHelper.Equals(m_HealthBarUI.maxHealth, maxHP.Value))
                    {
                        m_HealthBarUI.maxHealth = maxHP.Value;
                        m_HealthBarUI.ResetHP();
                    }
                }
            }
        }
        // Others

    } // Scope by class CQharacterStatus
} // namespace Root