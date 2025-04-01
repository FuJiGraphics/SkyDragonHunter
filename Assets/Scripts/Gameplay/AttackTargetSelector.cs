using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{
    public class AttackTargetSelector : MonoBehaviour
        , IAttackTargetProvider
    {
        // 필드 (Fields)
        [SerializeField] private string[] allowedTargetTags;

        // 속성 (Properties)
        public string[] AllowedTargetTags => allowedTargetTags;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class AttackTargetSelector
} // namespace Root
