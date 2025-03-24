using SkyDragonHunter.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EntityStatus : MonoBehaviour
    {
        // 필드 (Fields)
        private AlphaUnit m_maxHp;
        private AlphaUnit m_damage;
        private AlphaUnit m_armor;
        private AlphaUnit m_regeneration;

        // 속성 (Properties)
        public AlphaUnit HealthPoint { get; set; }
        public float HpPercent
        {
            get
            {
                var percentageAlpha = HealthPoint / m_maxHp;
                float percentage = (float)percentageAlpha.Value;
                return percentage;
            }
        }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            
        }
    
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class EntityStatus

} // namespace Root