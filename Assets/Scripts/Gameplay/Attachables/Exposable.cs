using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class Exposable : MonoBehaviour
    {
        // 필드 (Fields)
        private BigNum m_ExposeDamageMultiplier = 1;

        // 속성 (Properties)
        public BigNum ExposeDamageMultiplier
        {
            get => m_ExposeDamageMultiplier;
            set
            {
                if (value > 0)
                {
                    m_ExposeDamageMultiplier = value;
                }
                else
                {
                    m_ExposeDamageMultiplier = 1;
                }
            }
        }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class Attackable
} // namespace SkyDragonHunter