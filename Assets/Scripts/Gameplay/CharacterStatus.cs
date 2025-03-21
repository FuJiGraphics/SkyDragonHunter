using UnityEngine;
using SkyDragonHunter.Utility;

namespace SkyDragonHunter.Gameplay {
    public class CharacterStatus : MonoBehaviour
    {
        // 필드 (Fields)
        public AlphaUnit maxHP;
        public AlphaUnit currentHP;
        public AlphaUnit damage;
        public AlphaUnit armor;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            currentHP = maxHP;
        }

        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class CharacterStatus
} // namespace Root