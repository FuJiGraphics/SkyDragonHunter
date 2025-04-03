using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{
    public class LookAtable : MonoBehaviour
        , ILookAtProvider
    {
        // 필드 (Fields)
        [SerializeField] private Transform spinner;
        [SerializeField] private Transform start;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        // Private 메서드
        // Others
        public void LookAt(GameObject target)
        {
            Math2DHelper.LookAt(spinner, start, target.transform);
        }

    } // Scope by class LookAtable
} // namespace Root
