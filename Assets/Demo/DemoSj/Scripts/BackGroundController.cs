using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Game {

    public class BackGroundController : MonoBehaviour
    {
        // 필드 (Fields)
        public float scrollSpeed = 1f;
        public FloatingEffect[] floatingObjects;

        private BackGroundScrolling[] m_Backgrounds;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            m_Backgrounds = GetComponentsInChildren<BackGroundScrolling>();
        }

        private void Update()
        {
            UpdateScrollSpeed();
            UpdateFloatingSpeed();
        }

        // Public 메서드
        // Private 메서드
        private void UpdateScrollSpeed()
        {
            foreach (var back in m_Backgrounds)
            {
                back.scrollSpeed = scrollSpeed;
            }
        }

        private void UpdateFloatingSpeed()
        {
            foreach (var obj in floatingObjects)
            {
                obj.floatSpeedOffset = scrollSpeed;
            }
        }
        // Others

    } // Scope by class BackGroundManaged
}
