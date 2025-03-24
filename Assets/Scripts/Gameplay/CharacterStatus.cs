using UnityEngine;
using SkyDragonHunter.Utility;
using SkyDragonHunter.Managers;

namespace SkyDragonHunter.Gameplay {
    public class CharacterStatus : MonoBehaviour
    {
        // 필드 (Fields)
        public AlphaUnit maxHP = 100.0;             // 최대 HP
        public AlphaUnit currentHP = 100.0;         // 현재 HP

        public AlphaUnit maxShield = 100.0;         // 최대 방어막
        public AlphaUnit currentShield = 100.0;     // 현재 방어막

        public AlphaUnit maxDamage = 50.0;         // 최대 공격력
        public AlphaUnit currentDamage = 50.0;     // 현재 공격력

        public AlphaUnit maxArmor = 10.0;          // 최대 방어력
        public AlphaUnit currentArmor = 10.0;      // 현재 방어력

        public AlphaUnit maxReilient;              // 최대 회복력
        public AlphaUnit currentReilient;          // 현재 회복력

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
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            currentHP = maxHP;
            currentShield = maxShield;
            currentDamage = maxDamage;
            currentArmor = maxArmor;
            currentReilient = maxReilient;

            var go = GameMgr.FindObject("Player");
            Debug.Log(go);
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class CQharacterStatus
} // namespace Root