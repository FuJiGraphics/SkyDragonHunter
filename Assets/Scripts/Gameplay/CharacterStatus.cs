using UnityEngine;
using SkyDragonHunter.Utility;

namespace SkyDragonHunter.Gameplay {
    public class CharacterStatus : MonoBehaviour
    {
        // 필드 (Fields)
        public AlphaUnit maxHP;             // 최대 HP
        public AlphaUnit currentHP;         // 현재 HP

        public AlphaUnit maxShield;         // 최대 방어막
        public AlphaUnit currentShield;     // 현재 방어막

        public AlphaUnit maxDamage;         // 최대 공격력
        public AlphaUnit currentDamage;     // 현재 공격력

        public AlphaUnit maxArmor;          // 최대 방어력
        public AlphaUnit currentArmor;      // 현재 방어력

        public AlphaUnit maxReilient;       // 최대 회복력
        public AlphaUnit currentReilient;   // 현재 회복력

        // 속성 (Properties)
        public bool IsFullHP => currentHP.Equals(maxHP);
        public bool IsFullShield => currentShield.Equals(currentShield);
        public bool IsFullDamage => currentDamage.Equals(currentDamage);
        public bool IsFullArmor => currentArmor.Equals(currentArmor);
        public bool IsFullReilient => currentReilient.Equals(currentReilient);

        public void SetHP(AlphaUnit value) => currentHP = (currentHP.Value + value.Value > maxHP) ? maxHP : currentHP.Value + value.Value;
        public void SetShield(AlphaUnit value) => currentShield = (currentShield.Value + value.Value > maxShield) ? maxShield : currentShield.Value + value.Value;
        public void SetDamage(AlphaUnit value) => currentDamage = (currentDamage.Value + value.Value > maxDamage) ? maxDamage : currentDamage.Value + value.Value;
        public void SetArmor(AlphaUnit value) => currentArmor = (currentArmor.Value + value.Value > maxArmor) ? maxArmor : currentArmor.Value + value.Value;
        public void SetReilient(AlphaUnit value) => currentReilient = (currentReilient.Value + value.Value > maxReilient) ? maxReilient : currentReilient.Value + value.Value;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            currentHP = maxHP;
            currentShield = maxShield;
            currentDamage = maxDamage;
            currentArmor = maxArmor;
            currentReilient = maxReilient;
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class CQharacterStatus
} // namespace Root