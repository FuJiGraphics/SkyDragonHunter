using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {
    public class EquipAnchorProvider : MonoBehaviour
        , IEquipAnchor
    {
        [Tooltip("원거리 무기 장착 위치")]
        public Transform RangedWeaponDummy;

        public Transform GetRangedWeaponAttachPoint()
            => RangedWeaponDummy;

        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class EquipAnchorProvider
} // namespace SkyDragonHunter