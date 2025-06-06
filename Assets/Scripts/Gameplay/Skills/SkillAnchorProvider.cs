using SkyDragonHunter.Interfaces;
using UnityEngine;

namespace SkyDragonHunter.Gameplay
{
    public class SkillAnchorProvider : MonoBehaviour
        , ISkillAnchorProvider
    {
        [Tooltip("스킬 발사되는 위치")]
        public Transform firePoint;

        public Transform GetSkillFirePoint()
            => firePoint;

        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class WeaponAnchorProvider
} // namespace SkyDragonHunter